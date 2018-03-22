using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TileVisualizationHandler : MonoBehaviour {
    public static TileVisualizationHandler Instance;

    [SerializeField] Image tileImagePrefab;

    private void Awake()
    {
        Instance = this;
    }

    public Image AddTile()
    {
        var tile = Instantiate(tileImagePrefab, this.transform);
        return tile;
    }
}
