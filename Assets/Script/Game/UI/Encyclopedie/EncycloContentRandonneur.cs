using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Linq;
using TMPro;

public class EncycloContentRandonneur : Encyclopedie
{
    new void Start()
    {
        base.Start();
    }

    public void addInfoToList(string action, List<ContenuPages> liste)
    {
        // Debug.Log("Que vaut liste dans EncycloRando? "+liste);
        base.addInfoToList(action, liste, dynamicInfo);
    }

    public void initList()
    {
        dynamicInfo = new Dictionary<string, EncycloInfos>();
        staticInfo = new List<EncycloInfos>();

        // TC
         pagesDynamic = new List<ContenuPages>();

        // Récupération des données dans le JSON, lié dans le GameObject "Encyclopédie Manager"
        JObject objs = (JObject)JObject.Parse(jsonFile.text)["Rando"];

        foreach (JProperty obj in objs.OfType<JProperty>())
        {
            dynamicInfo.Add(obj.Name, new EncycloInfos(null, (string)obj.Value, ((string)obj.Value).Length/50+1));
        }


        dynamicInfo.Add("hautFait", new EncycloInfos(null, "Vous venez de gagner un haut-fait. Les hauts-faits sont des objectifs secondaires de jeu vous aidant à découvrir tous les aspects du jeu. Les hauts-faits vous octroient des points, composant votre score de découverte du jeu.", 6));

        //TC Test d'ajout dyn en direct (à mettre en JSON après)
        // ok mis en JSON ENcy
        //dynamicInfo.Add("pavot", new EncycloInfos(null, "Vous venez de découvrir une plante dangereuse. C'est le coquelicot, elle contient des subtances psychotropes...", 6));


        staticInfo.Add(new EncycloInfos(null, "En tant que randonneur, votre objectif principal est de découvrir l'environnement qui vous entoure. Vous pouvez aussi rechercher des randonnées que vous pouvez effectuer afin de vous donner du challenge dans votre aventure...", 6));
        staticInfo.Add(new EncycloInfos(null, "Cependant, essayez de découvrir en étant le moins néfaste possible pour votre environnement, afin d'effectuer la meilleure performance possible en tant que randonneur.", 6));


        base.Start();
        setPageStatic(staticInfo);
        if (DSRandonneur.Instance.initOnce) {
            // Il faut récupérer la partie dynamique de l'encyclo...
            //if(GOPointer.currentEncy.pagesDynamic==null) GOPointer.Instance.Link();
            //pagesDynamic= GOPointer.currentEncy.pagesDynamic;
            var tmpEncy = SaveLoad.Load<List<ContenuPages>>("Ency"+Global.Personnage);
            pagesDynamic= tmpEncy;
            Debug.Log("Je suis dans EncycloRando et j'ai nbInfos à: "+DSRandonneur.Instance.nbInfos);
            // on revient sur le Randonneur: 
            TextMeshProUGUI[] texte =  GOPointer.InfoRNCFS.GetComponentsInChildren<TextMeshProUGUI>(); 
            texte[0].SetText("Connaissances RNCFS : \n{0} / 11", DSRandonneur.Instance.nbInfos);  
            TextMeshProUGUI[] texte2 =  GOPointer.InfoRando.GetComponentsInChildren<TextMeshProUGUI>(); 
            texte2[0].SetText("Randonnées effectuées : \n{0} / 11", DSRandonneur.Instance.nbRandos); 
            
            // Y avait-il une rando en cours?
            if (DSRandonneur.Instance.randoLancee!="")
            {
                RandoManager.Instance.startRando(DSRandonneur.Instance.randoLancee);
                Debug.Log("La rando lancée est: "+DSRandonneur.Instance.randoLancee);
            }            
            
            //Global.randoNum[randoName]-1
            //Debug.Log("pagesDynamic taille:"+pagesDynamic.Count());
            //setPageDynamic(pagesDynamic);
            //pagesDynamic= GOPointer.currentEncy.pagesDynamic;
        
        }else DSRandonneur.Instance.initOnce=true;
    }
}
