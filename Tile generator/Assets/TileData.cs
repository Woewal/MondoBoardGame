using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile", menuName = "Data/Tile", order = 1)]
public class TileData : ScriptableObject {
    public List<Triangle> triangles;
    public bool includePassiveVulcano;
    public bool includeActiveVulcano;

    public AnimalData animal;
    public Triangle.Side animalSide;
	
    [System.Serializable]
    public class Triangle
    {
        public enum Side { Up, Down, Left, Right};
        public Side side;

        public enum Biome { Forest, Desert, Plains, Water}
        public Biome biome;
    }
}
