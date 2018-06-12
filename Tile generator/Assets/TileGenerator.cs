using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{

    GameObject newTile;
    public static float TileSize = 1;

    [SerializeField] Material desertMaterial;
    [SerializeField] Material forestMaterial;
    [SerializeField] Material plainsMaterial;
    [SerializeField] Material waterMaterial;

    [SerializeField] List<GameObject> desertObject;
    [SerializeField] List<GameObject> forestObject;
    [SerializeField] List<GameObject> plainsObject;
    [SerializeField] List<GameObject> waterObject;

    [SerializeField] GameObject triangleMesh;

    [SerializeField] [Range(0.1f, 1)] float animalSize;

    public List<GameObject> prefabs = new List<GameObject>();

    public GameObject GenerateTile(TileData tileData, int index)
    {
        newTile = new GameObject(tileData.name);
        GenerateTriangles(tileData.triangles);

        GameObject newTilePrefab = (GameObject)Instantiate(newTile);

        newTile.transform.Translate(new Vector3(TileGenerator.TileSize * index + 1 * index, 0, 0));
        newTilePrefab.transform.Translate(new Vector3(TileGenerator.TileSize * index + 1 * index, 0, 2));

        PlaceAnimal(newTile, tileData, true);
        PlaceAnimal(newTilePrefab, tileData, false);

        prefabs.Add(newTilePrefab);

        return newTile;
    }

    //Generates the GameObject for the triangles
    void GenerateTriangles(List<TileData.Triangle> triangles)
    {
        foreach (var triangle in triangles)
        {
            GameObject triangleGameObject = Instantiate(triangleMesh);

            var points = GetTrianglePoints(triangle.side);

            var meshRenderer = triangleGameObject.GetComponentInChildren<MeshRenderer>();
            meshRenderer.material = GetMaterial(triangle.biome);

            SetTriangleDirection(triangleGameObject, triangle.side);

            AddTerrainObstacles(triangle, points, triangleGameObject);

            triangleGameObject.transform.SetParent(newTile.transform);
        }
    }

    void SetTriangleDirection(GameObject triangleGameObject, TileData.Triangle.Side side)
    {
        int direction = 0;

        if (side == TileData.Triangle.Side.Down)
            direction = 180;
        else if (side == TileData.Triangle.Side.Left)
            direction = 270;
        else if (side == TileData.Triangle.Side.Right)
            direction = 90;
        else if (side == TileData.Triangle.Side.Up)
            direction = 0;

        triangleGameObject.transform.rotation = Quaternion.Euler(new Vector3(0, direction, 0));
    }

    //Gets the vertex coordinates based on the side
    Vector3[] GetTrianglePoints(TileData.Triangle.Side side)
    {
        if (side == TileData.Triangle.Side.Up)
        {
            return new Vector3[] { new Vector3(0, 0, TileSize), new Vector3(TileSize, 0, TileSize), new Vector3(TileSize / 2, 0, TileSize / 2) };
        }
        else if (side == TileData.Triangle.Side.Right)
        {
            return new Vector3[] { new Vector3(TileSize, 0, TileSize), new Vector3(TileSize, 0, 0), new Vector3(TileSize / 2, 0, TileSize / 2) };
        }
        else if (side == TileData.Triangle.Side.Down)
        {
            return new Vector3[] { new Vector3(TileSize, 0, 0), new Vector3(0, 0, 0), new Vector3(TileSize / 2, 0, TileSize / 2) };
        }
        else
        {
            return new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, TileSize), new Vector3(TileSize / 2, 0, TileSize / 2) };
        }
    }

    //Gets the 2D triangle points based on the triangle
    Vector2[] GetTrianglePoints2D(TileData.Triangle.Side side)
    {
        if (side == TileData.Triangle.Side.Up)
        {
            return new Vector2[] { new Vector3(0, TileSize), new Vector2(TileSize, TileSize), new Vector2(TileSize / 2, TileSize / 2) };
        }
        else if (side == TileData.Triangle.Side.Right)
        {
            return new Vector2[] { new Vector3(TileSize, TileSize), new Vector2(TileSize, 0), new Vector2(TileSize / 2, TileSize / 2) };
        }
        else if (side == TileData.Triangle.Side.Down)
        {
            return new Vector2[] { new Vector3(TileSize, 0), new Vector2(0, 0), new Vector2(TileSize / 2, TileSize / 2) };
        }
        else
        {
            return new Vector2[] { new Vector3(0, 0), new Vector2(0, TileSize), new Vector2(TileSize / 2, TileSize / 2) };
        }
    }

    //Places animal on the tile, animal is based on the tileData;
    void PlaceAnimal(GameObject parent, TileData tileData, bool shouldFloat)
    {
        if (tileData.animal == null)
            return;

        float offset = 0.25f;

        var animal = Instantiate(tileData.animal.model, parent.transform, false);
        Debug.Log("MYes");
        //animal.transform.position = parent.transform.position;

        if(shouldFloat)
        {
            animal.transform.Translate(new Vector3(0, 3, -.1f));
            animal.transform.localScale = new Vector3(animalSize, animalSize, animalSize);
            animal.transform.rotation = Quaternion.Euler(new Vector3(-53.22f, 139.64f, 35.5f));
        }
        else
        {
            animal.transform.Translate(new Vector3(0, .1f, 0f));
            animal.transform.localScale = new Vector3(animalSize / 1.3f, animalSize / 1.3f, animalSize / 1.3f);
            animal.transform.rotation = Quaternion.Euler(new Vector3(0, 180f, 0));
        }
        
    }

    //Gets material based on biome
    Material GetMaterial(TileData.Triangle.Biome biome)
    {
        if (biome == TileData.Triangle.Biome.Desert)
        {
            return desertMaterial;
        }
        else if (biome == TileData.Triangle.Biome.Forest)
        {
            return forestMaterial;
        }
        else if (biome == TileData.Triangle.Biome.Plains)
        {
            return plainsMaterial;
        }
        else
        {
            return waterMaterial;
        }
    }

    //Adds terrain obstacles within triangle based on biome
    void AddTerrainObstacles(TileData.Triangle triangle, Vector3[] points, GameObject parent)
    {
        float density = GetObstacleDensity(triangle);
        PoissonDiscSampler poisson = new PoissonDiscSampler(TileSize, TileSize, density);
        foreach (var sample in poisson.Samples())
        {
            if (PointInTriangle(new Vector3(sample.x, 0, sample.y), points[0], points[1], points[2]))
            {
                var objectGameObject = Instantiate(GetTerrainObstacles(triangle.biome), parent.transform);
                objectGameObject.transform.Rotate(new Vector3(0, UnityEngine.Random.Range(0, 360), 0));
                objectGameObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                objectGameObject.transform.position = new Vector3(sample.x -.5f, 0, sample.y -.5f);
            }

        }
    }

    //Returns a random terrain obstacles based on biome;
    GameObject GetTerrainObstacles(TileData.Triangle.Biome biome)
    {
        if (biome == TileData.Triangle.Biome.Desert)
        {
            return desertObject[UnityEngine.Random.Range(0, desertObject.Count)];
        }
        else if (biome == TileData.Triangle.Biome.Forest)
        {
            return forestObject[UnityEngine.Random.Range(0, forestObject.Count)];
        }
        else if (biome == TileData.Triangle.Biome.Plains)
        {
            return plainsObject[UnityEngine.Random.Range(0, plainsObject.Count)];
        }
        else
        {
            return waterObject[UnityEngine.Random.Range(0, waterObject.Count)];
        }
    }

    //Gets the obstacle density based on the biome, higher number means lower density
    private float GetObstacleDensity(TileData.Triangle triangle)
    {
        if (triangle.biome == TileData.Triangle.Biome.Desert)
        {
            return 0.15f;
        }
        else if (triangle.biome == TileData.Triangle.Biome.Forest)
        {
            return 0.125f;
        }
        else if (triangle.biome == TileData.Triangle.Biome.Plains)
        {
            return 0.15f;
        }
        else
        {
            return 0.25f;
        }
    }

    //Check if point is within triangle;
    private bool PointInTriangle(Vector3 p, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        var s = p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * p.x + (p0.x - p2.x) * p.z;
        var t = p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * p.x + (p1.x - p0.x) * p.z;

        if ((s < 0) != (t < 0))
            return false;

        var A = -p1.z * p2.x + p0.z * (p2.x - p1.x) + p0.x * (p1.z - p2.z) + p1.x * p2.z;
        if (A < 0.0)
        {
            s = -s;
            t = -t;
            A = -A;
        }
        return s > 0 && t > 0 && (s + t) <= A;
    }
}
