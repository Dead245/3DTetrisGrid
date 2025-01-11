using GridSystem.Interactions;
using UnityEngine;

namespace GridSystem.Bag
{
    public class BagScript : MonoBehaviour, IInteractable
    {
        private bool isOpen = false;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        public void Interact(GameObject originObject) {
            isOpen = !isOpen;
            Debug.Log($"Bag is open? {isOpen}");
        }
    }
}
