﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TileManager : MonoBehaviour {

    public List<TileData> tiles;
    TileGenerator tileGenerator;
    TilesToXML xmlProcessor;
    List<GameObject> tileGameObjects = new List<GameObject>();

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
        ExportTilesToPrefab();
    }

    private void GenerateTile(TileData tile, int index)
    {
        var tileObject = tileGenerator.GenerateTile(tile);
        tileObject.transform.Translate(new Vector3(TileGenerator.TileSize * index + 1 * index, 0, 0));
        tileGameObjects.Add(tileObject);
    }

    private void ExportTilesToPrefab()
    {
        foreach(var tile in tileGameObjects)
        {
            string mlam = string.Format("Assets/Export/Prefabs/{0}.prefab", tile.name);
            PrefabUtility.CreatePrefab(mlam, tile);
        }
    }
}
