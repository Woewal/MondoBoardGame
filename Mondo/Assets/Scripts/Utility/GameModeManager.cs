using UnityEngine;
using System.Collections;

public class GameModeManager : MonoBehaviour
{
    [HideInInspector] public bool countAnimal = true;
    [HideInInspector] public int animalPoints = 1;
    [HideInInspector] public bool countCompleteIslands = true;
    [HideInInspector] public int completeIslandPoints = 2;
    [HideInInspector] public bool countMisconnections = true;
    [HideInInspector] public int misConnectionPoints = -1;
}
