using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TargetProcessor : MonoBehaviour {
    static float tileSize = 1;

    Tile[,] tiles;

	public void Process(List<Target> targets)
    {
        int lowestX = 0, highestX = 0, lowestZ = 0, highestZ = 0;

        foreach (var target in targets)
        {
            SetMinAndMax(ref lowestX, ref highestX, ref lowestZ, ref highestZ, target);
        }

        tiles = new Tile[Mathf.Abs(lowestX - highestX) + 1, Mathf.Abs(lowestZ - highestZ) + 1];

        foreach(var target in targets)
        {
            var coordinate = GetCoordinate(target.transform.position);
            tiles[Mathf.Abs(coordinate.X - lowestX), Mathf.Abs(coordinate.Z - lowestZ)] = CreateTile(target); 
        }

        for(int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int z = 0; z < tiles.GetLength(1); z++)
            {
                tiles[x, z].X += Mathf.Abs(lowestX);
                tiles[x, z].Z += Mathf.Abs(lowestZ);
            }
        }

        SceneManager.LoadScene("ScoreScreen");
        SceneManager.sceneLoaded += VisualizeTiles;
    }

    void VisualizeTiles(Scene scene, LoadSceneMode mode)
    {
        GameObject.FindObjectOfType<IslandManager>().Visualize(tiles);
        SceneManager.sceneLoaded -= VisualizeTiles;
    }

    private void SetMinAndMax(ref int lowestX, ref int highestX, ref int lowestZ, ref int highestZ, Target target)
    {
        var coordinate = GetCoordinate(target.transform.position);
        if (coordinate.X < lowestX)
            lowestX = coordinate.X;
        else if (coordinate.X > highestX)
            highestX = coordinate.X;
        if (coordinate.Z < lowestZ)
            lowestZ = coordinate.Z;
        else if (coordinate.Z > highestZ)
            highestZ = coordinate.Z;
    }

    private Coordinate GetCoordinate(Vector3 position)
    {
        int x = (int)(Mathf.Round(position.x / tileSize) * tileSize);
        int z = (int)(Mathf.Round(position.z / tileSize) * tileSize);

        return new Coordinate(x, z);
    }

    private int GetRotation(float rotation)
    {
        return (int)Mathf.Round(rotation / 90) * 90;
    }

    private Tile CreateTile(Target target)
    {
        var newTile = new Tile();
        var coordinate = GetCoordinate(target.transform.position);

        newTile.X = coordinate.X; newTile.Z = coordinate.Z;

        newTile.rotation = GetRotation(target.transform.rotation.eulerAngles.y);

        newTile.tileData = target.TileData;

        return newTile;
    }
}
