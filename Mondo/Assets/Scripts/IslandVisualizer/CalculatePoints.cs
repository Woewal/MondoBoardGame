using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CalculatePoints : MonoBehaviour
{
    private IslandManager islandManager;
    private Tile[,] tiles;

    private Node[,] verticalNodes;
    private Node[,] horizontalNodes;

    private List<Node> openNodes = new List<Node>();
    private List<Node> closedNodes = new List<Node>();

    private List<Node> mistakeNodes = new List<Node>();

    private List<Island> islands = new List<Island>();

    int score = 0;
    [SerializeField] Text scoreText;
    int mistakeAmount = 0;


    private void Start()
    {
        islandManager = GetComponent<IslandManager>();
        tiles = islandManager.tiles;

        CreateNodes();
        Calculate();
    }

    private void Calculate()
    {
        SetAllNodeConnections();
        GetIslands();
        scoreText.text = string.Format("Connecting tiles: {0}, Mistakes: {1}, Complete islands:{2}", score, mistakeAmount, islands.Count);

        foreach(var island in islands)
        {
            Debug.Log(island.biome);
        }
    }

    void GetIslands()
    {
        while (openNodes.Count > 0)
        {
            var node = openNodes[0];

            GetIsland(node);
            
            openNodes.Remove(node);
            closedNodes.Add(node);
        }
    }

    void GetIsland(Node node)
    {
        List<Node> openNeighbouringNodes = new List<Node>();
        List<Node> closedNeighbouringNodes = new List<Node>();

        openNeighbouringNodes.Add(node);

        while (openNeighbouringNodes.Count != 0)
        {
            var nodes = FindNeighbouringNodes(openNeighbouringNodes[0]);

            if (nodes == null)
            {
                return;
            }

            foreach(var neighbour in nodes)
            {
                if(!closedNeighbouringNodes.Contains(neighbour))
                {
                    openNeighbouringNodes.Add(neighbour);
                }
            }

            closedNeighbouringNodes.Add(openNeighbouringNodes[0]);
            openNeighbouringNodes.Remove(openNeighbouringNodes[0]);
        }

        var island = new Island();
        island.biome = node.biome;

        islands.Add(island);

    }

    private void SetAllNodeConnections()
    {
        foreach (var verticalNode in verticalNodes)
        {
            verticalNode.biome = CheckNodeConnection(verticalNode);
            if (verticalNode.biome == TileData.Triangle.Biome.Null)
            {
                openNodes.Remove(verticalNode);
                closedNodes.Add(verticalNode);
            }
        }
        foreach (var horizontalNode in horizontalNodes)
        {
            horizontalNode.biome = CheckNodeConnection(horizontalNode);
            if (horizontalNode.biome == TileData.Triangle.Biome.Null)
            {
                openNodes.Remove(horizontalNode);
                closedNodes.Add(horizontalNode);
            }
        }
    }

    private TileData.Triangle.Biome CheckNodeConnection(Node node)
    {
        if (node.Orientation == Node.NodeOrientation.Horizontal)
        {
            if (node.NeighbouringTiles[0].GetBiome(90) == node.NeighbouringTiles[1].GetBiome(270))
            {
                return node.NeighbouringTiles[0].GetBiome(90);
            }
            else
            {
                return TileData.Triangle.Biome.Null;
            }
        }
        if (node.Orientation == Node.NodeOrientation.Vertical)
        {
            if (node.NeighbouringTiles[0].GetBiome(180) == node.NeighbouringTiles[1].GetBiome(0))
            {
                return node.NeighbouringTiles[0].GetBiome(180);
            }
            else
            {
                return TileData.Triangle.Biome.Null;
            }
        }
        return TileData.Triangle.Biome.Null;
    }

    private List<Node> FindNeighbouringNodes(Node node)
    {
        List<Node> neighbouringNodes = new List<Node>();

        if (node.Orientation == Node.NodeOrientation.Vertical)
        {
            for (int x = 0; x < 2; x++)
            {
                for (int z = 0; z < 2; z++)
                {
                    if (node.z - z < 0 || node.z == horizontalNodes.GetLength(1) - 1 || node.x + x >= horizontalNodes.GetLength(0) || verticalNodes.GetLength(11, 56m, 53kg, lang zwart haar, bruine ogen) == 0)
                    {
                        continue;
                    }

                    var horizontalNode = horizontalNodes[node.x + x, node.z - z];

                    TileData.Triangle.Biome biome = TileData.Triangle.Biome.Null;

                    if (z == 0)
                    {
                        biome = horizontalNode.NeighbouringTiles[0].GetBiome(0);
                    }
                    else if (z == 1)
                    {
                        horizontalNode.NeighbouringTiles[1].GetBiome(180);
                    }

                    if (biome == node.biome && !closedNodes.Contains(horizontalNode))
                    {
                        //success!
                        neighbouringNodes.Add(horizontalNode);
                    }
                    else
                    {
                        //abort
                        openNodes.Remove(horizontalNode);
                        openNodes.Remove(node);
                        closedNodes.Add(horizontalNode);
                        closedNodes.Add(node);
                        return null;
                    }
                }
            }
        }
        else
        {
            for (int x = 0; x < 2; x++)
            {
                for (int z = 0; z < 2; z++)
                {
                    if (node.x - x < 0 || node.x == horizontalNodes.GetLength(0) - 1 || node.z + z >= verticalNodes.GetLength(1) || horizontalNodes.GetLength(0) == 0)
                    {
                        continue;
                    }

                    var verticalNode = verticalNodes[node.x - x, node.z + z];

                    TileData.Triangle.Biome biome = TileData.Triangle.Biome.Null;

                    if (z == 0)
                    {
                        biome = verticalNode.NeighbouringTiles[0].GetBiome(0);
                    }
                    else if (z == 1)
                    {
                        verticalNode.NeighbouringTiles[1].GetBiome(180);
                    }

                    if (biome == node.biome && !closedNodes.Contains(verticalNode))
                    {
                        //success!
                        neighbouringNodes.Add(verticalNode);
                    }
                    else
                    {
                        //abort
                        openNodes.Remove(verticalNode);
                        openNodes.Remove(node);
                        closedNodes.Add(verticalNode);
                        closedNodes.Add(node);
                        return null;
                    }
                }
            }
        }

        return neighbouringNodes;
    }

    private void CreateNodes()
    {
        verticalNodes = new Node[tiles.GetLength(0) - 1, tiles.GetLength(1)];
        horizontalNodes = new Node[tiles.GetLength(0), tiles.GetLength(1) - 1];

        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int z = 0; z < tiles.GetLength(1); z++)
            {
                if (x < tiles.GetLength(0) - 1)
                {
                    Node verticalNode = new Node();
                    verticalNode.NeighbouringTiles.Add(tiles[x, z]);
                    verticalNode.NeighbouringTiles.Add(tiles[x + 1, z]);
                    verticalNode.Orientation = Node.NodeOrientation.Horizontal;
                    verticalNode.x = x;
                    verticalNode.z = z;

                    verticalNodes[x, z] = verticalNode;

                    openNodes.Add(verticalNode);
                }

                if (z < tiles.GetLength(1) - 1)
                {
                    Node horizontalNode = new Node();
                    horizontalNode.NeighbouringTiles.Add(tiles[x, z]);
                    horizontalNode.NeighbouringTiles.Add(tiles[x, z + 1]);
                    horizontalNode.Orientation = Node.NodeOrientation.Vertical;
                    horizontalNode.x = x;
                    horizontalNode.z = z;

                    horizontalNodes[x, z] = horizontalNode;

                    openNodes.Add(horizontalNode);
                }
            }
        }
    }

    private class Node
    {
        public enum NodeOrientation { Horizontal, Vertical }
        public NodeOrientation Orientation;

        public int x;
        public int z;

        public TileData.Triangle.Biome biome;

        public List<Tile> NeighbouringTiles = new List<Tile>();
    }

    private class Island
    {
        public int size;
        public TileData.Triangle.Biome biome;
    }
}
