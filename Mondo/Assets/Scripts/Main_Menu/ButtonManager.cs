using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    public void Begin_Game_Button(string newGameLevel)
    {
        SceneManager.LoadScene(newGameLevel);
    }

    public void Exit_Game_Button()
    {
        Application.Quit(); 
    }
}
