using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Linq;

public class TilesToXML : MonoBehaviour {

	public void ProcessTiles(List<TileData> tiles)
    {
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
            xmlWriter.WriteEndElement();
        }
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndDocument();
        xmlWriter.Flush();

    }

    string GetFilePath(string name)
    {
        return string.Format("{0}/Export/{1}.xml", Application.dataPath, name);
    }
}
