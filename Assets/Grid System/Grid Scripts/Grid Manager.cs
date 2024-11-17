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

        private Dictionary<Vector3Int,ItemScriptableObject> itemsInGrid = new();
        private bool[,,] occupiedStatus;

        private Bounds gridBounds;

        void Start()
        {
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

        private bool AddItem(ItemScriptableObject item, Vector3Int position)
        {
            return false;
        }

        private bool RemoveItem(ItemScriptableObject item, Vector3Int position)
        {
            return false;
        }

        public bool IsCellOccupied(Vector3Int position)
        {
            // Check if the position is within the bounds of the grid
            if (gridInfo.IsWithinBounds(position))
            {
                return occupiedStatus[position.x, position.y, position.z];
            }
            return false;
        }

        public void SetCellOccupied(Vector3Int position, bool isOccupied)
        {
            if (gridInfo.IsWithinBounds(position))
            {
                occupiedStatus[position.x, position.y, position.z] = isOccupied;
            }
        }

        public Vector3 GetNearestCell(Vector3 position)
        {
            if (!gridBounds.Contains(position))
            {
                //Position is outside the grid bounds, return an "invalid" position
                return new Vector3 (1000000,1000000,1000000); //gotta be a better way right?
            }

            Vector3 localPos = position - transform.position;
            int x = Mathf.RoundToInt(localPos.x / cellSize);
            int y = Mathf.RoundToInt(localPos.y / cellSize);
            int z = Mathf.RoundToInt(localPos.z / cellSize);
            return new Vector3(x * cellSize, y * cellSize, z * cellSize) + transform.position;
        }
        
        private void OnDrawGizmos()
        {
            var cellVector = new Vector3(cellSize,cellSize,cellSize);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(gridBounds.center, gridBounds.size);
            Gizmos.DrawWireCube(gridBounds.center, cellVector);
        }
    }
}
