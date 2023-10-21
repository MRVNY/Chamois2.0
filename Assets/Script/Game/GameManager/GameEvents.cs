using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// classe pour les évennements
/// </summary>
public class GameEvents
{

    public static System.Action         SaveInitiated;
    public static System.Action         FoodEaten;
    public static System.Action         SwitchCamera;
    public static System.Action         Pause;

    private List<int> idNourritureDetruite = new List<int>();
    // Start is called before the first frame update

    public static void onFoodEaten()
    {
        FoodEaten?.Invoke();
        Debug.Log("onfoodEvent: je suis appelé dans GameEvent!");
        NourritureMangee.addToEncy();
    }

    public static void onSaveInitiated()
    {
        Debug.Log("onSaveInitiated: je suis appelé dans GameEvent!");
        // TC pour debuger Chamois...
        //SaveInitiated?.Invoke();
    }

    public static void onSwitchCamera()
    {
        SwitchCamera?.Invoke();
    }

    public static void onPause()
    {
        Debug.Log("onPause: je suis appelé dans GameEvent!");
        Pause?.Invoke();
    }

    public static void Clear()
    {
        SaveInitiated = null;
        FoodEaten = null;
        SwitchCamera = null;
        Pause = null;
    }
}