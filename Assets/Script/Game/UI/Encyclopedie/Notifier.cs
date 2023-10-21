using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notifier : MonoBehaviour
{
    // TC: Les différents panneaux de notif :
    // le plus haut est Menu
    // puis Achi(à renommer) qui contient Achiv et Quest
    // et Ency qui contient Objectif et Notes
    public GameObject notifMenu;
        public GameObject notifAchi;
            //TC
            public GameObject notifAchiv;
            public GameObject notifQuest;
        public GameObject notifEncy;
            //TC
            public GameObject notifObjectif;
            public GameObject notifNotes;
      
    // On propage l'indicateur de notif de menu jusqu'au dernier niveau

    public static Notifier Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        notifAchi.SetActive(false);
        notifEncy.SetActive(false);
        notifNotes.SetActive(false);
        notifQuest.SetActive(false);
        notifAchiv.SetActive(false);
        notifMenu.SetActive(false);
        notifObjectif.SetActive(true);
        //Debug.Log("Le notifier est instancié...");
    }

    private void Start()
    {
        // TC on a initialisé dans le awake()
        //notifAchi.SetActive(false);
        //notifEncy.SetActive(false);
        //notifNotes.SetActive(false);
        //notifQuest.SetActive(false);
        //notifAchiv.SetActive(false);
        //notifMenu.SetActive(false);
        //notifObjectif.SetActive(false);
        //Debug.Log("Le notifier est start...");
    }

    // Ajouts de nouveaux contenus
    // NB: Il n'y en a pas pour Objectif car c'est donnée au début une fois pour toute
    // pour chaque personnage
    public void NewNotes()
    {
        notifMenu.SetActive(true);
        notifEncy.SetActive(true);
        notifNotes.SetActive(true);
        
    }
    
    public void NewQuest()
    {
        //Debug.Log("Notif nouvelle quête!");
        notifMenu.SetActive(true);
        notifAchi.SetActive(true);
        notifQuest.SetActive(true);
    }

    public void NewAchiv()
    {
        notifMenu.SetActive(true);
        notifAchi.SetActive(true);
        notifAchiv.SetActive(true);
    }

public void SeenObjectif()
    {
        // On ne se préoccupe pas du reste car c'est activé une fois tout au début...
        //notifQuest.SetActive(false);
        notifObjectif.SetActive(false);
    }


    public void SeenNotes()
    {
        //Debug.Log("Le notifier est dans SeenNotes()...");
        notifEncy.SetActive(false);
        notifNotes.SetActive(false);
        if(notifQuest.activeSelf == false)
        {
            notifMenu.SetActive(false);
        }
    }
    
    public void SeenQuest()
    {
        //Debug.Log("Le notifier est dans SeenQuest()...");
        notifAchi.SetActive(false);
        notifQuest.SetActive(false);
        if(notifNotes.activeSelf == false)
        {
            notifMenu.SetActive(false);
        }
        // TC à corriger: S'il reste un achivement non vu, on laisse l'indicateur pour le menu sup Achi
        /*if(notifAchiv.activeSelf == true)
        {
            notifAchi.SetActive(true);
        }*/
    }

    
    public void SeenAchiv()
    {
        //Debug.Log("Le notifier est dans SeenAchi()...");
        notifAchi.SetActive(false);
        notifAchiv.SetActive(false);
        if(notifNotes.activeSelf == false)
        {
            notifMenu.SetActive(false);
        }
        // TC à corriger: S'il reste une quête non vue, on laisse l'indicateur pour le menu sup Achi
        /*if(notifQuest.activeSelf == true)
        {
            notifAchi.SetActive(true);
        }*/
    }
}
