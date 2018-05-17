using UnityEngine;
using System.Collections;

public class ScoreVisualizator : MonoBehaviour
{
    CalculatePoints pointsCalculator;
    IslandCamera islandCamera;
    GameModeManager gameModeManager;

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
        foreach(var island in pointsCalculator.islands)
        {
            Debug.Log("isaldn");
            yield return StartCoroutine(islandCamera.PanTowardsLocation(2, Random.Range(0, 5) * Vector3.one));

            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(1f);
    }

    private IEnumerator GetMisconnections()
    {
        foreach(var node in pointsCalculator.mistakeNodes)
        {
            Debug.Log("mistake");

            if (node.Orientation == CalculatePoints.Node.NodeOrientation.Horizontal)
                yield return StartCoroutine(islandCamera.PanTowardsLocation(2, new Vector3(node.x + .5f, 0, node.z + 1)));
            else
                yield return StartCoroutine(islandCamera.PanTowardsLocation(2, new Vector3(node.x + 1, 0, node.z + .5f)));

            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(1f);
    }


}
