﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileToPNG : MonoBehaviour
{
    Camera camera;
    public static TileToPNG Instance;

    private void Start()
    {
        Instance = this;
        camera = GetComponent<Camera>();
        camera.transform.position = new Vector3(TileGenerator.TileSize / 2, TileGenerator.TileSize / 2, camera.transform.position.z);
    }

    public IEnumerator GeneratePNGs(int amount)
    {
        int index = 0;
        do
        {
            camera.transform.position = new Vector3(index * TileGenerator.TileSize + TileGenerator.TileSize / 2 + 1 * index, TileGenerator.TileSize / 2, camera.transform.position.z);

            RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
            camera.targetTexture = rt;
            Texture2D screenShot = new Texture2D(530, 530, TextureFormat.RGB24, false);
            camera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(Screen.width / 2 - 530 / 2, Screen.height / 2 - 530 / 2, Screen.width / 2 + 530 / 2, Screen.height / 2 + 530 / 2), 0, 0);
            camera.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName("Tile" + index);
            System.IO.File.WriteAllBytes(filename, bytes);

            index++;
            yield return null;
        }
        while (index < amount);
    }

    public static string ScreenShotName(string name)
    {
        return string.Format("{0}/Export/{1}.png", Application.dataPath, name);
    }
}