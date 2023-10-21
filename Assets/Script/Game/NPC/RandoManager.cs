using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGM.Gameplay;
using System;
using TMPro;
using System.Linq;

public class RandoManager : MonoBehaviour
{
    private DSRandonneur dataSt;
    EncycloContentRandonneur ency;


    private List<Transform> randosList;
    private InteractableController[] currentRoute;
    public static int totalPoints = -1;
    public static int currentPoint = -1;
    private string randoName;

    public static RandoManager Instance;

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
    }

    void Start()
    {

        if (Global.Personnage == "Randonneur")
        {
            dataSt = (DSRandonneur)DataStorer.currentDS;
        }

        //dataEncy = JObject.Parse(jsonFileEncy.text);
        ency = GOPointer.EncyclopedieManager.GetComponent<EncycloContentRandonneur>();

        randosList = new List<Transform>();
        var tmp = GetComponentsInChildren<Transform>();
        foreach (Transform t in tmp)
        {
            if (t.parent == transform)
            {
                randosList.Add(t);
            }
        }
        
        foreach (var rando in randosList)
        {
            rando.gameObject.SetActive(false);
        }
    }

    public void startRando(string rando){
        randoName = rando;
        //ency.addInfoToList("rando"+rando, ency.quete);

        int i = Global.randoNum[rando]-1;
        randosList[i].gameObject.SetActive(true);
        currentRoute = randosList[i].GetComponentsInChildren<InteractableController>();

        foreach (InteractableController ic in currentRoute)
        {
            //ic.setMessage("Oops, looks like you took a shortcut and skipped the last point, please validate the last point and come back");
            ic.setMessage("Oops, il me semble que t'as sauté des points de contrôle, valide tous les points précédents et reviens ici.");
        }
        
        //currentRoute[0].setMessage("Hurray! You've found the starting point! The nexr point is nearby!");
        currentRoute[0].setMessage("Youpi! Tu as trouvé le point de départ! Le point suivant doit être proche!");
        totalPoints = currentRoute.Length;
        currentPoint = -1;

        // à ne pas faire si reprise de rando déjà lancée:
         if (DSRandonneur.Instance.randoLancee=="")
        {
            QuestManager.Instance.addQuest("rando"+rando, totalPoints);
            QuestManager.Instance.currentQuest.currentStep = currentPoint;

            DSRandonneur.Instance.randoLancee=rando;
         }
    }

       
        

    public void nextRando(InteractableController point){
        if(currentRoute[currentPoint+1]==point){
            currentPoint++;
            QuestManager.Instance.currentQuest.currentStep = currentPoint;
            if(currentPoint==0) ency.addInfoToList("start"+randoName, ency.pagesDynamic);
            currentRoute[currentPoint].setMessage("Tu as déjà validé ce point, cherche le point suivant!");
            if(currentPoint == totalPoints-2){
                
                //TC Gestion des randos déjà faites
                if (DSRandonneur.Instance.randoFaites[Global.randoNum[randoName]-1]==true)
                {
                    currentRoute[currentPoint+1].setMessage("Bravo! Tu as atteint la fin de la randonnée mais tu l'avais déjà faite donc tu n'obtiendras pas de points supplémentaires!");
                }
                else {
                        
                    currentRoute[currentPoint+1].setMessage("Bravo! Tu as atteint la fin de la randonnée!");
                    Debug.Log("randodif: currentPoint+1: "+currentPoint+1);
                    if (currentPoint+1>=5) {DSRandonneur.Instance.randoDif=true;}
                }
               
            }
            else if (currentPoint >= totalPoints - 1)
            {
                     if (PlayerPrefs.GetInt("soundEffects") == 1)
                        {
                            GOPointer.InfoRando.GetComponent<AudioSource>().Play();               
                        }
                endRando();
            }
            else{
                 if (PlayerPrefs.GetInt("soundEffects") == 1)
                        {
                            GOPointer.RandonneurUI.GetComponent<AudioSource>().Play();               
                        }
                currentRoute[currentPoint+1].setMessage("Bravo! Tu as trouvé le point de contrôle numéro "+(currentPoint+2)+"!");
            }
        }
    }

    public void endRando(){
        ency.addInfoToList("end"+randoName, ency.pagesDynamic);
        totalPoints = -1;
        currentPoint = -1;
        
        QuestManager.Instance.currentQuest.isFinished= true;
        
        dataSt.setData(randoName+"Score", 1);
        //dataSt.nbRandos += 1;
        if (DSRandonneur.Instance.randoFaites[Global.randoNum[randoName]-1]==false)
        {
            DSRandonneur.Instance.nbRandos+= 1;
            //Debug.Log("nbRandos du dataSt: "+dataSt.nbRandos);
            TextMeshProUGUI[] texte =  GOPointer.InfoRando.GetComponentsInChildren<TextMeshProUGUI>(); 
            texte[0].SetText("Randonnées effectuées : \n{0} / 11", DSRandonneur.Instance.nbRandos);
            //texte[0].SetText("Randonnées effectuées : \n{0} / 11", dataSt.nbRandos);
        }
        DSRandonneur.Instance.randoJustFinished=true;
        DSRandonneur.Instance.randoLancee="";
        DSRandonneur.Instance.randoFaites[Global.randoNum[randoName]-1]=true;
        DSRandonneur.Instance.Update();
        // dataSt.nbRandosMemePartie += 1;
    }

}