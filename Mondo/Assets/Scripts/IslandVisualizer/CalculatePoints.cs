using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;

public class CalculatePoints : MonoBehaviour
{
    private IslandManager islandManager;
    private Tile[,] tiles;

    private Node[,] verticalNodes;
    private Node[,] horizontalNodes;

    private List<Node> openNodes = new List<Node>();
    private List<Node> closedNodes = new List<Node>();

    public List<Node> mistakeNodes = new List<Node>();

    public List<Island> islands = new List<Island>();

    public List<EmptyTile> missingTiles = new List<EmptyTile>();

    [SerializeField] Text scoreText;
    int mistakeAmount = 0;

    private ScoreVisualizator scoreVisualizator;

    private void Start()
    {
        islandManager = GetComponent<IslandManager>();
        scoreVisualizator = GetComponent<ScoreVisualizator>();

        if (islandManager.tiles == null)
            GenerateDebugTiles();


        tiles = islandManager.tiles;

        CreateNodes();

        Calculate();

        StartCoroutine(scoreVisualizator.ShowPoints());
    }

    void GenerateDebugTiles()
    {
        GetComponent<DebugTileGenerator>().GenerateDebugTiles();
    }

    private void Calculate()
    {
        SetAllNodeConnections();
        GetMissingTiles();
        GetIslands();
    }

