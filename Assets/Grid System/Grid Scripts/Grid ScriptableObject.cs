using UnityEngine;

namespace GridSystem.Core {
    [CreateAssetMenu(fileName = "Grid Scriptable Object", menuName = "Scriptable Objects/New Grid")]
    public class GridScriptableObject : ScriptableObject {
        [SerializeField]
        private Vector3Int gridSize;
        public Vector3Int GridSize => gridSize;

        public bool IsWithinBounds(Vector3 position) {
            int minX = -gridSize.x / 2;
            int maxX = gridSize.x / 2;
            int minY = -gridSize.y / 2;
            int maxY = gridSize.y / 2;
            int minZ = -gridSize.z / 2;
            int maxZ = gridSize.z / 2;

            return position.x >= minX && position.x <= maxX &&
                   position.y >= minY && position.y <= maxY &&
                   position.z >= minZ && position.z <= maxZ;
        }
    }
}
