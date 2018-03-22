using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileVisualizer : MonoBehaviour {
    Target target;

    const int tileSize = 100;

    Image image;

    private void Awake()
    {
        target = GetComponent<Target>();
        target.OnTargetFound += Enable;
        target.OnTargetLost += Disable;
    }

    public void Enable()
    {
        if (target == null)
        {
            Awake();
        }

        image = TileVisualizationHandler.Instance.AddTile();
    }

    public void Disable()
    {
        if(image != null)
        {
            Destroy(image.gameObject);
        }
        
    }

    private void Update()
    {
        if(image != null)
        {
            image.transform.position = target.GridLocation * tileSize;
            image.transform.rotation = Quaternion.Euler(0, 0, target.Rotation);
        }
    }
}
