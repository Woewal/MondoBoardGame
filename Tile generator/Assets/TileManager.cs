using System;
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
        //ExportTileData();
    }

    private void Update()
    {
        if(Input.GetButton("Fire1"))
        {
            ExportTiles();
        }
    }

    private void ExportTiles()
    {
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

    private void ExportTileData()
    {
        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Export/TileData/");
        FileUtil.CopyFileOrDirectory(Application.dataPath + "/Tiles", Application.dataPath + "/Export/TileData/");
    }
}
