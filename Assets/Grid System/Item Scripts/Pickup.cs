using GridSystem.Core;
using GridSystem.Interactions;
using GridSystem.Items;
using UnityEngine;

namespace GridSystem.PickupLogic
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField]
        Transform itemGrabPointTransform;
        [SerializeField]
        private float interactDistance = 3f;
        [SerializeField]
        private LayerMask hitLayer;
        [SerializeField]
        private float itemLerpSpeed = 1f;

        private bool isItemGrabbed;
        private GameObject grabbedItem;
        private Rigidbody itemRB;

        private GridManager[] grids;
        private GridManager interactingGridManager = null;
        private Vector3Int snappedCell;
        private Vector3 invalidPoint = new Vector3(1000000, 1000000, 1000000);
        public GameObject GrabbedItem => grabbedItem;

        private void Start()
        {
            grids = FindObjectsByType<GridManager>(FindObjectsSortMode.None);
        }

        //If true, it means it is interacting/holding an item (hopefully)
        public bool Interact()
        {
            if (isItemGrabbed)
            {
                DropItem();
                return false;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            // Check if the ray hits something within interactDistance
            if (Physics.Raycast(ray, out hitInfo, interactDistance, hitLayer))
            {
                // Do stuff to whatever we hit here
                PickupItem(hitInfo.collider.gameObject);
                return true;
            }
            return false;
        }
        private void PickupItem(GameObject itemObject)
        {
            if (itemObject.TryGetComponent<Rigidbody>(out itemRB))
            {
                if (itemRB.isKinematic) { //Means it is in a grid
                    Vector3Int itemCell = itemObject.GetComponent<ItemManager>().gridCellOrigin;
                    interactingGridManager = itemObject.GetComponent<ItemManager>().gridManager;
                    interactingGridManager.RemoveItem(itemCell);
                    itemRB.isKinematic = false;
                }
                isItemGrabbed = true;
                grabbedItem = itemObject;
                return;
            }
            else
            {
                Debug.LogWarning($"{itemObject.name} doesn't have a Rigidbody. Pickup Failed.");
            }
        }
        private void DropItem()
        {
            if (interactingGridManager != null) {
                //Add to {interactingGridManager}'s grid
                if (interactingGridManager.AddItem(grabbedItem, snappedCell)) {
                    Debug.Log("Add Item worked");
                    grabbedItem = null;
                    isItemGrabbed = false;
                }
                return;
            }
            grabbedItem = null;
            isItemGrabbed = false;
        }

        private Vector3 FindNearestSnapPoint(Vector3 position) {
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
