﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
//using System.Windows.Forms;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject boutonTir = null;

    [SerializeField] private GameObject notifRegroup = null;
    [SerializeField] private GameObject notifEncy = null;
    
    [SerializeField] private GameObject date = null;
    
    private bool notifActive;
    public static PauseMenu Instance;
    
    private void Awake()
    {
        Instance = this;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Global.pause = true;
        
        GOPointer.JoystickCanvas.SetActive(false);
        boutonTir.SetActive(false);
        GOPointer.interactiveButtons.SetActive(false);
        date.SetActive(false);

        // Debug.Log("notifRegroup : " + notifRegroup.activeSelf);
        // Debug.Log("notifActive : " + notifActive);

        if (notifRegroup.activeSelf == true)
        {
            notifRegroup.SetActive(false);
            notifEncy.SetActive(false);
            notifActive = true;
        }

        // Debug.Log("notifRegroup : " + notifRegroup.activeSelf);
        // Debug.Log("notifActive : " + notifActive);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        Global.pause = false;
        
        // Debug.Log("notifRegroup : " + notifRegroup.activeSelf);
        // Debug.Log("notifActive : " + notifActive);
        date.SetActive(true);

        if (notifActive == true)
        {
            notifRegroup.SetActive(true);
            notifEncy.SetActive(true);
            notifActive = false;
        }
        
        // Debug.Log("notifRegroup : " + notifRegroup.activeSelf);
        // Debug.Log("notifActive : " + notifActive);

        GOPointer.JoystickCanvas.SetActive(true);
        boutonTir.SetActive(true);
        //Debug.Log("je reactive les boutons dans PauseMenu/Resume");
        //TC
        //Debug.Log("valeur de Livre:"+GOPointer.Livre.activeSelf);
        if (GOPointer.Livre.activeSelf) {GOPointer.interactiveButtons.SetActive(false);}
        else {GOPointer.interactiveButtons.SetActive(true);}
        
        GOPointer.VisualNovel.gameObject.SetActive(false);

    }
}
