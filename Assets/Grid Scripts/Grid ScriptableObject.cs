using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Grid Scriptable Object", menuName = "Scriptable Objects/New Grid")]
public class GridScriptableObject : ScriptableObject
{
    [SerializeField]
    private Vector3Int gridSize;
    public Vector3Int GridSize => gridSize;

    private bool[,,] occupiedStatus;

    // Getter for the occupied status of a specific cell
    public bool IsCellOccupied(Vector3Int position)
    {
        // Check if the position is within the bounds of the grid
        if (IsWithinBounds(position))
        {
            return occupiedStatus[position.x, position.y, position.z];
        }
        return false;
    }

    // Setter for marking a cell as occupied or unoccupied
    public void SetCellOccupied(Vector3Int position, bool isOccupied)
    {
        if (IsWithinBounds(position))
        {
            occupiedStatus[position.x, position.y, position.z] = isOccupied;
        }
    }

    public bool IsWithinBounds(Vector3Int position)
    {
        return position.x >= 0 && position.x < gridSize.x &&
               position.y >= 0 && position.y < gridSize.y &&
               position.z >= 0 && position.z < gridSize.z;
    }

    private void OnEnable()
    {
        occupiedStatus = new bool[gridSize.x, gridSize.y, gridSize.z];
    }
}
