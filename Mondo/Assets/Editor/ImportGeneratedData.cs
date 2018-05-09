using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Linq;
using System;
using Vuforia;

public class ImportGeneratedData : ScriptableWizard {

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
        if(targetCreator.tiles != null)
        {
            targetCreator.tiles.Clear();
        }
        else
        {
            targetCreator.tiles = new List<TileData>();
        }

        XmlNodeList tiles = ReadXmlFile();
        foreach(XmlNode tile in tiles)
        {
            TileData tileData = new TileData();

            int triangleIndex = 0;
            foreach(XmlNode triangle in tile.ChildNodes)
            {
                if(triangle.Name == "up")
                {
                    tileData.triangles[triangleIndex].side = TileData.Triangle.Side.Up;
                    
                }
                else if(triangle.Name == "right")
                {
                    tileData.triangles[triangleIndex].side = TileData.Triangle.Side.Right;
                }
                else if (triangle.Name == "down")
                {
                    tileData.triangles[triangleIndex].side = TileData.Triangle.Side.Down;
                }
                else
                {
                    tileData.triangles[triangleIndex].side = TileData.Triangle.Side.Left;
                }

                tileData.triangles[triangleIndex].biome = (TileData.Triangle.Biome)Enum.Parse(typeof(TileData.Triangle.Biome), triangle.InnerText);

                triangleIndex++;
            }

            tileData.TilePrefab = tilePrefabs.Where(x => int.Parse(x.name) == int.Parse(tile.Attributes["id"].Value)).First();

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
