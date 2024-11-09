using System;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private GridScriptableObject gridInfo;

    private ItemScriptableObject[] itemsInGrid;

    void Start()
    {
        if (gridInfo == null)
        {
            Debug.LogError("Grid Scriptable Object not assigned!");
            return;
        }

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
                    Vector3 position = new Vector3(x, y, z);
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

}
