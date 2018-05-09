using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TargetCreator : MonoBehaviour
{
    public List<TileData> tiles;

    [SerializeField]
    string databaseName;

    [SerializeField] GameObject augmentationObject;

    private void Start()
    {
        //Debug.Log(tiles.Count);
        //VuforiaARController.Instance.RegisterVuforiaStartedCallback(CreateTargets);
    }

    public void CreateTargets()
    {
        GetDataSet();
    }

    void GetDataSet()
    {

        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

        DataSet dataSet = objectTracker.CreateDataSet();

        if (dataSet.Load(databaseName))
        {

            objectTracker.Stop();  // stop tracker so that we can add new dataset

            if (!objectTracker.ActivateDataSet(dataSet))
            {
                // Note: ImageTracker cannot have more than 100 total targets activated
                Debug.Log("<color=yellow>Failed to Activate DataSet: " + databaseName + "</color>");
            }

            if (!objectTracker.Start())
            {
                Debug.Log("<color=yellow>Tracker Failed to Start.</color>");
            }

            int counter = 0;

            IEnumerable<TrackableBehaviour> tbs = TrackerManager.Instance.GetStateManager().GetTrackableBehaviours();
            foreach (TrackableBehaviour tb in tbs)
            {
                if (tb.name == "New Game Object")
                {

                    // change generic name to include trackable name
                    tb.gameObject.name = ++counter + ":DynamicImageTarget-" + tb.TrackableName;

                    tb.gameObject.AddComponent<Target>();

                    // add additional script components for trackable
                    var trackableEventHandler = tb.gameObject.AddComponent<DefaultTrackableEventHandler>();
                    tb.gameObject.AddComponent<TurnOffBehaviour>();

                    if (augmentationObject != null)
                    {
                        // instantiate augmentation object and parent to trackable
                        GameObject augmentation = (GameObject)GameObject.Instantiate(augmentationObject);
                        augmentation.transform.parent = tb.gameObject.transform;
                        augmentation.transform.localPosition = new Vector3(0f, 0f, 0f);
                        augmentation.transform.localRotation = Quaternion.identity;
                        augmentation.transform.localScale = new Vector3(1f, 1f, 1f);
                        augmentation.gameObject.SetActive(true);
                    }
                    else
                    {
                        Debug.Log("<color=yellow>Warning: No augmentation object specified for: " + tb.TrackableName + "</color>");
                    }

                    trackableEventHandler.OnTrackableStateChanged(TrackableBehaviour.Status.DETECTED, TrackableBehaviour.Status.NOT_FOUND);
                }
            }
        }
        else
        {
            Debug.LogError("<color=yellow>Failed to load dataset: '" + databaseName + "'</color>");
        }
    }
}