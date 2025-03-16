using UnityEngine;

namespace GridSystem
{
    public class MasterGridScript
    {
        //For variables that need to be consistent across multiple scripts
        private const float cellSize = 0.1f;
        public float CellSize => cellSize;
    }
}
