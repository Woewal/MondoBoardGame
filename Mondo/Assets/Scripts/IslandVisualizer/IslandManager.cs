using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandManager : MonoBehaviour
{

    [SerializeField] GameObject placeholderIslandPrefab;
    public Tile[,] tiles;

    public void Visualize(Tile[,] tiles)
    {
        this.tiles = tiles;

        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int z = 0; z < tiles.GetLength(1); z++)
            {
                if (tiles[x, z] != null)
                {
                    var placeholderIsland = Instantiate(tiles[x, z].tileData.TilePrefab, transform);
                    placeholderIsland.transform.position = new Vector3(x, 0, z);
                    placeholderIsland.transform.Rotate(new Vector3(0, tiles[x,z].rotation, 0));
                    placeholderIsland.AddComponent<TileVisualizer>().tile = tiles[x,z];
                }
            }
        }
    }
}
