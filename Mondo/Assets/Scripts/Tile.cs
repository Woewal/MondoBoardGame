using UnityEngine;
using System.Collections;
using System.Linq;

public class Tile
{
    public static float tileSize = 1;

    static int amountOfTiles;

    public int x;
    public int z;

    public int rotation;

    public int id;

    public TileData tileData;

    public Tile ()
    {
        id = amountOfTiles;
        amountOfTiles++;
    }

    public TileData.Triangle.Biome GetBiome(int direction)
    {
        if(rotation == 360)
        {
            rotation = 0;
        }

        if (rotation < 0)
        {
            rotation += 360;
        }

        int calculatedDirection = direction / 90 + rotation / 90;

        if (calculatedDirection < 0)
        {
            calculatedDirection += 4;
        }

        if (calculatedDirection >= 4)
        {
            calculatedDirection -= 4;
        }
        
        if(calculatedDirection == 0)
        {
            return tileData.triangles[2].biome;
        }
        else if(calculatedDirection == 1)
        {
            return tileData.triangles[1].biome;
        }
        else if (calculatedDirection == 2)
        {
            return tileData.triangles[0].biome;
        }
        else if (calculatedDirection == 3)
        {
            return tileData.triangles[3].biome;
        }
        else
        {
            return TileData.Triangle.Biome.Desert;
        }
    }

    public static Tile CreateTile(Target target)
    {
        var newTile = new Tile();
        var coordinate = target.GetCoordinate();

        newTile.x = coordinate.X; newTile.z = coordinate.Z;

        newTile.rotation = target.GetRotation();

        newTile.tileData = target.TileData;

        return newTile;
    }
}
