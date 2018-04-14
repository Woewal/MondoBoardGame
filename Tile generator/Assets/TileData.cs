using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile", menuName = "Test/Tile", order = 1)]
public class TileData : ScriptableObject {
    public List<Triangle> triangles;
    [SerializeField] bool includePassiveVulcano;
    [SerializeField] bool includeActiveVulcano;
    [SerializeField] bool includeAnimal;
	
    [System.Serializable]
    public class Triangle
    {
        public enum Side { Up, Down, Left, Right};
        public Side side;

        public enum Biome { Forest, Desert, Plains, Water}
        public Biome biome;
    }
}
