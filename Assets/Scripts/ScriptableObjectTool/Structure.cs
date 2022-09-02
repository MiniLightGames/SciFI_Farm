using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Structure", order = 51)]
public class Structure : ScriptableObject
{
    public bool Explored;
    [Space]
    public string Name;
    public int Price;
    public Sprite StructureSprite;
    public GameObject StructurePrefab;
    [TextArea] public string StructureDescription;
}
