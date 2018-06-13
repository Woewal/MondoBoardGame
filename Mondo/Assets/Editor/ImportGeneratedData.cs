using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Linq;
using System;
using Vuforia;

public class ImportGeneratedData : ScriptableWizard
{

    [SerializeField] TextAsset xmlFile;
    [SerializeField] TargetCreator targetCreator;
    [SerializeField] List<GameObject> tilePrefabs;

    XmlDocument xml;

    [MenuItem("Mondo/Import Mondo Data")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("Import", typeof(ImportGeneratedData));
    }

    private void OnWizardCreate()
    {
        if (targetCreator.tiles != null)
        {
            targetCreator.tiles.Clear();
        }
        else
        {
            targetCreator.tiles = new List<TileData>();
        }

        XmlNodeList tiles = ReadXmlFile();
        
        for(int tileIndex = 0; tileIndex < tilePrefabs.Count; tileIndex++)
        {
            TileData tileData = new TileData();
            
            XmlNodeList triangles = tiles[tileIndex].ChildNodes;

            tileData.triangles[0].side = TileData.Triangle.Side.Up;
            tileData.triangles[1].side = TileData.Triangle.Side.Right;
            tileData.triangles[2].side = TileData.Triangle.Side.Down;
            tileData.triangles[3].side = TileData.Triangle.Side.Left;

            for(int i = 0; i < 4; i++)
            {
                tileData.triangles[i].biome = (TileData.Triangle.Biome)Enum.Parse(typeof(TileData.Triangle.Biome), triangles[i].InnerText);
            }

            tileData.TilePrefab = tilePrefabs[tileIndex];

            targetCreator.tiles.Add(tileData);
        }

        InitiateGameObjects();
        Debug.Log("Succesfully exported data");
    }

    XmlNodeList ReadXmlFile()
    {
        xml = new XmlDocument();
        xml.LoadXml(xmlFile.text);
        XmlNodeList nodes = xml.GetElementsByTagName("tile");
        return nodes;
    }

    void InitiateGameObjects()
    {
        foreach (Transform child in targetCreator.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (var tile in targetCreator.tiles)
        {
            GameObject target = new GameObject(tile.ToString());
            target.transform.SetParent(targetCreator.transform);
            target.AddComponent<DefaultTrackableEventHandler>();
            target.AddComponent<TurnOffBehaviour>();
            target.AddComponent<ImageTargetBehaviour>();
            var targetComponent = target.AddComponent<Target>();
            targetComponent.TileData = tile;

            var prefab = Instantiate(tile.TilePrefab, target.transform);
            prefab.transform.position = new Vector3(-.5f, 0, -.5f);
        }
    }
}
