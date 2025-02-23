using UnityEngine;
using GridSystem.Core;
using System.Collections.Generic;
using UnityEditor;
using GridSystem.Interactions;
using GridSystem.PickupLogic;

namespace GridSystem.Items
{
    [SelectionBase]
    public class ItemManager : MonoBehaviour, IInteractable
    {
        public GridManager gridManager; //For when it supposed to be in a grid.
        public Vector3Int gridCellOrigin;
        //Eventually remove the need to set the item beforehand.
        [SerializeField]
        private ItemScriptableObject item;
        private float itemSize;

        public Quaternion rotation;
        public List<Vector3Int> rotatedOffsets;

        public ItemScriptableObject Item => item;
        //[TODO] Serialized Field of the 3D model of the item

        private void OnEnable()
        {
            GenerateItem();
            rotatedOffsets = new List<Vector3Int>(item.ShapeOffsets);
            rotation = transform.rotation;
            itemSize = new MasterGridScript().CellSize;
        }
        private void GenerateItem() {
            if (item == null) {
                Debug.LogError($"{name}'s ItemManager tried to generate an Item while it was Null!");
                return;
            }
            //[TODO] Load the 3D model of the Item

        }

        public void Interact(GameObject originObject) {
            Pickup pickup = originObject.GetComponent<Pickup>();
            //Drop Item
            #region Drop Item
            if (pickup.isItemGrabbed) {
                rotation = transform.rotation;
                if (pickup.interactingGridManager != null) {
                    //Add to interactingGridManager's grid since it means it is in a grid
                    Vector3Int snappedCell = gridManager.GetCell((Vector3)gridManager.GetNearestEmptyCell(this,pickup.itemGrabPointTransform.position));
                    if (pickup.interactingGridManager.AddItem(pickup.grabbedItem, snappedCell)) {
                        pickup.grabbedItem = null;
                        pickup.isItemGrabbed = false;
                    }
                    return;
                }
                pickup.grabbedItem = null;
                pickup.isItemGrabbed = false;
                return;
            }
            #endregion
            //Pickup Item
            #region Pickup Item
            Rigidbody itemRB;
            if (this.TryGetComponent<Rigidbody>(out itemRB)) {
                pickup.itemRB = itemRB;
                rotation = transform.rotation;
                if (itemRB.isKinematic) {
                    //Means it is in a grid
                    Vector3Int itemCell = gridCellOrigin;
                    pickup.interactingGridManager = gridManager;
                    gridManager.RemoveItem(itemCell);
                    itemRB.isKinematic = false;
                }
                pickup.isItemGrabbed = true;
                pickup.grabbedItem = gameObject;
                return;
            }
            #endregion
        }

        //Below is for displaying the item shapes via wire cubes (TEMP).
        #region Wireframe Display
        private void Update() {
#if UNITY_EDITOR
            SceneView.RepaintAll();
#endif
        }
        private void OnDrawGizmos() {
            foreach (var cell in rotatedOffsets) {
                var cellVector = new Vector3(itemSize, itemSize, itemSize);
                Gizmos.color = Color.blue;
                Vector3 floatCell = cell;
                Gizmos.DrawWireCube(gameObject.transform.position + (floatCell * itemSize), cellVector);
            }
        }
        #endregion
    }
}
