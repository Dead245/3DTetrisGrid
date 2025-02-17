using UnityEngine;
using GridSystem.Items;
using GridSystem.PickupLogic;
using System.Collections.Generic;


namespace GridSystem.Core
{
    [RequireComponent(typeof(BoxCollider))]
    public class GridManager : MonoBehaviour
    {
        [SerializeField]
        private GridScriptableObject gridInfo;
        [SerializeField]
        private float cellSize = 1f;

        private Dictionary<Vector3Int,GameObject> itemsInGrid = new();
        private List<Vector3Int> occupiedStatus = new();

        public BoxCollider boxCollider;
        private float maxSearchDistance = 3f;

        private Collider intersectingItem;
        public Vector3? closestSnapPoint = null;

        public void OnEnable()
        {
            Vector3 sizeVector = gridInfo.GridSize;
            boxCollider = GetComponent<BoxCollider>();
            boxCollider.size = sizeVector * cellSize;
            boxCollider.isTrigger = true;
        }
        void Start() {
            if (gridInfo == null)
            {
                Debug.LogWarning("Grid Scriptable Object not assigned!");
                return;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            //Ignore items that are inside the grid already
            foreach (var item in itemsInGrid.Values) {
                if (other.gameObject.Equals(item)) return;
            }
            ItemManager itemMang;
            Pickup pickupScript;
            if (other.TryGetComponent<ItemManager>(out itemMang)) {
                pickupScript = Camera.main.GetComponentInParent<Pickup>();
                if (pickupScript.GrabbedItem != null)
                {
                    if (pickupScript.GrabbedItem.Equals(other.gameObject))
                    {
                        intersectingItem = other;
                        closestSnapPoint = GetNearestEmptyCell(itemMang,pickupScript.itemGrabPointTransform.position);
                        pickupScript.interactingGridManager = this;
                        itemMang.gridManager = this;
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.Equals(intersectingItem)) {
                Camera.main.GetComponentInParent<Pickup>().interactingGridManager = null;
                intersectingItem = null;
                //Parenting the item makes the trigger volume not recognize it aka "exit" it, so we do this:
                foreach (var item in itemsInGrid.Values) {
                    if (other.gameObject.Equals(item)) return;
                }
                other.transform.SetParent(null);
                other.GetComponent<ItemManager>().gridManager = null;
            }
        }

        public void SetGridActive(bool activeState) {
            gameObject.SetActive(activeState);
        }

        public bool AddItem(GameObject item, Vector3Int cellPosition) {
            //Check if every cell of the item can fit.
            if (!CheckItem(item.GetComponent<ItemManager>(), cellPosition)) {
                return false;
            }
            foreach (var cell in item.GetComponent<ItemManager>().rotatedOffsets) {
                SetCellOccupied(cell + cellPosition, true);
            }
            item.transform.SetParent(transform);
            item.GetComponent<Rigidbody>().isKinematic = true;
            itemsInGrid.Add(cellPosition,item);
            ItemManager itemManager = item.GetComponent<ItemManager>();
            itemManager.gridCellOrigin = cellPosition;
            return true;
        }

        public bool RemoveItem(Vector3Int cellPosition) {
            ItemManager itemManager = itemsInGrid[cellPosition].GetComponent<ItemManager>();
            itemManager.gridCellOrigin = new Vector3Int();
            itemsInGrid.Remove(cellPosition);
            foreach (var cell in itemManager.rotatedOffsets) {
                SetCellOccupied(cell + cellPosition, false);
            }
            itemManager.gridManager = null;
            return true;
        }

        private bool CheckItem(ItemManager itemMang, Vector3Int cellPos) {
            foreach (var cell in itemMang.rotatedOffsets) {
                Vector3 floatCell = cell;
                if (!GridContains(itemMang.transform.position + (floatCell * cellSize))) return false;
                if (IsCellOccupied(cell + cellPos)) return false;
            }
            return true;
        }

        private bool GridContains(Vector3 itemPos) {
            // Transform the point into the local space of the BoxCollider
            Vector3 localPoint = boxCollider.transform.InverseTransformPoint(itemPos);

            // Get the local bounds of the BoxCollider
            Vector3 center = boxCollider.center;
            Vector3 size = boxCollider.size / 2f;

            // Check if the point is inside the local bounds
            return (localPoint.x >= center.x - size.x && localPoint.x <= center.x + size.x &&
                    localPoint.y >= center.y - size.y && localPoint.y <= center.y + size.y &&
                    localPoint.z >= center.z - size.z && localPoint.z <= center.z + size.z);
        }

        #region Cell Functions
        private bool IsCellOccupied(Vector3Int position) {
            if (gridInfo.IsWithinBounds(position))
            {
                if (occupiedStatus != null)
                {
                    return occupiedStatus.Contains(position);
                }
            }
            return false;
        }
        private void SetCellOccupied(Vector3Int position, bool isOccupied) {
            if (gridInfo.IsWithinBounds(position))
            {
                if (isOccupied)
                {
                    occupiedStatus.Add(position);
                }
                else
                {
                    occupiedStatus.Remove(position);
                }

                return;
            }
        }
        public Vector3? GetNearestEmptyCell(ItemManager itemManager, Vector3 position) {
            Vector3? pos = GetNearestCell(position);
            if (pos == null) {
                return null;
            }
            position = (Vector3)pos;

            Queue<Vector3> toVisit = new Queue<Vector3>();
            HashSet<Vector3Int> visitedCells = new HashSet<Vector3Int>();

            toVisit.Enqueue(position);

            while (toVisit.Count > 0) {
                Vector3 current = toVisit.Dequeue();
                Vector3Int cell = GetCell(current);

                // Check if cell is valid and not occupied
                if (GridContains(current) && !visitedCells.Contains(cell)) {
                    if (CheckItem(itemManager,cell)) {
                        return current;
                    }

                    visitedCells.Add(cell);

                    // Neighboring cells
                    Vector3[] neighbors = {
                        current + new Vector3(cellSize, 0, 0),
                        current + new Vector3(-cellSize, 0, 0),
                        current + new Vector3(0, cellSize, 0),
                        current + new Vector3(0, -cellSize, 0),
                        current + new Vector3(0, 0, cellSize),
                        current + new Vector3(0, 0, -cellSize)
                    };
                    foreach (var neighbor in neighbors) {
                        if (!visitedCells.Contains(GetCell(neighbor)) && Vector3.Distance(position, neighbor) <= maxSearchDistance) {
                            toVisit.Enqueue(neighbor);
                        }
                    }
                }
            }
            return null; // Fallback if no empty cell found
        }
        public Vector3? GetNearestCell(Vector3 position) {
            if (!GridContains(position))
            {
                //Position is outside the grid bounds, return an invalid position
                return null;
            }
            Vector3 localPos = position - transform.position;
            float invCellSize = 1f / cellSize;
            int x = Mathf.RoundToInt(localPos.x * invCellSize);
            int y = Mathf.RoundToInt(localPos.y * invCellSize);
            int z = Mathf.RoundToInt(localPos.z * invCellSize);
            Vector3 nearestCell = new Vector3(x * cellSize, y * cellSize, z * cellSize) + transform.position;
            return nearestCell;
        }
        public Vector3Int GetCell(Vector3 position) {
            Vector3 calc = (position - transform.position) / cellSize;
            Vector3Int cellLocation = Vector3Int.FloorToInt(calc);
            return cellLocation;
        }
        #endregion
    }
}
