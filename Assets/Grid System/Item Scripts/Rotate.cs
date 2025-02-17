using GridSystem.Items;
using UnityEngine;

namespace GridSystem.Interactions
{
    public class Rotate : MonoBehaviour
    {
        private ItemManager itemMang;
        [SerializeField]
        private int outsideGridRotAmount = 5;

        void OnEnable() {
            itemMang = gameObject.GetComponent<ItemManager>();
        }

        //Unmodified Rotation - Left/Right | Modified Rotation - Up/Down
        public void RotateItem(int direction, bool modified) {
            Vector3 originalSnapVector = GetSnappedVector();
            //Wish there was a 2D switch statement
            if (direction == 1) {
                if (modified) {
                    //UP
                    if (itemMang.gridManager != null) {
                        itemMang.rotation *= Quaternion.Euler(-90, 0, 0);
                        if (!originalSnapVector.Equals(GetSnappedVector())) UpdateItemCellRotation(1);
                        return;
                    }
                    itemMang.rotation *= Quaternion.Euler(-outsideGridRotAmount, 0, 0);
                    if (!originalSnapVector.Equals(GetSnappedVector())) UpdateItemCellRotation(1);
                    return;
                }
                //LEFT
                if (itemMang.gridManager != null)
                {
                    itemMang.rotation *= Quaternion.Euler(0, 90, 0);
                    if (!originalSnapVector.Equals(GetSnappedVector())) UpdateItemCellRotation(3);
                    return;
                }
                itemMang.rotation *= Quaternion.Euler(0, outsideGridRotAmount, 0);
                if (!originalSnapVector.Equals(GetSnappedVector())) UpdateItemCellRotation(3);
                return;
            }
            if (modified) {
                //DOWN
                if (itemMang.gridManager != null)
                {
                    itemMang.rotation *= Quaternion.Euler(90, 0, 0);
                    if (!originalSnapVector.Equals(GetSnappedVector())) UpdateItemCellRotation(2);
                    return;
                }
                itemMang.rotation *= Quaternion.Euler(outsideGridRotAmount, 0, 0);
                if (!originalSnapVector.Equals(GetSnappedVector())) UpdateItemCellRotation(2);
                return;
            }
            //RIGHT
            if (itemMang.gridManager != null)
            {
                itemMang.rotation *= Quaternion.Euler(0, -90, 0);
                if (!originalSnapVector.Equals(GetSnappedVector())) UpdateItemCellRotation(4);
                return;
            }
            itemMang.rotation *= Quaternion.Euler(0, -outsideGridRotAmount, 0);
            if (!originalSnapVector.Equals(GetSnappedVector())) UpdateItemCellRotation(4);
        }

        public void SnapRotation() {
            Vector3 snappedRot = GetSnappedVector();
            itemMang.rotation.eulerAngles = snappedRot;
        }

        //Needs updating/fixing
        private Vector3 GetSnappedVector() {
            Vector3 snappedRot = new Vector3(0, 0, 0);
            snappedRot.x = Mathf.Round(itemMang.rotation.eulerAngles.x / 90) * 90;
            snappedRot.y = Mathf.Round(itemMang.rotation.eulerAngles.y / 90) * 90;
            snappedRot.z = Mathf.Round(itemMang.rotation.eulerAngles.z / 90) * 90;
            return snappedRot;
        }

        // 1 - UP | 2 - Down | 3 - Left | 4 - Right
        private void UpdateItemCellRotation(int direction) {
            switch (direction)
            {
                case 1:
                    for (int i = 0; i < itemMang.rotatedOffsets.Count; i++)
                    {
                        itemMang.rotatedOffsets[i] = new Vector3Int(itemMang.rotatedOffsets[i].x, itemMang.rotatedOffsets[i].z, -itemMang.rotatedOffsets[i].y);
                    }
                    break;
                case 2:
                    for (int i = 0; i < itemMang.rotatedOffsets.Count; i++)
                    {
                        itemMang.rotatedOffsets[i] = new Vector3Int(itemMang.rotatedOffsets[i].x, -itemMang.rotatedOffsets[i].z, itemMang.rotatedOffsets[i].y);
                    }
                    break;
                case 3:
                    for (int i = 0; i < itemMang.rotatedOffsets.Count; i++)
                    {
                        itemMang.rotatedOffsets[i] = new Vector3Int(itemMang.rotatedOffsets[i].z, itemMang.rotatedOffsets[i].y, -itemMang.rotatedOffsets[i].x);
                    }
                    break;
                case 4:
                    for (int i = 0; i < itemMang.rotatedOffsets.Count; i++)
                    {
                        itemMang.rotatedOffsets[i] = new Vector3Int(-itemMang.rotatedOffsets[i].z, itemMang.rotatedOffsets[i].y, itemMang.rotatedOffsets[i].x);
                    }
                    break;
                default:
                    Debug.LogError("Incorrect direction entered for UpdateItemCellRotation()");
                    break;
            }
        }
    }
}
