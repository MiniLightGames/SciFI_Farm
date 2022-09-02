using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gun", order = 51)]
public class Gun : ScriptableObject
{
    public bool Explored;
    [Space]

    public float FireRate;
    [Space]

    public Sprite InventorySprite;

    [Space]
    [TextArea] public string GunDescription;
}
