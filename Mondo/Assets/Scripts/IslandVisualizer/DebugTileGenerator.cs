using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTileGenerator : MonoBehaviour {

    IslandManager islandManager;

    private void Awake()
    {
        islandManager = GetComponent<IslandManager>();
        Debug.Log("mjyaam");
    }

    public void GenerateDebugTiles()
    {
        Target[] targets = FindObjectsOfType<Target>();

        Process(targets);
    }

    public void Process(Target[] targets)
    {
        int lowestX = 0, highestX = 0, lowestZ = 0, highestZ = 0;

        foreach (var target in targets)
        {
            SetMinAndMax(ref lowestX, ref highestX, ref lowestZ, ref highestZ, target);
        }

        islandManager.tiles = new Tile[Mathf.Abs(lowestX - highestX) + 1, Mathf.Abs(lowestZ - highestZ) + 1];

        foreach (var target in targets)
        {
            var coordinate = target.GetCoordinate();

            var tile = Tile.CreateTile(target);

            target.GetComponent<TileVisualizer>().tile = tile;

            islandManager.tiles[Mathf.Abs(coordinate.X - lowestX), Mathf.Abs(coordinate.Z - lowestZ)] = tile;
        }

        for (int x = 0; x < islandManager.tiles.GetLength(0); x++)
        {
            for (int z = 0; z < islandManager.tiles.GetLength(1); z++)
            {
                islandManager.tiles[x, z].x += Mathf.Abs(lowestX);
                islandManager.tiles[x, z].z += Mathf.Abs(lowestZ);
            }
        }
    }

    private void SetMinAndMax(ref int lowestX, ref int highestX, ref int lowestZ, ref int highestZ, Target target)
    {
        var coordinate = target.GetCoordinate();
        if (coordinate.X < lowestX)
            lowestX = coordinate.X;
        else if (coordinate.X > highestX)
            highestX = coordinate.X;
        if (coordinate.Z < lowestZ)
            lowestZ = coordinate.Z;
        else if (coordinate.Z > highestZ)
            highestZ = coordinate.Z;
    }
}
