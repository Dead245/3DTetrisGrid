using GridSystem.Items;
using UnityEngine;

namespace GridSystem.Interactions
{
    public class Rotate : MonoBehaviour
    {
        private ItemManager itemMang;
        [SerializeField]
        private int outsideGridRotAmount = 5;
        Vector3 baseOffsetRotation = Vector3.zero;
        void OnEnable() {
            itemMang = gameObject.GetComponent<ItemManager>();
            EnterGridOffsetUpdate();
        }

        //Unmodified Rotation - Left/Right | Modified Rotation - Up/Down
        public void RotateItem(int direction, bool modified) {
            if (itemMang.gridManager != null) {
                // Up/Down In Grid
                if (modified) {
                    itemMang.rotation = Quaternion.AngleAxis(direction * 90f, Camera.main.transform.right) * itemMang.rotation;
                    UpdateOffsets(1, direction);
                    SnapRotation();
                    return;
                }
                // Left/Right In Grid
                itemMang.rotation = Quaternion.AngleAxis(direction * 90f,Camera.main.transform.up) * itemMang.rotation;
                UpdateOffsets(2, direction);
                return;
            }
            // Up/Down Out of Grid
            if (modified) {
                itemMang.rotation = Quaternion.AngleAxis(direction * outsideGridRotAmount, Camera.main.transform.right) * itemMang.rotation;
                return;
            }
            // Left/Right Out of Grid
            itemMang.rotation = Quaternion.AngleAxis(direction * outsideGridRotAmount,Camera.main.transform.up) * itemMang.rotation;
        }
        public void SnapRotation() {
            if (itemMang.gridManager == null) { 
                Debug.LogError("Rotate/SnapRotation: itemMang.gridManger is null");
                return;
            }
            Quaternion gridRotation = itemMang.gridManager.transform.rotation;
            Quaternion relativeRot = Quaternion.Inverse(gridRotation) * itemMang.rotation;
            itemMang.rotation = gridRotation * GetSnappedQuat(relativeRot);
        }

        //Updating offsets due to item rotating outside of grid.
        public void EnterGridOffsetUpdate() {
            for (int i = 0; i < itemMang.rotatedOffsets.Count; i++) {
                Vector3 cell = itemMang.Item.ShapeOffsets[i];
                itemMang.rotatedOffsets[i] = Vector3Int.RoundToInt(itemMang.rotation * cell);
            }
        }

        private Quaternion GetSnappedQuat(Quaternion originalQuat) {
            Vector3 snappedRot = originalQuat.eulerAngles;
            snappedRot.x = Mathf.Round(snappedRot.x / 90) * 90;
            snappedRot.y = Mathf.Round(snappedRot.y / 90) * 90;
            snappedRot.z = Mathf.Round(snappedRot.z / 90) * 90;
            return Quaternion.Euler(snappedRot);
        }
        private void UpdateOffsets(int rotationDir, int amount) {
            switch (rotationDir) {
                case 1:// Up/Down
                    for (int a = 0; a < Mathf.Abs(amount); a++) {
                        for (int b = 0; b < itemMang.rotatedOffsets.Count; b++) {
                            //Need to do extra for Up/Down due to player orientation
                            float camYRot = Mathf.Round(Camera.main.transform.rotation.eulerAngles.y / 90);
                            Vector3Int rotation = itemMang.rotatedOffsets[b];
                            Debug.Log(camYRot);
                            switch (camYRot)
                            {
                                case 1: //Z Axis - ~90 degrees
                                    if (amount < 0) itemMang.rotatedOffsets[b] = new Vector3Int(-rotation.y, rotation.x, rotation.z);
                                    if (amount > 0) itemMang.rotatedOffsets[b] = new Vector3Int(rotation.y, -rotation.x, rotation.z);
                                    break;
                                case 2: //X Axis - ~180 degrees
                                    if (amount < 0) itemMang.rotatedOffsets[b] = new Vector3Int(rotation.x, -rotation.z, rotation.y);
                                    if (amount > 0) itemMang.rotatedOffsets[b] = new Vector3Int(rotation.x, rotation.z, -rotation.y);
                                    break;
                                case 3: //Z Axis - ~270 degrees
                                    if (amount < 0) itemMang.rotatedOffsets[b] = new Vector3Int(rotation.y, -rotation.x, rotation.z);
                                    if (amount > 0) itemMang.rotatedOffsets[b] = new Vector3Int(-rotation.y, rotation.x, rotation.z);
                                    break;
                                case 0: //X Axis - ~0 and 360 degrees
                                case 4:
                                    if (amount < 0) itemMang.rotatedOffsets[b] = new Vector3Int(rotation.x, rotation.z, -rotation.y);
                                    if (amount > 0) itemMang.rotatedOffsets[b] = new Vector3Int(rotation.x, -rotation.z, rotation.y);
                                    break;
                                default:
                                    Debug.LogError("Rotate/Update Offsets: Unsupported camYRot Direction"); //Should never happen
                                    break;
                            }
                        }
                    }
                    break;
                case 2:// Left/Right - Y Axis
                    for (int a = 0; a < Mathf.Abs(amount); a++) {
                        for (int b = 0; b < itemMang.rotatedOffsets.Count; b++) {
                            Vector3Int rotation = itemMang.rotatedOffsets[b];
                            if (amount < 0) itemMang.rotatedOffsets[b] = new Vector3Int(-rotation.z, rotation.y, rotation.x);
                            if (amount > 0) itemMang.rotatedOffsets[b] = new Vector3Int(rotation.z, rotation.y, -rotation.x);
                        }
                    }
                    break;
                default:
                    Debug.LogError("Rotate/Update Offsets: Invalid rotationDir in updateOffsets()");
                    break;
            }
        }
    }
}
