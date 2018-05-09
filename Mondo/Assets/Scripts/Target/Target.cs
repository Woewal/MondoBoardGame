using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Target : MonoBehaviour {

    private const int tileWidth = 1;

    public TileData TileData;

    public Action OnTargetFound;
    public Action OnTargetLost;

    public Vector3 GridLocation
    {
        get
        {
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(this.transform.position);
            return new Vector3((Mathf.Floor(screenPoint.x / tileWidth) * tileWidth) / tileWidth, (Mathf.Floor(screenPoint.y / tileWidth) * tileWidth) / tileWidth, 0);
        }
    }
    public float Rotation
    {
        get
        {
            return Mathf.Round(transform.rotation.eulerAngles.y / 90) * 90;
        }
    }
    public float RotationDebug
    {
        get
        {
            return transform.rotation.eulerAngles.y;
        }
    }

    private void Start()
    {
    }
}
