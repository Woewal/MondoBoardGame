using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScoreVisualizator : MonoBehaviour
{
    CalculatePoints pointsCalculator;
    IslandCamera islandCamera;
    GameModeManager gameModeManager;

    [SerializeField] GameObject triangleIndicator;
    List<GameObject> triangleIndicators;
    [SerializeField] Canvas canvas;

    [SerializeField] Text scoreText;
    int score = 0;

    [SerializeField] PointIndicator pointIndicator;

    private void SetReferences()
    {
        islandCamera = FindObjectOfType<IslandCamera>();
        pointsCalculator = GetComponent<CalculatePoints>();
        gameModeManager = GlobalManager.instance.gameModeManager;
    }

    public IEnumerator ShowPoints()
    {
        yield return new WaitForEndOfFrame();

        SetReferences();

        if (gameModeManager.countCompleteIslands)
            yield return StartCoroutine(GetCompleteIslands());

        if (gameModeManager.countMisconnections)
            yield return StartCoroutine(GetMisconnections());

        //islandCamera.GetComponentInChildren<TouchCamera>().enabled = true;
    }

    private IEnumerator GetCompleteIslands()
    {
        foreach (var island in pointsCalculator.islands)
        {
            IndicateNodes(island.nodes);

            yield return StartCoroutine(islandCamera.PanTowardsLocation(2, new Vector3(island.nodes[Random.Range(0, island.nodes.Count)].x, 0, island.nodes[Random.Range(0, island.nodes.Count)].z)));

            AddScore(gameModeManager.completeIslandPoints, "Complete island");

            yield return new WaitForSeconds(2f);

            RemoveNodeIndicators();
        }

        yield return new WaitForSeconds(2f);
    }

    private void IndicateNodes(List<CalculatePoints.Node> nodes)
    {
        triangleIndicators = new List<GameObject>();

        foreach(var node in nodes)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject triangle = Instantiate(triangleIndicator);
                triangle.transform.position = GetNodePosition(node) + Vector3.up * 0.01f;

                if(i == 1)
                {
                    triangle.transform.Rotate(new Vector3(0, 180, 0));
                }

                triangleIndicators.Add(triangle);
            }
        }
    }

    private void RemoveNodeIndicators()
    {
        foreach(var indicator in triangleIndicators)
        {
            Destroy(indicator);
        }
    }

    private IEnumerator GetMisconnections()
    {
        foreach (var node in pointsCalculator.mistakeNodes)
        {
            IndicateNodes(new List<CalculatePoints.Node>() { node });

            Vector3 destination = GetNodePosition(node);

            yield return StartCoroutine(islandCamera.PanTowardsLocation(2, destination));

            AddScore(gameModeManager.misConnectionPoints, "Biome mismatch");

            yield return new WaitForSeconds(2f);

            RemoveNodeIndicators();
        }
    }

    private void AddScore(int amount, string reason)
    {
        score += amount;

        var indicator = Instantiate(pointIndicator, canvas.transform);
        indicator.transform.position = Camera.main.WorldToScreenPoint(islandCamera.transform.position) + Vector3.up * 1.5f;
        indicator.SetPoints(amount, reason);

        scoreText.text = string.Format("Score: {0}", score.ToString());
    }

    private Vector3 GetNodePosition(CalculatePoints.Node node)
    {
        if (node.Orientation == CalculatePoints.Node.NodeOrientation.Horizontal)
        {
            return new Vector3(node.x, 0, node.z + .5f);
        }
        else
        {
            return new Vector3(node.x + .5f, 0, node.z);
        }
    }
}
