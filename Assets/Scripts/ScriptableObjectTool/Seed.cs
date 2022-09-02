using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Seed", order = 51)]
public class Seed : ScriptableObject
{
    public bool Explored;
    [Space]

    public bool Edible;
    [Space]

    public bool Poison;
    [Space]

    public float ConvertTime;
    [Space]

    [Header("Seed Parameters")]
    [Space]
    public SeedsName Name;

    public SeedBedType SeedBedType;

    [Space]
    public Seed—lip SeedPrefab;

    [Space]
    public Sprite SeedSprite;

    [Space]
    [TextArea] public string SeedDescription;

    [Space]
    [Header("Plant Parameters")]
    [Space]

    public Plant PlantPrefab;

    [Space]
    public Sprite PlantSprite;

    [Space]
    public float Energy;

    [Space]
    [Header("Growth Time in seconds")]
    public float GrowthTime;

    [Space]
    [TextArea] public string PlantDescription;
}
