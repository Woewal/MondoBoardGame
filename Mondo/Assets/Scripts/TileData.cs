using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData {
    public List<Triangle> triangles = new List<Triangle>() { new Triangle(), new Triangle(), new Triangle(), new Triangle() };
    //[SerializeField] bool includePassiveVulcano;
    //[SerializeField] bool includeActiveVulcano;
    //[SerializeField] bool includeAnimal;

    public GameObject TilePrefab;
	
    [System.Serializable]
    public class Triangle
    {
        public enum Side { Up, Down, Left, Right};
        public Side side;

        public enum Biome { Forest, Desert, Plains, Water, Null }
        public Biome biome;
    }
}
