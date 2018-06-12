using UnityEngine;
using System.Collections;

public class TileVisualizer : MonoBehaviour
{
    public Tile tile;


    private void Start()
    {
        //name = tile.id.ToString();    
    }

    void OnDrawGizmosSelected()
    {
        if (tile == null)
            return;

        for (int dir = 0; dir < 360; dir += 90)
        {
            Vector3 direction = Vector3.zero;

            if (dir == 0)
            {
                direction = new Vector3(0, 0, -1);
            }
            else if (dir == 90)
            {
                direction = new Vector3(1, 0, 0);
            }
            else if (dir == 180)
            {
                direction = new Vector3(0, 0, 1);
            }
            else if (dir == 270)
            {
                direction = new Vector3(-1, 0, 0);
            }

            Color color = Color.white;

            var biome = tile.GetBiome(dir);

            if (biome == TileData.Triangle.Biome.Desert)
            {
                color = Color.yellow;
            }
            else if (biome == TileData.Triangle.Biome.Forest)
            {
                color = Color.green;
            }
            else if (biome == TileData.Triangle.Biome.Plains)
            {
                color = Color.cyan;
            }
            else if (biome == TileData.Triangle.Biome.Water)
            {
                color = Color.blue;
            }

            Gizmos.color = color;
            Gizmos.DrawLine(transform.position, transform.position + direction);
        }
    }
}
