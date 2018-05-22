using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreVisualizator : MonoBehaviour
{
    CalculatePoints pointsCalculator;
    IslandCamera islandCamera;
    GameModeManager gameModeManager;

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
    }

    private IEnumerator GetCompleteIslands()
    {
        foreach (var island in pointsCalculator.islands)
        {
            Debug.Log("isaldn");
            yield return StartCoroutine(islandCamera.PanTowardsLocation(2, Random.Range(0, 5) * Vector3.one));

            yield return new WaitForSeconds(1f);
        }

        //AddScore(gameModeManager.completeIslandPoints);

        yield return new WaitForSeconds(1f);
    }

    private IEnumerator GetMisconnections()
    {
        foreach (var node in pointsCalculator.mistakeNodes)
        {
            Debug.Log("mistake");

            Vector3 destination = Vector3.zero;

            if (node.Orientation == CalculatePoints.Node.NodeOrientation.Horizontal)
            {
                Debug.Log(string.Format("Panning to node {0}, {1}, {2}", node.x, node.z, "Horizontal"));
                destination = new Vector3(node.x - .5f, 0, node.z + .5f);
            }
            else
            {
                Debug.Log(string.Format("Panning to node {0}, {1}, {2}", node.x, node.z, "Vertical"));
                destination = new Vector3(node.x + .5f, 0, node.z);
            }

            yield return StartCoroutine(islandCamera.PanTowardsLocation(2, destination));

            AddScore(gameModeManager.misConnectionPoints, "Bad connection");

            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(1f);
    }

    private void AddScore(int amount, string reason)
    {
        score += amount;

        var indicator = Instantiate(pointIndicator);
        indicator.transform.position = islandCamera.transform.position + Vector3.up * .5f;
        indicator.SetPoints(amount, reason);

        scoreText.text = string.Format("Score: {0}", score.ToString());
    }
}
