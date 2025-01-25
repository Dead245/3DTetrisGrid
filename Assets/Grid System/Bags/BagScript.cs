using GridSystem.Interactions;
using UnityEngine;
using GridSystem.Core;

namespace GridSystem.Bag
{
    public class BagScript : MonoBehaviour, IInteractable
    {
        private bool isOpen = false;
        GridManager BagGrid;
        void OnEnable() {
            BagGrid = GetComponentInChildren<GridManager>();
            BagGrid.SetGridActive(isOpen);
        }
        public void Interact(GameObject originObject) {
            isOpen = !isOpen;
            BagGrid.SetGridActive(isOpen);
            if (isOpen) {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
            else {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
        }
    }
}
