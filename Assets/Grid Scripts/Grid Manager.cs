using System;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private GridScriptableObject gridInfo;

    private ItemScriptableObject[] itemsInGrid;

    private bool[,,] occupiedStatus;

    void Start()
    {
        if (gridInfo == null)
        {
            Debug.LogError("Grid Scriptable Object not assigned!");
            return;
        }

        occupiedStatus = new bool[gridInfo.GridSize.x, gridInfo.GridSize.y, gridInfo.GridSize.z];

        GenerateGrid();
    }

    void Update()
    {
        
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < gridInfo.GridSize.x; x++)
        {
            for (int y = 0; y < gridInfo.GridSize.y; y++)
            {
                for (int z = 0; z < gridInfo.GridSize.z; z++)
                {
                    // Temp create cube to show grid
                    Vector3 position = new Vector3(transform.position.x + x,
                                                   transform.position.y + y,
                                                   transform.position.z + z);
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = position;
                    cube.name = $"Cell_{x}_{y}_{z}";
                    cube.transform.localScale = new Vector3(0.2f,0.2f,0.2f);

                    // Here, we'll assume all cells are unoccupied initially, 
                    // but you could also initialize their status.
                }
            }
        }
    }

    private bool AddItem(ItemScriptableObject item, Vector3Int position) {
        
        return false;
    }

    private bool RemoveItem(ItemScriptableObject item, Vector3Int position) {
        return false;
    }

    // Getter for the occupied status of a specific cell
    public bool IsCellOccupied(Vector3Int position)
    {
        // Check if the position is within the bounds of the grid
        if (gridInfo.IsWithinBounds(position))
        {
            return occupiedStatus[position.x, position.y, position.z];
        }
        return false;
    }

    // Setter for marking a cell as occupied or unoccupied
    public void SetCellOccupied(Vector3Int position, bool isOccupied)
    {
        if (gridInfo.IsWithinBounds(position))
        {
            occupiedStatus[position.x, position.y, position.z] = isOccupied;
        }
    }
}
