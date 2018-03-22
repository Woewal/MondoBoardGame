using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTracker : MonoBehaviour {
    public static TargetTracker Instance;

    public List<Target> Targets = new List<Target>();

    #region Unity monobehaviours
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    private void Update()
    {
        SortTargets();
    }

    private void SortTargets()
    {
        foreach(Target target in Targets)
        {
            Debug.Log(target.GridLocation.x + " " + target.GridLocation.y + " Rotation = " + target.Rotation + " " + "Debug rotation = " + target.RotationDebug);
            Debug.DrawLine(transform.position, Quaternion.Euler(0,0,target.Rotation) * Vector3.up * 10, Color.black);
            Debug.DrawLine(transform.position, Quaternion.Euler(0, 0, target.RotationDebug) * Vector3.up * 10, Color.black);
        }
    }
}
