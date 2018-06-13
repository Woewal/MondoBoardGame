using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Linq;
using System.IO;

public class TilesToXML : MonoBehaviour {

    [SerializeField]
    bool run = false;

	public void ProcessTiles(List<TileData> tiles)
    {
        if (!run)
            return;

        XmlWriter xmlWriter = XmlWriter.Create(GetFilePath("Tiles"));

        xmlWriter.WriteStartDocument();
        xmlWriter.WriteStartElement("tiles");

        foreach(var tile in tiles)
        {
            xmlWriter.WriteStartElement("tile");
            xmlWriter.WriteAttributeString("id", tile.name);
            xmlWriter.WriteElementString("up", tile.triangles.Where(x => x.side == TileData.Triangle.Side.Up).First().biome.ToString());
            xmlWriter.WriteElementString("right", tile.triangles.Where(x => x.side == TileData.Triangle.Side.Right).First().biome.ToString());
            xmlWriter.WriteElementString("down", tile.triangles.Where(x => x.side == TileData.Triangle.Side.Down).First().biome.ToString());
            xmlWriter.WriteElementString("left", tile.triangles.Where(x => x.side == TileData.Triangle.Side.Left).First().biome.ToString());

            if(tile.animal != null)
                xmlWriter.WriteElementString("animal", tile.animal.biomeType.ToString());

            xmlWriter.WriteEndElement();
        }
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndDocument();
        xmlWriter.Flush();

    }

    string GetFilePath(string name)
    {
        return string.Format("{0}/Export/TileData/{1}.xml", Application.dataPath, name);
    }
}
