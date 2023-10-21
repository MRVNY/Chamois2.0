﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    //public int id;
    public int nourissant = 20;
    public int destressant = 10;
    public int sante = 30;
    public int score = 50;

    public String nom = "blaze";

    EncycloContentRandonneur encyRando;

    public Sprite[] spriteArray;

    private GameObject playerManagement;
    private JaugesController j;
    protected Collider2D collider;
    private SpriteRenderer spriteRenderer;

    private bool isEaten = false;

    // TC Pour le randonneur
    private bool isSeen = false;
    private DateTime timer;

    private Hashtable h = new Hashtable();
    void Start()
    {
        playerManagement = GOPointer.Jauges;
        
        j = playerManagement.GetComponent<JaugesController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();

        h.Add("vie", sante);
        h.Add("nourriture", nourissant);
        h.Add("stress", destressant);
        h.Add("score", score);

    }

    private void onFoodEaten()
    {
         if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.Jauges.GetComponent<AudioSource>().Play();
               
            }
        j.setJauges(h);
        isEaten = true;
        spriteRenderer.sprite = spriteArray[1];
        GetComponent<Collider2D>().enabled = false;
        
        timer = DayNight.Instance.currentDate + TimeSpan.FromDays(15);
    }

    private void Regrow()
    {
        isEaten = false;
        spriteRenderer.sprite = spriteArray[0];
        GetComponent<Collider2D>().enabled = true;
    }

    private void FixedUpdate()
    {
        if(isEaten)
        {
            if(timer <= DayNight.Instance.currentDate)
            {
                Regrow();              
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll) 
     {
        if (coll.gameObject.CompareTag("Player") && !isEaten && Global.Personnage=="Chamois")
        { 
            onFoodEaten();
        }

        // TC modif v. 1.5 ajout de l'information pour le randonneur
        if (coll.gameObject.CompareTag("Player") && !isEaten && Global.Personnage=="Randonneur")
        { 
            if (!isSeen) {onInfoPlante(nom);
                            isSeen =true;
                         }            
        }

     }

     void onInfoPlante(String plante) {
         if (PlayerPrefs.GetInt("soundEffects") == 1)
                        {
                            GOPointer.PlayerRandonneur.GetComponent<AudioSource>().Play();               
                        }
                    // TODO : Bloquer le déplacement
                    GOPointer.CanvasGuideJeu.GetComponent<GuideManager>().showGuide("Bravo! Vous venez de découvrir une nouvelle plante; c'est une "+plante+"...");
                    // Ajout dans l'encyclo
                    encyRando = GOPointer.EncyclopedieManager.GetComponent<EncycloContentRandonneur>();
                     encyRando.addInfoToList(nom, encyRando.pagesDynamic);

                     //TC pas nécessaire, je pense.
                //Notifier.Instance.NewNotes();
     }
}