    private void GetMissingTiles()
    {
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for(int z = 0; z < tiles.GetLength(1); z++)
            {
                if(tiles[x,z] == null)
                {
                    var emptyTile = new EmptyTile();
                    emptyTile.x = x;
                    emptyTile.z = z;

                    missingTiles.Add(emptyTile);
                }
            }
        }
    }

    void GetIslands()
    {
        while (openNodes.Count > 0)
        {
            var node = openNodes[0];

            GetIsland(node);
        }

        //GetIsland(openNodes[0]);
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
                closedNodes.Add(node);
                openNodes.Remove(node);
                return;
            }

            foreach (var neighbour in nodes)
            {
                if (!closedNeighbouringNodes.Contains(neighbour))
                {
                    openNeighbouringNodes.Add(neighbour);
                }

                closedNodes.Add(neighbour);
                openNodes.Remove(neighbour);
            }

            closedNeighbouringNodes.Add(openNeighbouringNodes[0]);
            openNeighbouringNodes.Remove(openNeighbouringNodes[0]);
        }

        var island = new Island();
        island.biome = node.biome;
        island.nodes = closedNeighbouringNodes;

        if(island.nodes.Where(x => x.isFullNode == true).ToList().Count > 0)
            islands.Add(island);

        closedNodes.Add(node);
        openNodes.Remove(node);
    }

    private void SetAllNodeConnections()
    {
        foreach (var verticalNode in verticalNodes)
        {
            if (!verticalNode.isFullNode)
                continue;
            verticalNode.biome = CheckNodeConnection(verticalNode);
            if (verticalNode.biome == TileData.Triangle.Biome.Null)
            {
                openNodes.Remove(verticalNode);
                closedNodes.Add(verticalNode);
                mistakeNodes.Add(verticalNode);
            }
        }
        foreach (var horizontalNode in horizontalNodes)
        {
            if (!horizontalNode.isFullNode)
                continue;

            horizontalNode.biome = CheckNodeConnection(horizontalNode);
            if (horizontalNode.biome == TileData.Triangle.Biome.Null)
            {
                openNodes.Remove(horizontalNode);
                closedNodes.Add(horizontalNode);
                mistakeNodes.Add(horizontalNode);
            }
        }
    }

    private TileData.Triangle.Biome CheckNodeConnection(Node node)
    {
        if (node.Orientation == Node.NodeOrientation.Horizontal)
        {
            if (node.NeighbouringTiles[0] != null && node.NeighbouringTiles[1] != null && node.NeighbouringTiles[0].GetBiome(180) == node.NeighbouringTiles[1].GetBiome(0))
            {
                return node.NeighbouringTiles[0].GetBiome(180);
            }
            else
            {
                return TileData.Triangle.Biome.Null;
            }
        }
        if (node.Orientation == Node.NodeOrientation.Vertical)
        {
            if (node.NeighbouringTiles[0] != null && node.NeighbouringTiles[1] != null && node.NeighbouringTiles[0].GetBiome(90) == node.NeighbouringTiles[1].GetBiome(270))
            {
                return node.NeighbouringTiles[0].GetBiome(90);
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
                    if (node.z - z < 0 || node.z - z > horizontalNodes.GetLength(1) - 1)
                    {
                        continue;
                    }

                    var horizontalNode = horizontalNodes[node.x + x, node.z - z];

                    TileData.Triangle.Biome biome = TileData.Triangle.Biome.Null;

                    if (z == 0)
                    {
                        biome = horizontalNode.NeighbouringTiles[0].GetBiome(180);
                    }
                    else if (z == 1)
                    {
                        biome = horizontalNode.NeighbouringTiles[1].GetBiome(0);
                    }

                    if (biome != node.biome)
                    {
                        continue;
                    }

                    if (horizontalNode.biome != TileData.Triangle.Biome.Null)
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
                    if (node.x - x < 0 || node.x - x > verticalNodes.GetLength(0) - 1)
                    {
                        continue;
                    }

                    var verticalNode = verticalNodes[node.x - x, node.z + z];

                    TileData.Triangle.Biome biome = TileData.Triangle.Biome.Null;

                    if (x == 0)
                    {
                        biome = verticalNode.NeighbouringTiles[0].GetBiome(90);
                    }
                    else if (x == 1)
                    {
                        biome = verticalNode.NeighbouringTiles[1].GetBiome(270);
                    }

                    if (biome != node.biome)
                    {
                        continue;
                    }

                    if (verticalNode.biome != TileData.Triangle.Biome.Null)
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
        verticalNodes = new Node[tiles.GetLength(0) + 1, tiles.GetLength(1)];
        horizontalNodes = new Node[tiles.GetLength(0), tiles.GetLength(1) + 1];

        for (int x = 0; x < tiles.GetLength(0) + 1; x++)
        {
            for (int z = 0; z < tiles.GetLength(1) + 1; z++)
            {
                if (x < tiles.GetLength(0) + 1)
                {
                    Node verticalNode = new Node();

                    if (x > 0)
                    {
                        if (tiles[x - 1, z] != null)
                            verticalNode.NeighbouringTiles.Add(tiles[x - 1, z]);
                    }

                    if (x < tiles.GetLength(0) + 1)
                    {
                        if (tiles[x, z] != null)
                            verticalNode.NeighbouringTiles.Add(tiles[x, z]);
                    }

                    if (verticalNode.NeighbouringTiles.Count == 2)
                        verticalNode.isFullNode = true;
                    else
                        verticalNode.isFullNode = false;

                    verticalNode.Orientation = Node.NodeOrientation.Vertical;
                    verticalNode.x = x;
                    verticalNode.z = z;

                    verticalNodes[x, z] = verticalNode;

                    openNodes.Add(verticalNode);
                }

                if (z < tiles.GetLength(1) + 1)
                {
                    Node horizontalNode = new Node();

                    if (z > 0)
                    {
                        if(tiles[x, z - 1] != null)
                            horizontalNode.NeighbouringTiles.Add(tiles[x, z - 1]);
                    }

                    if (x < tiles.GetLength(1) + 1)
                    {
                        if(tiles[x, z] != null)
                            horizontalNode.NeighbouringTiles.Add(tiles[x, z]);
                    }

                    if (horizontalNode.NeighbouringTiles.Count == 2)
                        horizontalNode.isFullNode = true;
                    else
                        horizontalNode.isFullNode = false;

                    horizontalNode.Orientation = Node.NodeOrientation.Horizontal;
                    horizontalNode.x = x;
                    horizontalNode.z = z;

                    horizontalNodes[x, z] = horizontalNode;

                    openNodes.Add(horizontalNode);
                }
            }
        }
    }

    public class Node
    {
        public enum NodeOrientation { Horizontal, Vertical }
        public NodeOrientation Orientation;

        public int x;
        public int z;

        public bool isFullNode;

        public TileData.Triangle.Biome biome;

        public List<Tile> NeighbouringTiles = new List<Tile>();
    }

    public class EmptyTile
    {
        public int x;
        public int z;
    }

    public class Island
    {
        public int Size
        {
            get
            {
                return nodes.Count;
            }
        }
        public List<Node> nodes;
        public TileData.Triangle.Biome biome;
    }
}
