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

        private GridManager[] grids;
        public GridManager interactingGridManager = null;
        private Vector3Int snappedCell;
        private Vector3 invalidPoint = new Vector3(1000000, 1000000, 1000000);
        public GameObject GrabbedItem => grabbedItem;
        #endregion

        private void Start()
        {
            grids = FindObjectsByType<GridManager>(FindObjectsSortMode.None);
        }

        //When trying to place an item in a grid
        public Vector3 FindNearestSnapPoint(Vector3 position) {
            Vector3 closestPoint = invalidPoint;

            interactingGridManager = null;
            grabbedItem.GetComponent<ItemManager>().gridManager = null;
            foreach (var grid in grids)
            {   //Swapping snapPoint to just be closestPoint makes it not update closestPoint correctly???
                Vector3 snapPoint = grid.GetNearestEmptyCell(grabbedItem.GetComponent<ItemManager>(), position);
                if (snapPoint == invalidPoint) continue; // Ignore "invalid" return
                closestPoint = snapPoint;
                interactingGridManager = grid;
                grabbedItem.GetComponent<ItemManager>().gridManager = grid;
                return closestPoint;
            }
            return invalidPoint;
        }
        
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
                Vector3 nearestSnapPoint = FindNearestSnapPoint(itemGrabPointTransform.position);
                if (nearestSnapPoint != invalidPoint)
                {
                    //Snap to grid positions
                    targetVelocity = (nearestSnapPoint - itemRB.position) / Time.fixedDeltaTime;
                    itemRB.linearVelocity = targetVelocity;
                    snappedCell = interactingGridManager.GetCell(nearestSnapPoint);
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
