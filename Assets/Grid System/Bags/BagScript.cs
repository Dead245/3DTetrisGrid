using GridSystem.Interactions;
using UnityEngine;
using GridSystem.Core;

namespace GridSystem.Bag
{
    public class BagScript : MonoBehaviour, IInteractable
    {
        private bool isOpen = false;
        GridManager BagGrid;
        Rigidbody rb;
        void OnEnable() {
            BagGrid = GetComponentInChildren<GridManager>();
            BagGrid.SetGridActive(isOpen);
            rb = GetComponent<Rigidbody>();
        }
        public void Interact(GameObject originObject) {
            isOpen = !isOpen;
            BagGrid.SetGridActive(isOpen);
            if (isOpen) {
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }
            else {
                rb.constraints = RigidbodyConstraints.None;
            }
        }
    }
}
