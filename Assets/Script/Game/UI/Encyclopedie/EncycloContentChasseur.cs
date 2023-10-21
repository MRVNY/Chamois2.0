﻿ using System;
 using UnityEngine;
 using System.Collections.Generic;
using System.Collections;
 using Newtonsoft.Json.Linq;
 using System.Linq;

//TC
//[Serializable]
public class EncycloContentChasseur : Encyclopedie
{

    /*public GameObject notifToEnable;
    public bool notifEnabled = false;*/

    new void Start()
    {
        base.Start();
    }

    /*void Update()
    {
        if (notifEnabled)
        {
            notifToEnable.SetActive(true);
        }
        else
        {
            notifToEnable.SetActive(false); 
        }
    }

    public void setTrue()
    {
        notifEnabled = true;
    }

    public void setFalse()
    {
        notifEnabled = false;
    }*/
    
    public void addInfoToList(string action, List<ContenuPages> liste)
    {
        base.addInfoToList(action, liste, dynamicInfo);
    }

    public void initList()
    {
        Debug.Log("J initList dans encycloChasseur");
        dynamicInfo  = new Dictionary<string, EncycloInfos>();
        staticInfo   = new List<EncycloInfos>();
        pagesDynamic = new List<ContenuPages>();
        quete = pagesDynamic;
            


        string path = "Visuals/UI/Ency/Chasseur/";

        //string pathPecloz = "Visuals/UI/Ency/Chasseur/chassePhotographique/montpecloz/";
        //string pathChaurionde = "Visuals/UI/Ency/Chasseur/chassePhotographique/montpecloz/";
        string pathPhoto = "Visuals/UI/Ency/Chasseur/chassePhotographique/";


        //dynamicInfo.Add("photoChaurionde", new EncycloInfos(pathChaurionde + "Pointechaurionde", null, 6));
        dynamicInfo.Add("photoChaurionde", new EncycloInfos(pathPhoto + "Pointechaurionde", "        Pointe de Chaurionde", 6));
        dynamicInfo.Add("photoPecloz", new EncycloInfos(pathPhoto + "Montpecloz", "       Le Mont Pecloz", 6));
        dynamicInfo.Add("photoArmenaz", new EncycloInfos(pathPhoto + "Armenaz", "Depuis l'Armenaz", 6));
        dynamicInfo.Add("photoCoutarse", new EncycloInfos(pathPhoto + "Coutarse", "ZR de Coutarse", 6));
        dynamicInfo.Add("photoCharbonnet", new EncycloInfos(pathPhoto + "Charbonnet", "Depuis le Charbonnet", 6));


        dynamicInfo.Add("mange", new EncycloInfos(null, "Manger c'est bon pour la santé",1));
        dynamicInfo.Add("cours", new EncycloInfos("","Courir c'est cool pour avancer", 5));
        dynamicInfo.Add("touche", new EncycloInfos(null,"Toucher des truc c'est dangereux", 1));
        dynamicInfo.Add("hautFait", new EncycloInfos(null, "Vous venez de gagner un haut-fait. Les haut-faits sont des objectifs secondaires de jeu vous aidant à découvrir tous les aspects du jeu. Les haut-faits vous octroient des points, composant votre score de découverte du jeu.", 6));

        // Récupération des données dans le JSON, lié dans le GameObject "Encyclopédie Manager"
        JObject objs = (JObject)JObject.Parse(jsonFile.text)["Chasseur"];

        foreach (JProperty obj in objs.OfType<JProperty>())
        {
            dynamicInfo.Add(obj.Name, new EncycloInfos(null, (string)obj.Value, ((string)obj.Value).Length/50+1));
        }

        // infos pour la quete du chasseur
        /*dynamicInfo.Add("donneurQuete", new EncycloInfos(null, "Vous devez trouver un garde forestier a l'est de la ville...", 3));
        dynamicInfo.Add("gardeForestier", new EncycloInfos(null, "Vous devez trouver un randonneur, ce dernier devrait pouvoir vous en apprendre plus...", 4));
        dynamicInfo.Add("randonneur", new EncycloInfos(null, "L'animal pourrait devenir un danger pour les autres, vous devez la trouver. Vous devriez investiguer dans le massif a votre nord...", 5));
        dynamicInfo.Add("photographe", new EncycloInfos(null, "Votre proie se trouve à la Tour de l'angle Est, vous devriez probablement vous y rendre...", 4));*/

        // infos diverses
        // utilisées
        /*dynamicInfo.Add("poidsChevreuil",           new EncycloInfos(null, "Chez les chevreuils, la femelle pèse environ 24kg, alors qu'un mâle pèse environ 26kg", 4));
        dynamicInfo.Add("predateursPresents",       new EncycloInfos(null, "Dans le Massif des Bauges, vous pouvez trouver des prédateurs, comme des loups, lynxs, vautours, renards ou des aigles.", 6));
        dynamicInfo.Add("periodeChasse",            new EncycloInfos(null, "La période de chasse du chamois est de septembre à février, il existe une période d'interdiction fin novemmbre, pendant la période de rut.", 6));
        dynamicInfo.Add("regulationChasseChamois",  new EncycloInfos(null, "Un quota de chamois prélevable est défini par communes.", 3));
        dynamicInfo.Add("habitatChamois",           new EncycloInfos(null, "Les chamois ont pour habitude de vivre dans les alpages", 3));
        dynamicInfo.Add("sociabiliteChamois",       new EncycloInfos(null, "En termes de sociabilité, le chamois a pour habitude d'être social", 4));
        dynamicInfo.Add("regimeChamois",            new EncycloInfos(null, "Pour se nourrir, le chamois va cueillir ou bien brouter sa nourriture", 4));
        dynamicInfo.Add("poidsChamois",             new EncycloInfos(null, "Chez les chamois, la femelle pèse environ 26kg, alors qu'un mâle pèse environ 39kg", 4));
        dynamicInfo.Add("faunePresente",            new EncycloInfos(null, "Dans le Massif des Bauges, vous pouvez trouver sangliers, cerfs, mouflons, lagopèdes, tétras lyre, lièvres, chevreuils et chamois.", 6));
        dynamicInfo.Add("chasseurTelemetre",        new EncycloInfos(null, "Pour chasser, le chasseur utilise un télémètre, afin d'estimer la distance à laquelle se trouve une cible, et d'adapter son matériel en fonction.", 6));
        dynamicInfo.Add("utiliteChasse",            new EncycloInfos(null, "La chasse est un loisir, qui n'a pas que pour but de chasser pour manger, elle a aussi pour but de réguler les espèces.", 6));
        dynamicInfo.Add("loupRegulation",           new EncycloInfos(null, "Le loup, comme le chasseur, permet la régulation des populations de gibier.", 4));

        // non utilisées
        dynamicInfo.Add("habitatChevreuil",         new EncycloInfos(null, "Les chevreuils ont pour habitude de vivre dans les forêts", 3));
        dynamicInfo.Add("sociabiliteChevreuil",     new EncycloInfos(null, "En termes de sociabilité, le chevreuil a pour habitude d'être de solitaire à social", 4));
        dynamicInfo.Add("regimeChevreuil",          new EncycloInfos(null, "Pour se nourrir, le chevreuil va cueillir sa nourriture", 3));

        dynamicInfo.Add("poidsMouflon",         new EncycloInfos(null, "Chez les mouflons, la femelle pèse environ 30kg, alors qu'un mâle pèse environ 43kg", 4));
        dynamicInfo.Add("habitatMouflon",       new EncycloInfos(null, "Les mouflons ont pour habitude de vivre dans les alpages et les forêts", 4));
        dynamicInfo.Add("sociabiliteMouflon",   new EncycloInfos(null, "En termes de sociabilité, le mouflon a pour habitude d'être social", 4));
        dynamicInfo.Add("regimeMouflon",        new EncycloInfos(null, "Pour se nourrir, le mouflon va brouter sa nourriture", 3));
        
        dynamicInfo.Add("domaines",                 new EncycloInfos(null, "Les animaux possèdent des domaines. L'usage de ces derniers par l'homme va perturber la faune", 4));
        dynamicInfo.Add("chamoisRaces",             new EncycloInfos(null, "Il n'y a pas de races de chamois, pour les différencier, il existe des différences de taille entre mâles et femelles, ainsi que des tailles de cornes en fonction de l'âge.", 6));
        dynamicInfo.Add("presencePredateur",        new EncycloInfos(null, "La présence de prédateurs a des effets comportementaux sur la faune.", 4));
        dynamicInfo.Add("chasseurChien",            new EncycloInfos(null, "Un chasseur doit dresser ses chiens à chasser un gibier précis afin d'éviter que ce dernier ne court après tout et n'importe quoi.", 6));
        dynamicInfo.Add("horsChasse",               new EncycloInfos(null, "En période hors chasse, le chasseur peut faire du comptage ou encore de la chasse photographique.", 4));
        dynamicInfo.Add("typesChasse",              new EncycloInfos(null, "Il existe plusieurs types de chasse, comme par exemple la chasse en battue, ou la chasse en billebaude.", 5));
        dynamicInfo.Add("chasseurJumelles",         new EncycloInfos(null, "Pour chasser, le chasseur utilise des jumelles, afin d'identifier les animaux.", 6));*/
        
        staticInfo.Add(new EncycloInfos(null, "En tant que chasseur, votre objectif principal est d'aider à la régulation des espèces. N'hésitez pas à converser avec tous les perosnnages afin d'obtenir des informations ou d'obtenir des quêtes de chasse.", 6));

        staticInfo.Add( new EncycloInfos(null, "Votre rôle est de chasser, de traquer et de prélever des cibles potentiellement dangereuses et prendre soin du parc des Bauges.", 4));
        staticInfo.Add( new EncycloInfos(null, "Attention à ne pas vous tromper de cible! cela pourrait avoir des conséquences désastreuses....", 4));
        staticInfo.Add( new EncycloInfos(path + "a", "Chamois dans les Bauges", 1));
        base.Start();
        setPageStatic(staticInfo);
        if (DSChasseur.Instance.initOnce) {
            // Cas où on revient d'un autre rôle:
            // Il faut récupérer la partie dynamique de l'encyclo...
            var tmpEncy = SaveLoad.Load<List<ContenuPages>>("Ency"+Global.Personnage);
            pagesDynamic= tmpEncy;
            // quete_en_cours=0 pas de quête,1 (kill),2(photo),3(bizarre),4(agressif)
            // Si une quête localisée est en cours, il faut remettre à jour les "targets".
            // quête 1 kill chamois 0 non déclenchée, -1 finie...
            switch(DSChasseur.Instance.quete_en_cours)

                {
                    case 1: { if (DSChasseur.Instance.killableChamois>0) {
                        var listDeChamois = ListProie.Instance.listDeProie;
                        var i = DSChasseur.Instance.killableChamois;
                        Debug.Log("i récupéré (ex au sort):"+i);
                        QuestManager.Instance.target = listDeChamois[i].gameObject;
                        listDeChamois[i].GetComponent<SpriteRenderer>().color  = Color.red;
                        QuestManager.Instance.oldTag = QuestManager.Instance.target.tag;
                        QuestManager.Instance.target.tag = "Target";
                        Debug.Log("Il est rouge et est une target"+i);
                         }  
                        break;     
                    }                   
                    case 2: {
                        if (!DSChasseur.Instance.peclozPrise) {
                             QuestManager.Instance.target = ((Collider2D)ZoneManager.Instance.zones["zonePecloz"])?.gameObject;
                            if(QuestManager.Instance.target != null)
                            {
                                QuestManager.Instance.oldTag = QuestManager.Instance.target.tag;
                                QuestManager.Instance.target.tag = "PhotoSite";
                            }
                        }
                        if (!DSChasseur.Instance.chauriondePrise) {
                            QuestManager.Instance.target = ((Collider2D)ZoneManager.Instance.zones["zoneChaurionde"])?.gameObject;
                            if(QuestManager.Instance.target != null)
                            {
                                QuestManager.Instance.oldTag = QuestManager.Instance.target.tag;
                                QuestManager.Instance.target.tag = "PhotoSite";
                            }
                        }
                        break;
                    }                    
                    case 3: { 
                            // Il faudrait aussi réactiver les sites dont les photos n'ont pas encore été prises
                            RemisePhotoSite();
                            // Je dois remettre le chamois blessé s'il n'a pas été encore trouvé
                            
                             var listDeChamoisBizarre = ListProie.Instance.listWeakChamois;
                             var i = DSChasseur.Instance.sleepableChamois;
        
                            //Debug.Log("i Bizarre tiré au sort:"+i);
                            QuestManager.Instance.target = listDeChamoisBizarre[i].gameObject;                            
                            QuestManager.Instance.target.SetActive(true);
                            QuestManager.Instance.oldTag = QuestManager.Instance.target.tag;
                            QuestManager.Instance.target.tag = "Target";
                            Debug.Log("Je viens de remettre le chamois étonnant...");
                        break;
                    }                    
                    case 4: { 
                                // Il faudrait aussi réactiver les sites dont les photos n'ont pas encore été prises
                                RemisePhotoSite();
                            // Je dois remettre le chamois agressif s'il n'a pas été encore trouvé
                              var listDeChamoisBizarre = ListProie.Instance.listWeakChamois;
                             var i = DSChasseur.Instance.sleepableChamois;       
                            //Debug.Log("i Bizarre tiré au sort:"+i);
                            QuestManager.Instance.target = listDeChamoisBizarre[i].gameObject;
                            listDeChamoisBizarre[i].GetComponent<SpriteRenderer>().color  = new Color(51,25,0,1);
                            QuestManager.Instance.target.SetActive(true);
                            QuestManager.Instance.oldTag = QuestManager.Instance.target.tag;
                            QuestManager.Instance.target.tag = "Target";
                            Debug.Log("Je viens de remettre le chamois mal en point...");
                        break;
                    }                                       
                }
                // Il faudrait aussi réactiver les sites dont les photos n'ont pas encore été prises
                
           
            // quête 2 photo: quetephotoChaurionde==true 
            //setPageDynamic(pagesDynamic);
            //pagesDynamic= GOPointer.currentEncy.pagesDynamic;
        
        }else DSChasseur.Instance.initOnce=true;

    }

