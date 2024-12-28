using GridSystem.Items;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GridSystem.Interactions
{
    public class Rotate : MonoBehaviour
    {
        private ItemManager itemMang;
        void Start()
        {
            itemMang = gameObject.GetComponent<ItemManager>();
        }

        //Unmodified Rotation - Left/Right | Modified Rotation - Up/Down
        public void RotateItem(int direction, bool modified) {
            Vector3 rot = itemMang.rotation;
            Debug.Log($"Original Rotation: {rot}");
            //Wish there was a 2D switch statement
            if (direction == 1) {
                if (modified) {
                    //UP
                    return;
                }
                //LEFT
                return;
            }
            if (modified) {
                //DOWN
                return;
            }
            //RIGHT
        }
    }
}
