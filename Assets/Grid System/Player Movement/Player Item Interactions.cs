using UnityEngine;
using GridSystem.PickupLogic;
using UnityEngine.InputSystem;

namespace GridSystem.Interactions
{
    //Has to be on the same object that has the player Input Map
    public class PlayerItemInteractions : MonoBehaviour
    {
        [SerializeField]
        private float interactDistance = 3f;
        private bool modifiedRot = false;
        void OnRotate(InputValue rotDir) {
            Pickup pickup = GetComponent<Pickup>();
            if (pickup.isItemGrabbed)
            {
                pickup.GrabbedItem.GetComponent<Rotate>().RotateItem((int)rotDir.Get<float>(), modifiedRot);
            }
        }
        void OnRotateModifierOn() {
            modifiedRot = true;
        }
        void OnRotateModifierOff() {
            modifiedRot = false;
        }

        void OnInteract() {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            // Check if the ray hits something within interactDistance
            if (Physics.Raycast(ray, out hitInfo, interactDistance))
            {
                // Do stuff to whatever we hit here
                IInteractable interactable = hitInfo.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact(this.gameObject);
                }
            }
        }
        void OnAttack()
        {
            Debug.Log("Attack (Left Clicked)");
        }
    }
}
