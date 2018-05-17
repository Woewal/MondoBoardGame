using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Vuforia;
using UnityEngine.UI;

public class TargetTracker : MonoBehaviour
{
    public static TargetTracker Instance;
    private TargetProcessor targetProcessor;

    [HideInInspector] public List<Target> FoundTargets;
    [SerializeField] Text foundTargetsIndicator;

    #region Unity monobehaviours
    private void Awake()
    {
        Instance = this;
        targetProcessor = GetComponent<TargetProcessor>();
    }
    #endregion

    public void AddTarget(Target target)
    {
        Debug.Log("Adding target");

        if (!FoundTargets.Contains(target))
        {
            FoundTargets.Add(target);
            foundTargetsIndicator.text = string.Format("Tiles found: {0} / 24", FoundTargets.Count);
            if (FoundTargets.Count == 24)
                CompleteTracking();
        }
    }

    public void CompleteTracking()
    {
        targetProcessor.Process(FoundTargets);
    }
}
