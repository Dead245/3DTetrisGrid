using GridSystem.Items;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GridSystem.Interactions
{
    public class Rotate : MonoBehaviour
    {
        private ItemManager itemMang;

        private int outsideGridRotAmount = 5;
        void Start()
        {
            itemMang = gameObject.GetComponent<ItemManager>();
        }

        //Unmodified Rotation - Left/Right | Modified Rotation - Up/Down
        public void RotateItem(int direction, bool modified) {
            //Wish there was a 2D switch statement
            if (direction == 1) {
                if (modified) {
                    //UP
                    if (itemMang.gridManager != null) {
                        itemMang.rotation.eulerAngles -= new Vector3(90, 0, 0);
                        return;
                    }
                    itemMang.rotation.eulerAngles -= new Vector3(outsideGridRotAmount, 0, 0);
                    return;
                }
                //LEFT
                if (itemMang.gridManager != null)
                {
                    itemMang.rotation.eulerAngles += new Vector3(0, 90, 0);
                    return;
                }
                itemMang.rotation.eulerAngles += new Vector3(0, outsideGridRotAmount, 0);
                return;
            }
            if (modified) {
                //DOWN
                if (itemMang.gridManager != null)
                {
                    itemMang.rotation.eulerAngles += new Vector3(90, 0, 0);
                    return;
                }
                itemMang.rotation.eulerAngles += new Vector3(outsideGridRotAmount, 0, 0);
                return;
            }
            //RIGHT
            if (itemMang.gridManager != null)
            {
                itemMang.rotation.eulerAngles -= new Vector3(0, 90, 0);
                return;
            }
            itemMang.rotation.eulerAngles -= new Vector3(0, outsideGridRotAmount, 0);
        }

        public void snapRotation() {
            Vector3 snappedRot = new Vector3 (0, 0, 0);
            snappedRot.x = Mathf.Round(itemMang.rotation.eulerAngles.x / 90) * 90;
            snappedRot.y = Mathf.Round(itemMang.rotation.eulerAngles.y / 90) * 90;
            snappedRot.z = Mathf.Round(itemMang.rotation.eulerAngles.z / 90) * 90;
            itemMang.rotation.eulerAngles = snappedRot;
        }
    }
}
