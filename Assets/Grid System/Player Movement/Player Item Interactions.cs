using UnityEngine;
using GridSystem.PickupLogic;
using UnityEngine.InputSystem;

namespace GridSystem.Interactions
{
    //Has to be on the same object that has the player Input Map
    public class PlayerItemInteractions : MonoBehaviour
    {
        private bool itemGrabbed = false;
        private bool modifiedRot = false;
        void OnRotate(InputValue rotDir) {
            if (!itemGrabbed) return;
            Pickup pickup = GetComponent<Pickup>();
            pickup.GrabbedItem.GetComponent<Rotate>()?.RotateItem((int)rotDir.Get<float>(), modifiedRot);
        }
        void OnRotateModifierOn() {
            modifiedRot = true;
        }
        void OnRotateModifierOff() {
            modifiedRot = false;
        }

        void OnInteract() {
            itemGrabbed = (bool)GetComponent<Pickup>()?.Interact();
        }
        void OnAttack()
        {
            Debug.Log("Attack (Left Clicked)");
        }
    }
}
