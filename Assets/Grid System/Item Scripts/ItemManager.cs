using UnityEngine;
using GridSystem.Core;

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

        public ItemScriptableObject Item => item;
        //[TODO] Serialized Field of the 3D model of the item

        private void OnEnable()
        {
            generateItem();
        }

        private void generateItem()
        {
            if (item == null)
            {
                Debug.LogError($"{this.name}'s ItemManager tried to generate an Item while it was Null!");
                return;
            }
            //[TODO] Load the 3D model of the Item

        }
    }
}
