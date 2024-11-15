using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Grid Scriptable Object", menuName = "Scriptable Objects/New Grid")]
public class GridScriptableObject : ScriptableObject
{
    [SerializeField]
    private Vector3Int gridSize;
    public Vector3Int GridSize => gridSize;

    public bool IsWithinBounds(Vector3Int position)
    {
        return position.x >= 0 && position.x < gridSize.x &&
               position.y >= 0 && position.y < gridSize.y &&
               position.z >= 0 && position.z < gridSize.z;
    }
}
