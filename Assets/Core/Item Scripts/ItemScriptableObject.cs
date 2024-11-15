using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemScriptableObject", menuName = "Scriptable Objects/New Item")]
public class ItemScriptableObject : ScriptableObject
{
    [SerializeField]
    private List<Vector3Int> shapeOffsets = new List<Vector3Int>();

    public List<Vector3Int> ShapeOffsets => shapeOffsets;

}
