using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandVisualizer : MonoBehaviour {

    [SerializeField] GameObject placeholderIslandPrefab;

	public void Visualize(Tile[,] tiles)
    {
        for(int x = 0; x < tiles.GetLength(0); x++)
        {
            for(int z = 0; z < tiles.GetLength(1); z++)
            {
                if(tiles[x,z] != null)
                {
                    var placeholderIsland = Instantiate(placeholderIslandPrefab, transform);
                    placeholderIsland.transform.position = new Vector3(x, 0, z);
                }
            }
        }
    }
}
