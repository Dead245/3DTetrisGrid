using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GridSystem.Items
{
    [CreateAssetMenu(fileName = "ItemScriptableObject", menuName = "Scriptable Objects/New Item")]
    public class ItemScriptableObject : ScriptableObject
    {
        [SerializeField]
        private List<Vector3Int> shapeOffsets = new List<Vector3Int>();
        [SerializeField]
        private GameObject itemPrefabModel;

        public List<Vector3Int> ShapeOffsets => shapeOffsets;
        public GameObject ItemPrefabModel => itemPrefabModel;

    }
}
