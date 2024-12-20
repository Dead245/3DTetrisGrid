using UnityEngine;
using UnityEngine.Rendering;

namespace GridSystem.Core
{
    [CreateAssetMenu(fileName = "Grid Scriptable Object", menuName = "Scriptable Objects/New Grid")]
    public class GridScriptableObject : ScriptableObject
    {
        [SerializeField]
        private Vector3Int gridSize;
        public Vector3Int GridSize => gridSize;

        public bool IsWithinBounds(Vector3Int position)
        {
            return position.x >= 0 && position.x < (gridSize.x / 2) - 1 &&
                   position.y >= 0 && position.y < (gridSize.y / 2) - 1 &&
                   position.z >= 0 && position.z < (gridSize.z / 2) - 1;
        }
    }
}
