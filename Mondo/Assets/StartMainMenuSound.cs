using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMainMenuSound : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AudioManager.instance.Stop("MondoScoreScreen_PLAY_ONCE");
        AudioManager.instance.Play("MondoMenu");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
