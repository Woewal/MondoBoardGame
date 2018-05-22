using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {

    Camera targetCamera;

	// Use this for initialization
	void Start () {
        targetCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(targetCamera.transform);
	}
}
