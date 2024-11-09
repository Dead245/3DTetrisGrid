using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

[SelectionBase]
public class ItemManager : MonoBehaviour
{
    //Eventually remove the need to set the item beforehand.
    [SerializeField]
    private ItemScriptableObject item;

    //[TODO] Serialized Field of the 3D model of the item

    private void OnEnable()
    {
        generateItem();
    }

    private void generateItem() {
        if (item == null) {
            Debug.LogError($"{this.name}'s ItemManager tried to generate an Item while it was Null!");
            return;
        }

        //[TODO] Load the 3D model of the Item instead of this
        foreach (var loc in item.ShapeOffsets) {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.SetParent(this.transform);
            cube.transform.position = this.transform.position + loc;
        }
    }
}
