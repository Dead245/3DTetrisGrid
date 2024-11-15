using UnityEngine;

namespace GridSystem.Pickup
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

        private void FixedUpdate()
        {
            if (grabbedItem != null)
            {
                //Item Movement Handling
                Vector3 targetVelocity = (itemGrabPointTransform.position - grabbedItem.transform.position) / Time.fixedDeltaTime;
                itemRB.linearVelocity = targetVelocity;

                //Item Rotation Handling (slows it down)
                itemRB.angularVelocity = Vector3.Lerp(itemRB.angularVelocity, Vector3.zero, itemLerpSpeed * Time.fixedDeltaTime);
            }
        }
    }
}
