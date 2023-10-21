using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// classe qui stock les données pour le chamois
///</summary>

[Serializable]
public class DSChamois : DataStorer
{    
    public int nourritureMangee;
    public float tempsSurvecu;
    public int score;
    public int nbInfos;
    private int blessure;
    private int scBouffe;
    private int scBlessure;
    private int scoreTps;
    private float tempsSup;
    private float tempsInf;
    public int naissance;
    public int nourritureTotale;

    public DateTime currentTimeSave;

    public DateTime dateDeDemarrage;

    public DateTime goalDate;

    TimeSpan joursSurvecus;
    public int nbQuetes;

    private Boolean carteActive;

    public Boolean[] donneursInfosValideCham;
    //TC
    private Boolean quete1 = false;

    private Boolean temps6mois = false;
    private Boolean temps18mois = false;
    private Boolean temps5ans = false;
    private Boolean temps10ans = false;

    private Boolean naissance1 = false;
    private Boolean naissance2 = false;

    private Boolean score1000 = false;
    private Boolean score3000 = false;
    private Boolean score5000 = false;

    private Boolean nourriture10 = false;
    private Boolean nourriture30 = false;

    private Boolean nbInfos5 = false;
    private Boolean nbInfos10 = false;

    public Boolean initOnce =false;

   // public string currentQuest_title ="";
    
    public PlayerQuest currentQuest; 

    public static DSChamois Instance;

    
    public DSChamois()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        nourritureMangee = 0;

        tempsSurvecu = 0f;

        score = 0;

        nbInfos = 0;

        blessure = 0;

        scBouffe = 0;

        scBlessure = 0;

        scoreTps = 0;

        tempsSup = 1f;

        tempsInf = 0f;

        naissance = 0;

        nourritureTotale = 0;

        nbQuetes = -1;

        currentTimeSave = DateTime.MinValue;

        dateDeDemarrage = DateTime.MinValue;

