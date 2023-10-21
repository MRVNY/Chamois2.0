using System;
using System.Collections;
using System.Collections.Generic;
using RPGM.Gameplay;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance;
    
    //JSON
    public TextAsset ENCY;
    
    public TextAsset ChamoisJson;
    public TextAsset ChasseurJson;
    public TextAsset RandoJson;

    private JObject JENCY;
    private JObject JPerso;

    private DSRandonneur dataRando;
    private DSChamois dataChamois;
    private DSChasseur dataChasseur;

    EncycloContentRandonneur encyRando;
    EncycloContentChamois encyChamois;
    EncycloContentChasseur encyChasseur;

    //NPC
    //Rando
    public NPCController[] randoNPCList;

    //Chasseur
    public NPCController[] ChasseurNPCList;

    //Chamois
    public NPCController[] chamoisNPCList;
    public GameObject ChamoisInfos;
    
    //Communs
    public GameObject DonneursInfos;
    
    [NonSerialized] public NPCController[] currentNPCList;
    [NonSerialized] public Hashtable currentNPCTable;
    
    //Donneurs
    [NonSerialized] public NPCController[] listDonneurs;

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

    public async Task loadConvo()
    {
        ChamoisInfos.SetActive(false);
        DonneursInfos.SetActive(false);
        
        foreach (var npc in ChasseurNPCList)
        {
            npc.gameObject.SetActive(false);
        }
        foreach (var npc in randoNPCList)
        {
            npc.gameObject.SetActive(false);
        }
        foreach (var npc in chamoisNPCList)
        {
            npc.gameObject.SetActive(false);
        }

        dataRando = DSRandonneur.Instance;
        dataChamois = DSChamois.Instance;
        dataChasseur = DSChasseur.Instance;
        
        encyRando = GOPointer.EncyclopedieManager.GetComponent<EncycloContentRandonneur>();
        encyChamois = GOPointer.EncyclopedieManager.GetComponent<EncycloContentChamois>();

        //TC Je la recharge et elle est vide?
        
        encyChasseur = GOPointer.EncyclopedieManager.GetComponent<EncycloContentChasseur>();
        //Debug.Log("encyChasseur: "+encyChasseur.ToString());
        /*
        if (DSChasseur.Instance.encyChasseurData!=null)
        {
            Debug.Log("DSencyChasseurData: "+DSChasseur.Instance.encyChasseurData.ToString()); 
        }
        */

        JENCY = (JObject)JObject.Parse(ENCY.text)[Global.Personnage];

        currentNPCList = null;
        
        switch (Global.Personnage)
        {
            case "Chamois":
                JPerso = JObject.Parse(ChamoisJson.text);
                currentNPCList = chamoisNPCList;

                ChamoisInfos.SetActive(true);
                listDonneurs = ChamoisInfos.GetComponentsInChildren<NPCController>();
                break;
            
            case "Chasseur":
                JPerso = JObject.Parse(ChasseurJson.text);
                currentNPCList = ChasseurNPCList;

                DonneursInfos.SetActive(true);
                listDonneurs = DonneursInfos.GetComponentsInChildren<NPCController>();
                break;
            
            case "Randonneur":
                JPerso = JObject.Parse(RandoJson.text);
                currentNPCList = randoNPCList;
                
                DonneursInfos.SetActive(true);
                listDonneurs = DonneursInfos.GetComponentsInChildren<NPCController>();
                break;
        }

        currentNPCTable = new Hashtable();
        
        foreach (var npc in currentNPCList)
        {
            currentNPCTable.Add(npc.name, npc);
            npc.gameObject.SetActive(true);
            npc.setConvo((JObject)JPerso[npc.name]);
            //npc.setFirstNode(Global.persoNum[Global.Personnage]);
        }
        
        foreach (var don in listDonneurs)
        {
            don.setConvo((JObject)JPerso[don.name]);
        }
    }

    public void actionRando(string hint)
    {
        DSRandonneur.Instance.nbInfos++;
        //Debug.Log("J'ajoute une info au rando:"+DSRandonneur.Instance.nbInfos);
         //Debug.Log("Que vaut liste (encyRando;pageDyn en long? "+GOPointer.EncyclopedieManager.GetComponent<EncycloContentRandonneur>().pagesDynamic);
         if (encyRando.pagesDynamic==null) {
            Debug.Log("Tanass...les pagesDynamic ne sont pas initalisées!");
            GOPointer.EncyclopedieManager.GetComponent<EncycloContentRandonneur>().initList();
            encyRando = GOPointer.EncyclopedieManager.GetComponent<EncycloContentRandonneur>();
            if (encyRando.pagesDynamic==null) {Debug.Log("ça merde toujours avec encyRando nulle!");}
         }
        encyRando.addInfoToList(hint,encyRando.pagesDynamic);
        DSRandonneur.Instance.Update();
    }

    public void actionChasseur(string hint)
    {
        DSChasseur.Instance.nbInfos++;
        encyChasseur.addInfoToList(hint, encyChasseur.pagesDynamic);
        // Je sauvegarde l'état de l'encycloChasseur.
        //dataChasseur.encyChasseurData = encyChasseur;
        DSChasseur.Instance.Update();
    }
    
    public void actionChamois(string hint)
    {
        DSChamois.Instance.nbInfos++;
        encyChamois.addInfoToList(hint,encyChamois.pagesDynamic);
        DSChamois.Instance.Update();
    }

    public async void switchNode(string hint)
    {
        if (Init.convo != null) await Init.convo;
        var tmp = hint.Split(",");
        var npcName = tmp[1];
        var node = tmp[2];
        //Debug.Log("dans switch node, voici node: "+node);
        if (!currentNPCTable.ContainsKey(npcName)) await loadConvo();
        ((NPCController)currentNPCTable[npcName])?.setFirstNode(node);
    }

    public async void questAction(string hint)
    {
        //TC
        //Debug.Log("tmp: "+hint);
        if (Init.convo != null) await Init.convo;
        var tmp = hint.Split(",");
        string from = tmp[1]; //from can be the hint or the NPC name (or both)
        string to = tmp[2]; //to is the NPC that you have to talk to next
        string questName = tmp[3];
        //TC
        //Debug.Log("SetFirstNode dans Manager questName"+questName);
        ((NPCController)currentNPCTable[to])?.setFirstNode(questName);
        ((NPCController)currentNPCTable[from])?.setFirstNode("asked");

        if (QuestManager.Instance.foundQuests[0].title != questName)
        {
            QuestManager.Instance.addQuest(questName);
            // Traitement d'un petit bug de démarrage possible du chasseur...
            //Debug.Log("Cas particulier, j'arrive en cours...");
            if ((Global.Personnage == "Chasseur") && (DSChasseur.Instance.nbQuetes==0))
            {
                DSChasseur.Instance.nbQuetes++;
                DSChasseur.Instance.Update();
            }
        }
        
        QuestManager.Instance.currentQuest.hintName = from;
        Notifier.Instance.NewQuest();
    }
}
