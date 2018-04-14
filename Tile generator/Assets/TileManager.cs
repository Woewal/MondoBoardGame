using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    public List<TileData> tiles;
    TileGenerator tileGenerator;
    TilesToXML xmlProcessor;

    public void Start()
    {
        tileGenerator = GetComponent<TileGenerator>();
        xmlProcessor = GetComponent<TilesToXML>();
        Generate();
    }

    private void Generate()
    {
        for(int i = 0; i < tiles.Count; i++)
        {
            GenerateTile(tiles[i], i);
        }

        StartCoroutine(TileToPNG.Instance.GeneratePNGs(tiles.Count));
        xmlProcessor.ProcessTiles(tiles);
    }

    private void GenerateTile(TileData tile, int index)
    {
        var tileObject = tileGenerator.GenerateTile(tile);
        tileObject.transform.Translate(new Vector3(TileGenerator.TileSize * index + 1 * index, 0, 0));
    }
}
