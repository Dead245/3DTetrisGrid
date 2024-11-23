using UnityEngine;
using GridSystem.Items;
using System.Collections.Generic;

namespace GridSystem.Core
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField]
        private GridScriptableObject gridInfo;
        [SerializeField]
        private float cellSize = 1f;

        private Dictionary<Vector3Int,GameObject> itemsInGrid = new();
        private bool[,,] occupiedStatus;

        private Bounds gridBounds;
        private float maxSearchDistance = 3f;

        private Vector3 invalidPos = new Vector3(1000000,1000000,1000000);
        void Start() {
            if (gridInfo == null)
            {
                Debug.LogError("Grid Scriptable Object not assigned!");
                return;
            }

            occupiedStatus = new bool[gridInfo.GridSize.x, gridInfo.GridSize.y, gridInfo.GridSize.z];
            Vector3 boundsVector = new Vector3(cellSize / 2, cellSize / 2, cellSize / 2);
            Vector3 sizeVector = gridInfo.GridSize;
            gridBounds = new Bounds(transform.position, sizeVector * cellSize);
        }

        public bool AddItem(GameObject item, Vector3Int cellPosition) {
            SetCellOccupied(cellPosition, true);
            item.GetComponent<Rigidbody>().isKinematic = true;
            itemsInGrid.Add(cellPosition,item);
            ItemManager itemManager = item.GetComponent<ItemManager>();
            itemManager.gridManager = this;
            itemManager.gridCellOrigin = cellPosition;
            return true;
        }

        public bool RemoveItem(Vector3Int cellPosition) {
            Debug.Log($"Removing item at {cellPosition}");
            ItemManager itemManager = itemsInGrid[cellPosition].GetComponent<ItemManager>();
            itemManager.gridManager = null;
            itemManager.gridCellOrigin = new Vector3Int();
            itemsInGrid.Remove(cellPosition);
            SetCellOccupied(cellPosition, false);
            return true;
        }
        
        #region Cell Functions
        private bool IsCellOccupied(Vector3Int position) {
            // Check if the position is within the bounds of the grid
            if (gridInfo.IsWithinBounds(position))
            {
                return occupiedStatus[position.x, position.y, position.z];
            }
            return false;
        }

        private void SetCellOccupied(Vector3Int position, bool isOccupied) {
            if (gridInfo.IsWithinBounds(position))
            {
                occupiedStatus[position.x, position.y, position.z] = isOccupied;
            }
        }
        public Vector3 GetNearestEmptyCell(Vector3 position) {
            position = GetNearestCell(position);
            if (position == invalidPos) {
                return invalidPos;
            }

            Queue<Vector3> toVisit = new Queue<Vector3>();
            HashSet<Vector3Int> visitedCells = new HashSet<Vector3Int>();

            toVisit.Enqueue(position);

            while (toVisit.Count > 0) {
                Vector3 current = toVisit.Dequeue();
                Vector3Int cell = GetCell(current);

                // Check if cell is valid and not occupied
                if (gridBounds.Contains(current) && !visitedCells.Contains(cell)) {
                    if (!IsCellOccupied(cell)) {
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
            return invalidPos; // Fallback if no empty cell found
        }
        public Vector3 GetNearestCell(Vector3 position) {
            if (!gridBounds.Contains(position))
            {
                //Position is outside the grid bounds, return an "invalid" position
                return invalidPos; //gotta be a better way right?
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

        private void OnDrawGizmos() {
            var cellVector = new Vector3(cellSize,cellSize,cellSize);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(gridBounds.center, gridBounds.size);
            Gizmos.DrawWireCube(gridBounds.center, cellVector);
        }
    }
}
