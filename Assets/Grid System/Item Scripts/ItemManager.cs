using UnityEngine;
using GridSystem.Core;
using PlasticGui;

namespace GridSystem.Items
{
    [SelectionBase]
    public class ItemManager : MonoBehaviour
    {
        public GridManager gridManager; //For when it supposed to be in a grid.
        public Vector3Int gridCellOrigin;
        //Eventually remove the need to set the item beforehand.
        [SerializeField]
        private ItemScriptableObject item;
        [SerializeField]
        private float itemSize = 0.5f;
        public ItemScriptableObject Item => item;
        //[TODO] Serialized Field of the 3D model of the item

        private void OnEnable()
        {
            GenerateItem();
        }

        private void GenerateItem()
        {
            if (item == null)
            {
                Debug.LogError($"{this.name}'s ItemManager tried to generate an Item while it was Null!");
                return;
            }
            //[TODO] Load the 3D model of the Item

        }

        private void OnDrawGizmos()
        {
            foreach (var cell in Item.ShapeOffsets) {
                var cellVector = new Vector3(itemSize, itemSize, itemSize);
                Gizmos.color = Color.blue;
                Vector3 floatCell = cell;
                Gizmos.DrawWireCube(gameObject.transform.position + (floatCell * itemSize), cellVector);
            }
        }
    }
}
