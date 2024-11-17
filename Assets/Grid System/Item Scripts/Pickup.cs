using GridSystem.Core;
using UnityEngine;
using UnityEngine.UIElements;

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
            grabbedItem = null;
            itemGrabbed = false;
        }

        private Vector3 FindNearestSnapPoint(Vector3 position) {
            Vector3 closestPoint = invalidPoint;
            float closestDistance = float.MaxValue;

            foreach (var grid in grids)
            {
                interactingGridManager = null;
                Vector3 snapPoint = grid.GetNearestCell(itemGrabPointTransform.position);
                if (snapPoint == invalidPoint) continue; // Ignore "invalid" return

                float distance = Vector3.Distance(itemGrabPointTransform.position, snapPoint);
                if (distance < closestDistance)
                {
                    closestPoint = snapPoint;
                    closestDistance = distance;
                    interactingGridManager = grid;
                }
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
