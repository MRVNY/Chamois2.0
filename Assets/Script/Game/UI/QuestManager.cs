using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using RPGM.Gameplay;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class QuestManager : MonoBehaviour
{
    public TextAsset jsonFile;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public TextMeshProUGUI hint;
    public TextMeshProUGUI finished;

    public GameObject left;
    public GameObject right;
    private int onPage = 0;

    private Hashtable allQuests = new Hashtable();
    [NonSerialized] public List<PlayerQuest> foundQuests = new List<PlayerQuest>();
    private PlayerQuest empty;
    [NonSerialized] public PlayerQuest currentQuest;

    public GameObject target;
    public string oldTag;

    public string oldTTag;
    
    public static QuestManager Instance;
    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        if (foundQuests.Count == 0)
        {
            JObject objs = (JObject)JObject.Parse(jsonFile.text)[Global.Personnage];

            foreach (JProperty obj in objs.OfType<JProperty>())
            {
                PlayerQuest tmp = new PlayerQuest(obj.Name, obj.Value["description"]?.ToString());
                
                if (obj.Value["hints"] != null)
                {
                    foreach (JProperty hint in obj.Value["hints"])
                    {
                        tmp.hints.Add(hint.Name, hint.Value.ToString());
                    }
                }

                tmp.participants = ((string)obj.Value["participants"])?.Split(',');

                tmp.nextQuest = obj.Value["nextQuest"]?.ToString();

                allQuests.Add(obj.Name, tmp);
            }
            
            // Initialisation avec une quête "vide"
            empty = new PlayerQuest("Pas de quête en cours", "Baladez-vous dans les Bauges pour en touver !");
            empty.isFinished = true;
            
            foundQuests.Insert(0,empty);
            //Debug.Log("J'ai initialisé la non-quête...");
            Notifier.Instance.NewQuest();
        }
    }

    private void OnEnable()
    {
        if(foundQuests.Count==0)
        {
            Start();
        }
        
        currentQuest = foundQuests[0];
        onPage = 0;
        LoadPage(currentQuest);
    }

    public void addQuest(string title)
    {
        addQuest(title,0);
         
    }
    
    public void addQuest(string title, int steps)
    {
        if (foundQuests.Contains(empty))
        {
            foundQuests.Remove(empty);
            if (Global.Personnage=="Chasseur")
            {
                // Je mets à jour nbQuetes...
                DSChasseur.Instance.nbQuetes++;
                DSChasseur.Instance.Update();
            }
            
            //GOPointer.AchievementManager.EarnAchievement("Badge première quête!");
        }
        // Avec le chamois, on active directe la première quête donc on démarre à -1.
        if (Global.Personnage=="Chamois")
            {
                // Je mets à jour nbQuetes...
                DSChamois.Instance.nbQuetes++;
                //Debug.Log("nbQuetes Chamois: "+DSChamois.Instance.nbQuetes);
                DSChamois.Instance.Update();
            }


        if (currentQuest.isFinished)
        {
            if (allQuests.ContainsKey(title))
            {
                PlayerQuest tmp = (PlayerQuest)allQuests[title];
                tmp.totalSteps = steps;
                foundQuests.Insert(0,tmp);
                currentQuest = tmp;
                // Dans le cas du chasseur, on doit sauver la quête en cours au
                // cas où on changerait entre temps de personnage. 
                if (Global.Personnage== "Chasseur")
                {
                    DSChasseur.Instance.currentQuest = currentQuest;
                }
            }
        
            Notifier.Instance.NewQuest();

            switch (title)
            {
                case "Un Chamois Malade":
                    DSChasseur.Instance.currentQuest= currentQuest;
                    startKillQuest();
                    break;
                case "Retrouver votre troupeau":
                    DSChamois.Instance.currentQuest= currentQuest;
                    startZoneQuest("Troupeau");
                    break;
                case "Survivre":
                    //startTimeQuest(new DateTime(2024,5,1));
                    DSChamois.Instance.currentQuest= currentQuest;
                    startTimeQuest(DateTime.Now.AddDays(180));
                    //startTimeQuest(DateTime.Now.AddDays(10));
                    break;
                case "Survivre2":
                    //startTimeQuest(new DateTime(2024,5,1));
                    DSChamois.Instance.currentQuest= currentQuest;
                    startTimeQuest(DateTime.Now.AddDays(365));
                    //startTimeQuest(DateTime.Now.AddDays(36));
                    break;
                case "Survivre3":
                    //startTimeQuest(new DateTime(2024,5,1));
                    DSChamois.Instance.currentQuest= currentQuest;
                    startTimeQuest(DateTime.Now.AddDays(1280));
                    //startTimeQuest(DateTime.Now.AddDays(36));
                    break;
                case "Survivre4":
                    //startTimeQuest(new DateTime(2024,5,1));
                    DSChamois.Instance.currentQuest= currentQuest;
                    startTimeQuest(DateTime.Now.AddDays(2560));
                    //startTimeQuest(DateTime.Now.AddDays(36));
                    break;
            }
        }
        else
        {
            GOPointer.CanvasGuideJeu.SetActive(true);
            GuideManager.Instance.guideText.SetText("Vous avez déjà une quête en cours !");
        }
        
    }

    public void LoadPage(PlayerQuest quest)
    {
        title.text = quest.title;
        description.text = quest.desc;
        finished.text = quest!=empty ? (quest.isFinished ? "Finie" : "En cours") : "";
        
        hint.text = quest.hints.ContainsKey(quest.hintName) ? quest.hints[quest.hintName].ToString() : "";
        if (quest.totalSteps > 0)
        {
            hint.text = "Points validés : " + (quest.currentStep+1) + "/" + quest.totalSteps;
        }
        
        // J'active les boutons de page gauche et droite si on a d'autres quêtes.
        left.SetActive(onPage > 0);
        right.SetActive(onPage < foundQuests.Count - 1);
    }

    public void NextPage()
    {
        //TC
        /*
        if(onPage > 0)
        {
            onPage--;
            LoadPage(foundQuests[onPage]);
        }
        */
        
        if(onPage < foundQuests.Count - 1)
        {
            onPage++;
            LoadPage(foundQuests[onPage]);
        }
        
    }
    
    public void PreviousPage()
    {
        //TC
        /*
         if(onPage < foundQuests.Count - 1)
        {
            onPage++;
            LoadPage(foundQuests[onPage]);
        }
        */
        
        if(onPage > 0)
        {
            onPage--;
            LoadPage(foundQuests[onPage]);
        }
    }

    // Départ de la quête 1 du chasseur
    public void startKillQuest()
    {
        // TODO en attente...
        DSChasseur.Instance.quete_en_cours=1;
        //Debug.Log("Je lance la quête du chasseur: kill chamois");
        //var listDeChamois = GOPointer.ListeChamoisSauvages.GetComponentsInChildren<SpriteRenderer>(false);
        var listDeChamois = ListProie.Instance.listDeProie;
        //var listDeChamois = GOPointer.ChamoisPrey.ListProie.GetComponentsInChildren<SpriteRenderer>(false);
        
        var i = Random.Range(0, listDeChamois.Count);
        DSChasseur.Instance.killableChamois=i;
        //Debug.Log("i tiré au sort:"+i);
        target = listDeChamois[i].gameObject;
        //listDeProie[i].color = Color.red;
        listDeChamois[i].GetComponent<SpriteRenderer>().color  = Color.red;
        oldTag = target.tag;
        target.tag = "Target";
        //startPhotoQuest("zonePecloz","zoneChaurionde");
    }
    
    public void endKillQuest()
    {
        target.GetComponent<SpriteRenderer>().color = Color.white;
        //Debug.Log("Je termine la quête du chasseur: kill chamois");
        DSChasseur.Instance.abattus+=1;
        DSChasseur.Instance.Update();
        //GOPointer.AchievementManager.EarnAchievement("Prélèvement I");
        
        if (currentQuest.title!=DSChasseur.Instance.currentQuest.title)
        {
            currentQuest=DSChasseur.Instance.currentQuest;
        }


        endQuest();
        DSChasseur.Instance.quete_en_cours++;
        //startPhotoQuest("zonePecloz","zoneChaurionde");
    }

    // Départ de la quête 2 du chasseur.
    public void startPhotoQuest(string zoneName1, string zoneName2)
    {
       // GOPointer.PhotoQuestManager.SetActive(true); 
       //Debug.Log("Je lance startPhotoQuest avec: "+zoneName1);
        if (Global.Personnage=="Chasseur")
        {
            target = ((Collider2D)ZoneManager.Instance.zones[zoneName1])?.gameObject;
            if(target != null)
            {
                oldTag = target.tag;
                target.tag = "PhotoSite";
            }
            // je rajoute
            //Debug.Log("Je lance startPhotoQuest avec: "+zoneName2);
             target = ((Collider2D)ZoneManager.Instance.zones[zoneName2])?.gameObject;
            if(target != null)
            {
                oldTag = target.tag;
                target.tag = "PhotoSite";
            }
        }
        
    }

    // Départ de la quête III du Chasseur
    public void startSleepQuest()
    {
        // TC prévoir 3 lieux aléatoires
         var listDeChamoisBizarre = ListProie.Instance.listWeakChamois;
        //var listDeChamois = GOPointer.ChamoisPrey.ListProie.GetComponentsInChildren<SpriteRenderer>(false);
        
        var i = Random.Range(0, listDeChamoisBizarre.Count);
        while (i==DSChasseur.Instance.sleepableChamois)
        {
            i = Random.Range(0, listDeChamoisBizarre.Count);
        } 
        DSChasseur.Instance.sleepableChamois=i;
        //Debug.Log("i Bizarre tiré au sort:"+i);
        target = listDeChamoisBizarre[i].gameObject;
        //listDeProie[i].color = Color.red;
        if (currentQuest.title=="Un Chamois mal en point") {listDeChamoisBizarre[i].GetComponent<SpriteRenderer>().color  = new Color(51,25,0,1);}
        target.SetActive(true);
        oldTag = target.tag;
        target.tag = "Target";
    }

    // Départ d'une quête de zone (Quête Chamois I par exemple)
    public void startZoneQuest(string zoneName)
    {   
            // pour le chamois
            target = ((Collider2D)ZoneManager.Instance.zones[zoneName])?.gameObject;
            if(target != null)
            {
                oldTag = target.tag;
                target.tag = "Target";
            }
        
    }


    // Quête temporisée (Chamois 2 par exemple)
    void startTimeQuest(DateTime endDate)
    {
        // Cas pour le chamois, sinon à chaque chgt de perso, on perd la quête en cours
        if (Global.Personnage == "Chamois")
        {
            DSChamois.Instance.goalDate = endDate;
        }
        DayNight.Instance.goalDate = endDate;
    }

    public void endQuest()
    {
        if ((Global.Personnage== "Chamois") &&  (DSChamois.Instance.nbQuetes==0)) 
        {
           target=((Collider2D)ZoneManager.Instance.zones["Troupeau"])?.gameObject;
           //Debug.Log("Je remets à Untagged");
           target.tag="Untagged";
        } 
        else
        if (target!=null) target.tag = oldTag;
        target = null;

        // En cas de chgt entre temps de joueur, il faut récupérer la bonne quête.
        if ((Global.Personnage== "Chamois") && (currentQuest.title!=DSChamois.Instance.currentQuest.title))
        {
            currentQuest= DSChamois.Instance.currentQuest;
        }
         if ((Global.Personnage== "Chasseur") && (currentQuest.title!=DSChasseur.Instance.currentQuest.title))
        {
            currentQuest= DSChasseur.Instance.currentQuest;
        }

        //Debug.Log("la currentQuest est: "+currentQuest.title);
        currentQuest.isFinished = true;
        //Debug.Log("TODO Juste pour mon 1er endQuest()...");
        
        var oldQuest = currentQuest;
        //if (oldQuest.title=="Un Chamois Malade") {oldQuest.title="Un chamois malade...";}

        if (currentQuest.participants!=null)
        {
            foreach (var npc in currentQuest.participants)
            {
                ((NPCController)NPCManager.Instance.currentNPCTable[npc]).setFirstNode("");
            }
        }
        

        if(currentQuest.nextQuest!=null)
        {
            addQuest(currentQuest.nextQuest);
            //Debug.Log("Quete finie, J'addQuest la suivante...");
        }
         if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.GameManager.GetComponent<AudioSource>().Play();
               
            }
        GOPointer.CanvasGuideJeu.SetActive(true);
        GuideManager.Instance.guideText.SetText("Bravo, vous avez fini la quête: \n"+oldQuest.title+" !");

        Notifier.Instance.NewQuest();
        
        // TODO Détection de la fin du jeu pour chaque joueur (= l'ensemble des quêtes dispo est réussi)
        // fin du jeu non détectée pour le chasseur
        //Debug.Log("valeur de foundQuests(Empty): "+foundQuests.Contains(empty));
        //Debug.Log("valeur de foundQuests.Count: "+foundQuests.Count);
        //Debug.Log("valeur de allQuests.Count: "+allQuests.Count);
        //Debug.Log("currentQuest.isFinished? : "+currentQuest.isFinished);
        if(!foundQuests.Contains(empty) && foundQuests.Count==allQuests.Count && currentQuest.isFinished)
        {
           //Debug.Log("Nombre de Quests total: "+allQuests.Count+"Attention, je détecte la fin de toutes les quêtes...");
           GameOver.Instance.End("Bravo, vous avez fini toutes les quêtes du "+Global.Personnage+"!");
        }
    }

    public void activeZoneRecherche(string zoneName1, string zoneName2, string zoneName3)
    {
               if (Global.Personnage=="Chasseur")
        {
            target = ((Collider2D)ZoneManager.Instance.zones[zoneName1])?.gameObject;
            if(target != null)
            {
                oldTag = target.tag;
                target.tag = "PhotoSiteSupp";
            }
            // je rajoute
            //Debug.Log("Je lance startPhotoQuest avec: "+zoneName2);
             target = ((Collider2D)ZoneManager.Instance.zones[zoneName2])?.gameObject;
            if(target != null)
            {
                oldTag = target.tag;
                target.tag = "PhotoSiteSupp";
            }
             
             target = ((Collider2D)ZoneManager.Instance.zones[zoneName3])?.gameObject;
           //  Debug.Log("Voici la valeur de la zoneName3:"+zoneName3+" et la target: "+target);
            if(target != null)
            {
                oldTag = target.tag;
                target.tag = "PhotoSiteSupp";
               // Debug.Log("Je mets à jour le tag de :"+target);
            }
        }
        
            
    }

}
