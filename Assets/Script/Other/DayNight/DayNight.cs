using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using RPGM.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayNight : MonoBehaviour
{

    // public GameObject sun;
    // public SpriteRenderer sr;

    public TextMeshProUGUI dateText;
    public DateTime currentDate;
    public DateTime goalDate = new DateTime(2044,5,1);
    private CultureInfo french;

    public static DayNight Instance;

    //private Boolean updateDSRunning = false;
    //private Color sra;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            //currentDate = System.DateTime.Now;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        french = new CultureInfo("fr-FR");
        //TC
        //currentDate = new DateTime(2021, 12, 1);
        //Debug.Log("Je remets la date.");
        
        if (Global.Personnage == "Chamois") {
             // Si DSChamois n'a pas de sauvegarde, je sauve la date initiale
             if (DSChamois.Instance.currentTimeSave==DateTime.MinValue)
             {
                //Debug.Log("Je chope la date courante et je la met dans DSChamois");
                currentDate = System.DateTime.Now;
                DSChamois.Instance.dateDeDemarrage = currentDate;
                DSChamois.Instance.currentTimeSave = currentDate;
             }   
             else // sinon je restaure pour le chamois la dernière date sauvée.
             {
                //Debug.Log("Je remet à jour currentDate avec celle de DSChamois...");
                //Debug.Log("valeur de DSChamoisDaterecup: "+DSChamois.Instance.currentTimeSave.Date);
                currentDate= DSChamois.Instance.currentTimeSave;
             }
             // TC attention pour l'instant le timer se lance toutes les 10sec
            // mais que pour le chamois qui a des quêtes temporisées..
            StartCoroutine(timer()); 
        } 
        else // pour les autres personnages, je relance avec la date actuelle.
        {
            //Debug.Log("Je ne suis pas un chamois, je mets la date courante...s");
            currentDate = System.DateTime.Now;   
        }
                 
    }

    void Update()
    {
        // On met à jour la date régulièrement
        if (!Global.pause)
        {
            //currentDate = currentDate.Add(TimeSpan.FromMinutes(10));
            currentDate = currentDate.Add(TimeSpan.FromMinutes(0.5));
            dateText.SetText(currentDate.ToString("dd MMMM yyyy", french));
            //TC, je propage et sauvegarde la date courante pour le chamois..
             if (Global.Personnage== "Chamois")
              {
                DSChamois.Instance.currentTimeSave = currentDate;
                //Debug.Log("La current date est à:"+currentDate);
                // Je récupère la goalDate chez le chamois.
                goalDate = DSChamois.Instance.goalDate;
                //Debug.Log("La goal date est à:"+goalDate);
                //Debug.Log("Au cas où que vaut currentDate >= goalDate? : "+(currentDate>=goalDate));
              }
              
            if (goalDate != null && currentDate >= goalDate)
            {
              //TC : il ne faut faire cela que pour le chamois qui a une quête timée
              if (Global.Personnage== "Chamois")
              {
                if (DSChamois.Instance.nbQuetes==1) 
                {
                    DSChamois.Instance.tempsSurvecu=6.0f;
                } else if (DSChamois.Instance.nbQuetes==2)
                    {
                        DSChamois.Instance.tempsSurvecu=18.0f;
                    } else if (DSChamois.Instance.nbQuetes==3)
                        {
                            DSChamois.Instance.tempsSurvecu=60.0f;
                        } else if (DSChamois.Instance.nbQuetes==4)
                            {
                                DSChamois.Instance.tempsSurvecu=120.0f;
                            }                
                //DSChamois.Instance.Update();
                Debug.Log("La goal date est à:"+goalDate);
                QuestManager.Instance.endQuest();
                            Debug.Log("survie réussie!");
              }
            } 
            
        }
    }

        //TC Timer pour tester les update() de chaque personnage.
        IEnumerator timer()
        {
            while (true)
            {
                //updateDSRunning=true;
                if (Global.Personnage=="Chamois")
                {
                    DSChamois.Instance.Update();
                     //Debug.Log("J'ai lancé Update Chamois...");
                    //yield return new WaitForSeconds(10f);
                }
                // TC 
                yield return new WaitForSeconds(10f);
                //Debug.Log("Je remet à false");
                //updateDSRunning = false;
            }
            
        }


    

    // IEnumerator changerDate()
    // {
    //     while (FinPartie.Instance.fin == false)
    //     {
    //         yield return new WaitForSeconds(3.0f);
    //         jour++;
    //         nbJours++;
    //         if(Global.Personnage == "Chamois" && FinPartie.Instance.fin == false)
    //         {
    //             //GOPointer.Jauges.GetComponent<Experience>().addExperience(GOPointer.Jauges.GetComponent<Experience>().palierExp);
    //         }
    //     }
    // }

    // IEnumerator Sunset()
    // {
    //     while (sra.a > 0.0f)
    //     {
    //         if (sra.a < 0.1f)
    //         {
    //             sra.a -= 0.00005f;
    //             sr.color = sra;
    //             yield return new WaitForSeconds(0.01f);
    //         }
    //         else
    //         {
    //             sra.a -= 0.001f;
    //             sr.color = sra;
    //             yield return new WaitForSeconds(0.01f);
    //         }
    //
    //     }
    //     StartCoroutine(Sunrise());
    //     StopCoroutine(Sunset());
    // }
    //
    // IEnumerator Sunrise()
    // {
    //
    //     while (sra.a < 0.6f)
    //     {
    //         if (sra.a % 0.2f == 0)
    //         {
    //             jour += 1;
    //             nbJours += 1;
    //         }
    //         sra.a += 0.001f;
    //         sr.color = sra;
    //         yield return new WaitForSeconds(0.01f);
    //     }
    //     StartCoroutine(Sunset());
    //     StopCoroutine(Sunrise());
    // }
}