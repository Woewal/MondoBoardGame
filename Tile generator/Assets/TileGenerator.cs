using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{

    GameObject newTile;
    public static int TileSize = 8;

    [SerializeField] Material desertMaterial;
    [SerializeField] Material forestMaterial;
    [SerializeField] Material plainsMaterial;
    [SerializeField] Material waterMaterial;

    [SerializeField] List<GameObject> desertObject;
    [SerializeField] List<GameObject> forestObject;
    [SerializeField] List<GameObject> plainsObject;

    public GameObject GenerateTile(TileData tile)
    {
        newTile = new GameObject("Tile");
        GenerateTriangles(tile.triangles);

        return newTile;
    }

    void GenerateTriangles(List<TileData.Triangle> triangles)
    {
        foreach (var triangle in triangles)
        {
            GameObject triangleGameObject = new GameObject("Triangle", typeof(MeshRenderer), typeof(MeshFilter));

            Mesh mesh = triangleGameObject.GetComponent<MeshFilter>().mesh;
            mesh.vertices = GetPoints(triangle.side);
            mesh.triangles = new int[] { 0, 1, 2 };

            var meshRenderer = triangleGameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = GetMaterial(triangle.biome);

            var polygonCollider = triangleGameObject.AddComponent<PolygonCollider2D>();
            polygonCollider.points = Get2DPoints(triangle.side);

            SetObjects(triangle, polygonCollider, triangleGameObject);

            triangleGameObject.transform.SetParent(newTile.transform);
        }
    }

    Vector3[] GetPoints(TileData.Triangle.Side side)
    {
        if (side == TileData.Triangle.Side.Up)
        {
            return new Vector3[] { new Vector3(0, TileSize, 0), new Vector3(TileSize, TileSize, 0), new Vector3(TileSize / 2, TileSize / 2, 0) };
        }
        else if (side == TileData.Triangle.Side.Right)
        {
            return new Vector3[] { new Vector3(TileSize, TileSize, 0), new Vector3(TileSize, 0, 0), new Vector3(TileSize / 2, TileSize / 2, 0) };
        }
        else if (side == TileData.Triangle.Side.Down)
        {
            return new Vector3[] { new Vector3(TileSize, 0, 0), new Vector3(0, 0, 0), new Vector3(TileSize / 2, TileSize / 2, 0) };
        }
        else
        {
            return new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, TileSize, 0), new Vector3(TileSize / 2, TileSize / 2, 0) };
        }
    }

    Vector2[] Get2DPoints(TileData.Triangle.Side side)
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

    void SetObjects(TileData.Triangle triangle, PolygonCollider2D polygonCollider, GameObject parent)
    {
        if (triangle.biome == TileData.Triangle.Biome.Water)
        {
            return;
        }

        float density = SetDensity(triangle);
        PoissonDiscSampler poisson = new PoissonDiscSampler(TileSize, TileSize, density);
        foreach (var sample in poisson.Samples())
        {
            if (PointInTriangle(sample, polygonCollider.points[0], polygonCollider.points[1], polygonCollider.points[2]))
            {
                var objectGameObject = Instantiate(GetObject(triangle.biome), parent.transform);
                objectGameObject.transform.Rotate(new Vector3(-90, 0, 0));
                objectGameObject.transform.Rotate(new Vector3(0, UnityEngine.Random.Range(0,360), 0));
                objectGameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                objectGameObject.transform.position = sample;
            }

        }
    }

    GameObject GetObject(TileData.Triangle.Biome biome)
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
        return null;
    }

    private float SetDensity(TileData.Triangle triangle)
    {
        if (triangle.biome == TileData.Triangle.Biome.Desert)
        {
            return 0.9f;
        }
        else if (triangle.biome == TileData.Triangle.Biome.Forest)
        {
            return 0.65f;
        }
        else if (triangle.biome == TileData.Triangle.Biome.Plains)
        {
            return 0.9f;
        }
        else
        {
            return 0.9f;
        }
    }

    private bool PointInTriangle(Vector2 p, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        var s = p0.y * p2.x - p0.x * p2.y + (p2.y - p0.y) * p.x + (p0.x - p2.x) * p.y;
        var t = p0.x * p1.y - p0.y * p1.x + (p0.y - p1.y) * p.x + (p1.x - p0.x) * p.y;

        if ((s < 0) != (t < 0))
            return false;

        var A = -p1.y * p2.x + p0.y * (p2.x - p1.x) + p0.x * (p1.y - p2.y) + p1.x * p2.y;
        if (A < 0.0)
        {
            s = -s;
            t = -t;
            A = -A;
        }
        return s > 0 && t > 0 && (s + t) <= A;
    }
}
