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
        [SerializeField]
        private float itemSize = 0.5f;

        public Quaternion rotation;
        public List<Vector3Int> rotatedOffsets = new List<Vector3Int>();

        public ItemScriptableObject Item => item;
        //[TODO] Serialized Field of the 3D model of the item

        private void OnEnable()
        {
            GenerateItem();
            rotatedOffsets = item.ShapeOffsets;
            rotation = transform.rotation;
        }

        private void GenerateItem()
        {
            if (item == null)
            {
                Debug.LogError($"{name}'s ItemManager tried to generate an Item while it was Null!");
                return;
            }
            //[TODO] Load the 3D model of the Item

        }

        public void Interact(GameObject originObject) {
            originObject.GetComponent<Pickup>().InteractItem(this.transform.gameObject);
        }

        private void Update()
        {
#if UNITY_EDITOR
            SceneView.RepaintAll();
#endif
        }
        private void OnDrawGizmos()
        {
            foreach (var cell in rotatedOffsets) {
                var cellVector = new Vector3(itemSize, itemSize, itemSize);
                Gizmos.color = Color.blue;
                Vector3 floatCell = cell;
                Gizmos.DrawWireCube(gameObject.transform.position + (floatCell * itemSize), cellVector);
            }
        }
    }
}
