using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Animal", menuName = "Data/Animal", order = 1)]
public class AnimalData : ScriptableObject
{
    public GameObject model;

    public enum Biome { Sea, Land };

    public Biome biomeType;
}
