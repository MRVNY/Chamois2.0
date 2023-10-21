using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// classe qui stock les données pour le chasseur
///</summary>

[Serializable]
public class DSChasseur : DataStorer
{
    private Boolean carteActive;

    private Boolean quete1;

    private Boolean photo2;
    private Boolean photo5;
    private Boolean photo15;

    private Boolean abattre1;
    private Boolean abattre2;
    private Boolean abattre3;

    private Boolean score1000;
    private Boolean score3000;
    private Boolean score5000;

    private Boolean nbInfos5 = false;
    private Boolean nbInfos10 = false;

    public int nbQuetes;
    public int nbPhoto;
    public int scDechets;
    public int dechets;
    public int mauvaisChamois;
    public int bonChamois;
    public int scmauvaisChamois;
    public int scbonChamois;
    public int nbPhotoMemePartie;
    public int abattus;
    public int abattusMemePartie;
    public int score;
    public int nbInfos;

    public bool quetePhotoInit = false;
    public bool queteEtonnInit =false;

    public bool queteMepInit =false;

    public Boolean[] donneursInfosValideChass;

    public Boolean[] dechetsRamasses;

    public bool isNettoyeur;

    // Sauvegarde des photos prises.
    public Boolean chauriondePrise = false;
    
    public Boolean peclozPrise = false;

    public Boolean armenazPrise =false;

    public Boolean charbonnetPrise =false;

    public Boolean coutarsePrise = false;

    public Boolean initOnce =false;

    public int quete_en_cours = 0;

    public int killableChamois = 0;

    public int sleepableChamois = 0;

    //public string currentQuest_title ="";

    public PlayerQuest currentQuest; 

    // TC Sauvergarde de l'encyclo.
    //public EncycloContentChasseur encyChasseurData;
    
    public static DSChasseur Instance;
    
    public DSChasseur()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        
        nbQuetes = 0;

        nbPhoto = 0;

        nbPhotoMemePartie = 0;

        abattus = 0;

        abattusMemePartie = 0;

        score = 0;

        nbInfos = 0;

        scDechets = 0;

        donneursInfosValideChass =new Boolean[13];
        dechetsRamasses = new Boolean[51];

        //encyChasseurData = new EncycloContentChasseur();
    }

    // Update is called once per frame
    public void Update()
    {
        Debug.Log("DSChasseur update...nbQuetes: "+nbQuetes);
        if (!quete1 && nbQuetes >= 1)
        {
                 if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
                GOPointer.AchievementManager.EarnAchievement("Badge première quête!");
                quete1 = true;
        }

        if (!photo2 && nbPhoto > 1)
        {
                 if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
                GOPointer.AchievementManager.EarnAchievement("Chasse Photographique I");
                photo2 = true;
                // TC, il faudrait déclencher endQuest()
                Debug.Log("Je dois déclencher fin de quête photo...");
                QuestManager.Instance.endQuest();
                // Ce n'est pas une quête mais on ouvre la suite...
                QuestManager.Instance.activeZoneRecherche("ResearchArmenaz","ResearchCoutarse","ResearchCharbonnet");
                
                // Vous pouvez continuer à chercher des sites jolis...

        }

        if (!photo5 && nbPhoto > 4)
        {
                 if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
                GOPointer.AchievementManager.EarnAchievement("Chasse Photographique II");
                photo5 = true;
        }

        if (!photo15 && nbPhoto > 14)
        {
                 if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
                GOPointer.AchievementManager.EarnAchievement("Chasse Photographique III");
                photo15 = true;
        }

        if (!abattre1 && abattus > 0)
        {
                 if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
                GOPointer.AchievementManager.EarnAchievement("Prélèvement I");
                abattre1 = true;
        }

        if (!abattre2 && abattus > 1)
        {
                 if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
                GOPointer.AchievementManager.EarnAchievement("Prélèvement II");
                abattre2 = true;
        }

        if (!abattre3 && abattus > 2)
        {
                 if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
                GOPointer.AchievementManager.EarnAchievement("Prélèvement III");
                abattre3 = true;
        }

        if (!score1000 && score > 999)
        {
                 if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
                GOPointer.AchievementManager.EarnAchievement("Score Chasseur I");
                score1000 = true;
        }

        if (!score3000 && abattus > 2999)
        {
                 if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
                GOPointer.AchievementManager.EarnAchievement("Score Chasseur II");
                score3000 = true;
        }

        if (!score5000 && score > 4999)
        {
                 if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
                GOPointer.AchievementManager.EarnAchievement("Score Chasseur III");
                score5000 = true;
        }

        if (!nbInfos5 && nbInfos > 4)
        {
                 if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
                GOPointer.AchievementManager.EarnAchievement("Connaissances en Chasse I");
                nbInfos5 = true;
        }

        if (!nbInfos10 && nbInfos > 9)
        {
                 if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
                GOPointer.AchievementManager.EarnAchievement("Connaissances en Chasse II");
                nbInfos10 = true;
        }
        if ((dechets==50)) 
        {
                 if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.FogOfWarCanvas.GetComponent<AudioSource>().Play();
               
            }
                GOPointer.AchievementManager.EarnAchievement("Badge Ami de la Nature parfait!");
        }
        else
                if ((dechets>=25)) {
                     if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
            }
                    GOPointer.AchievementManager.EarnAchievement("Badge Ami de la Nature III");
                }
                else
                        if ((dechets>=10)) {
                         if (PlayerPrefs.GetInt("soundEffects") == 1)
                        {
                                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
                        }   
                        GOPointer.AchievementManager.EarnAchievement("Badge Ami de la Nature II");
                        }
                        else
                                if ((dechets>=3)) {
                                         if (PlayerPrefs.GetInt("soundEffects") == 1)
                                        {
                                                GOPointer.AchievementManager.GetComponent<AudioSource>().Play();
               
                                        }
                                        GOPointer.AchievementManager.EarnAchievement("Badge Ami de la Nature I");
                        }
    }

    public void sendData()
    {
            h.Clear();
            h.Add("Dechets", dechets);
            h.Add("scDechets", scDechets);
            h.Add("mauvaisChamois", mauvaisChamois);
            h.Add("scmauvaisChamois", scmauvaisChamois);
            h.Add("bonChamois", bonChamois);
            h.Add("scbonChamois", scbonChamois);
            h.Add("score", score);
    }
    
    public void setData(string type, int var)
    {
        switch(type)
        {
            case "scDechets":
                score += var;
                dechets++;
                scDechets += var;
                break;

            case "mauvaisChamois":
                mauvaisChamois++;
                scmauvaisChamois += var;
                score += var;
                break;
            
            case "bonChamois":
                bonChamois++;
                scbonChamois += var;
                score += var;
                break;
            
            default:
                break;
        }
    }
}
