using GridSystem.Core;
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

        private bool itemGrabbed;
        private GameObject grabbedItem;
        private Rigidbody itemRB;

        private GridManager[] grids;
        private GridManager interactingGridManager = null;
        private Vector3Int snappedCell;
        private Vector3 invalidPoint = new Vector3(1000000, 1000000, 1000000);

        private void Start()
        {
            grids = FindObjectsByType<GridManager>(FindObjectsSortMode.None);
        }

        //Swap to 'OnAttack' for left click interaction, 'OnInteract' for 'E' key interaction.
        void OnInteract()
        {
            if (itemGrabbed)
            {
                DropItem();
                return;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            // Check if the ray hits something within interactDistance
            if (Physics.Raycast(ray, out hitInfo, interactDistance, hitLayer))
            {
                // Do stuff to whatever we hit here
                PickupItem(hitInfo.collider.gameObject);
            }
        }
        private void PickupItem(GameObject itemObject)
        {
            if (itemObject.TryGetComponent<Rigidbody>(out itemRB))
            {
                itemGrabbed = true;
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
                ItemScriptableObject item = grabbedItem.gameObject.GetComponent<ItemManager>().Item;
                if (interactingGridManager.AddItem(item, snappedCell)){
                    Debug.Log("Add Item worked");
                    grabbedItem = null;
                    itemGrabbed = false;
                } else {
                    Debug.Log("Add Item failed");
                }
                //!!!Still need to handle items bigger than 1 cell and rotation!!!
                return;
            }
            grabbedItem = null;
            itemGrabbed = false;
        }

        private Vector3 FindNearestSnapPoint(Vector3 position) {
            Vector3 closestPoint = invalidPoint;

            interactingGridManager = null;
            foreach (var grid in grids)
            {   //Swapping snapPoint to just be closestPoint makes it not update closestPoint correctly???
                Vector3 snapPoint = grid.GetNearestCell(position);
                if (snapPoint == invalidPoint) continue; // Ignore "invalid" return
                closestPoint = snapPoint;
                interactingGridManager = grid;
            }
            return closestPoint;
        }
        
        private void FixedUpdate()
        {
            if (grabbedItem != null)
            {
                Vector3 targetVelocity;
                Vector3 nearestSnapPoint = FindNearestSnapPoint(itemGrabPointTransform.position);
                if (nearestSnapPoint != invalidPoint) {
                    //Snap to grid positions
                    targetVelocity = (nearestSnapPoint - itemRB.position) / Time.fixedDeltaTime;
                    itemRB.linearVelocity = targetVelocity;
                    snappedCell = interactingGridManager.GetCell(nearestSnapPoint);
                    //Item Rotation Handling
                    itemRB.angularVelocity = Vector3.zero;
                    itemRB.rotation = Quaternion.identity; //Grids cannot be rotated due to how they work with Bounds, for now
                    return;
                }

                //Item Movement Handling
                targetVelocity = (itemGrabPointTransform.position - grabbedItem.transform.position) / Time.fixedDeltaTime;
                itemRB.linearVelocity = targetVelocity;

                //Item Rotation Handling
                itemRB.angularVelocity = Vector3.Lerp(itemRB.angularVelocity, Vector3.zero, itemLerpSpeed * Time.fixedDeltaTime);
            }
        }
    }
}