    public void RemisePhotoSite()
    {
                    if (!DSChasseur.Instance.armenazPrise) {
                          QuestManager.Instance.target = ((Collider2D)ZoneManager.Instance.zones["ResearchArmenaz"])?.gameObject;
                        if(QuestManager.Instance.target != null)
                        {
                        QuestManager.Instance.oldTag = QuestManager.Instance.target.tag;
                        QuestManager.Instance.target.tag = "PhotoSiteSupp";
                        }
                    }
                    if (!DSChasseur.Instance.charbonnetPrise) {
                        QuestManager.Instance.target = ((Collider2D)ZoneManager.Instance.zones["ResearchCharbonnet"])?.gameObject;
                        if(QuestManager.Instance.target != null)
                        {
                        QuestManager.Instance.oldTag = QuestManager.Instance.target.tag;
                        QuestManager.Instance.target.tag = "PhotoSiteSupp";
                        }
                    }
                    if (!DSChasseur.Instance.coutarsePrise) {
                        QuestManager.Instance.target = ((Collider2D)ZoneManager.Instance.zones["ResearchCoutarse"])?.gameObject;
                        if(QuestManager.Instance.target != null)
                        {
                        QuestManager.Instance.oldTag = QuestManager.Instance.target.tag;
                        QuestManager.Instance.target.tag = "PhotoSiteSupp";
                        }
                    }
    }
    
}
