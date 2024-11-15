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
        private float itemLerpSpeed = 10f;


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
                itemRB.useGravity = false;
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
            itemRB.useGravity = true;
            grabbedItem = null;
            itemGrabbed = false;
        }

        //Still jitters at high speeds while picked up...
        private void FixedUpdate()
        {
            if (grabbedItem != null)
            {
                Vector3 itemMovement = Vector3.Lerp(grabbedItem.transform.position, itemGrabPointTransform.position, Time.fixedDeltaTime * itemLerpSpeed);
                itemRB.MovePosition(itemMovement);
            }
        }
    }
}