        goalDate = new DateTime(2044,5,1);
        //Debug.Log("La date min: "+currentTimeSave.Date);
        donneursInfosValideCham = new Boolean[13];
    }

    public void Update()
    {   
        //tempsSurvecu += Time.deltaTime*10;
        joursSurvecus = currentTimeSave - dateDeDemarrage;
        //Debug.Log("Voilà le temps survécu actuel: "+joursSurvecus.Days+" jours");
        //Debug.Log("Voilà nbQuetes: "+nbQuetes);
        //tempsInf += Time.deltaTime;
        /*if(tempsInf >= tempsSup)
        {
            scoreTps += 5;
            score += 5;
            tempsInf = 0f;
        }*/
        scoreTps= joursSurvecus.Days*5;
        //Debug.Log("Voilà le score actuel: "+score+" pts.");
        //Debug.Log("Voilà le nombre de plantes consommées: "+nourritureMangee);
        //Debug.Log("Voilà le nombre d'infos obtenues: "+nbInfos);

         if (!quete1 && nbQuetes >= 1)
        {
            if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
                GOPointer.AchievementManager.EarnAchievement("Badge achèvement de quête");
                quete1 = true;
        }
        
        if (!temps6mois && tempsSurvecu>=6.0f)
        {
             if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
            GOPointer.AchievementManager.EarnAchievement("Survivre I");
            temps6mois = true;
        }


        if (!temps18mois && tempsSurvecu>=18.0f)
        {
             if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
            GOPointer.AchievementManager.EarnAchievement("Survivre II");
            temps18mois = true;
        }


        if (!temps5ans && tempsSurvecu >= 60.0f)
        {
             if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
            GOPointer.AchievementManager.EarnAchievement("Survivre III");
            temps5ans = true;
        }


        if (!temps10ans && tempsSurvecu >= 120.0f)
        {
             if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
            GOPointer.AchievementManager.EarnAchievement("Survivre IV");
            temps10ans = true;
        }


        if (!naissance1 && naissance > 0)
        {
             if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
            GOPointer.AchievementManager.EarnAchievement("Heureux Évènement I");
            naissance1 = true;
        }


        if (!naissance2 && naissance > 1)
        {
             if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
            GOPointer.AchievementManager.EarnAchievement("Heureux Évènement II");
            naissance2 = true;
        }


        if (!nbInfos5 && nbInfos > 4)
        {
             if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
            GOPointer.AchievementManager.EarnAchievement("Connaissances en Chamois I");
            nbInfos5 = true;
        }


        if (!nbInfos10 && nbInfos > 9)
        {
             if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
            GOPointer.AchievementManager.EarnAchievement("Connaissances en Chamois II");
            nbInfos10 = true;
        }


        if (!score1000 && score > 999)
        {
             if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
            GOPointer.AchievementManager.EarnAchievement("Score Chamois I");
            score1000 = true;
        }


        if (!score3000 && score > 2999)
        {
             if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
            GOPointer.AchievementManager.EarnAchievement("Score Chamois II");
            score3000 = true;
        }


        if (!score5000 && score > 4999)
        {
             if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
            GOPointer.AchievementManager.EarnAchievement("Score Chamois III");
            score5000 = true;
        }


        if (!nourriture10 && nourritureMangee > 10)
        {
             if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
            GOPointer.AchievementManager.EarnAchievement("Alimentation I");
            nourriture10 = true;
        }


        if (!nourriture30 && nourritureMangee > 29)
        {
             if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
            GOPointer.AchievementManager.EarnAchievement("Alimentation II");
            nourriture30 = true;
        }
    }

    public void sendData()
    {
        h.Clear();
        h.Add("tps", joursSurvecus.Days);
        h.Add("nouriture", nourritureMangee);
        h.Add("score", score);
        h.Add("blessure", blessure);
        h.Add("scBouffe", scBouffe);
        h.Add("scBlessure", scBlessure);
        h.Add("scoreTps", scoreTps);
    }

    public void setData(string type, int var)
    {
        switch(type)
        {
            case "nourriture":
            nourritureMangee ++;
            score += var;
            scBouffe += var;
            break;

            case "blessure":
            blessure++;
            scBlessure += var;
            score += var;
            break;



            default:
            break;
        }
    }

    // TC création d'une fonction ToString() 
    //pour un affichage en mode txt dans le fichier de sauvegarde

    public String ToString()
    {
        String value="";

        value = value+"nourritureMangee: "+nourritureMangee+"\n";
        value +="tempsSurvecu: "+tempsSurvecu+"\n";
        value+="quete1: "+quete1+"\n";
        value+="nombrequete: "+nbQuetes+"\n"; 

        return value;
        /*
          public int nourritureMangee;
    public float tempsSurvecu;
    public int score;
    public int nbInfos;
    private int blessure;
    private int scBouffe;
    private int scBlessure;
    private int scoreTps;
    private float tempsSup;
    private float tempsInf;
    public int naissance;
    public int nourritureTotale;

    public DateTime currentTimeSave;

    public DateTime dateDeDemarrage;

    public DateTime goalDate;

    TimeSpan joursSurvecus;
    public int nbQuetes;

    private Boolean carteActive;

    public Boolean[] donneursInfosValideCham;
    //TC
    private Boolean quete1 = false;

    private Boolean temps6mois = false;
    private Boolean temps18mois = false;
    private Boolean temps5ans = false;
    private Boolean temps10ans = false;

    private Boolean naissance1 = false;
    private Boolean naissance2 = false;

    private Boolean score1000 = false;
    private Boolean score3000 = false;
    private Boolean score5000 = false;

    private Boolean nourriture10 = false;
    private Boolean nourriture30 = false;

    private Boolean nbInfos5 = false;
    private Boolean nbInfos10 = false;

    public Boolean initOnce =false;
    */

    }
}
