using GridSystem.Core;
using GridSystem.Interactions;
using GridSystem.Items;
using UnityEngine;

namespace GridSystem.PickupLogic
{
    public class Pickup : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        public Transform itemGrabPointTransform;
        [SerializeField]
        private float itemLerpSpeed = 1f;

        public bool isItemGrabbed;
        public GameObject grabbedItem;
        public Rigidbody itemRB;

        public GridManager interactingGridManager = null;
        private Vector3Int snappedCell;
        public GameObject GrabbedItem => grabbedItem;
        #endregion
        
        private void FixedUpdate()
        {
            //For moving an item around once grabbed
            if (grabbedItem != null)
            {
                //Item Rotation Handling
                    grabbedItem.transform.rotation = Quaternion.Lerp(
                            grabbedItem.transform.rotation,
                            grabbedItem.GetComponent<ItemManager>().rotation,
                            itemLerpSpeed * Time.deltaTime );

                //Check if item is in a grid
                Vector3 targetVelocity;
                Vector3? nearestSnapPoint;
                if (interactingGridManager == null) nearestSnapPoint = null;
                else nearestSnapPoint = interactingGridManager.closestSnapPoint;

                if (nearestSnapPoint != null)
                {
                    //Snap to grid positions
                    targetVelocity = ((Vector3)nearestSnapPoint - itemRB.position) / Time.fixedDeltaTime;
                    itemRB.linearVelocity = targetVelocity;
                    snappedCell = interactingGridManager.GetCell((Vector3)nearestSnapPoint);
                    //Snap Rotation
                    grabbedItem.GetComponent<Rotate>().SnapRotation();
                    return;
                }

                //Item Movement Handling
                targetVelocity = (itemGrabPointTransform.position - grabbedItem.transform.position) / Time.fixedDeltaTime;
                itemRB.linearVelocity = targetVelocity;
            }
        }
    }
}
