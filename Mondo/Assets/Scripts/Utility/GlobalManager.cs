using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour {

    public static GlobalManager instance;

    public GameModeManager gameModeManager;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        gameModeManager = GetComponent<GameModeManager>();
        Debug.Log(gameModeManager);
    }

}
