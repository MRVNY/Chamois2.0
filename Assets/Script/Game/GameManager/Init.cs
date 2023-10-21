using System;
using System.Threading.Tasks;
using RPGM.Gameplay;
using UnityEngine;


///<summary>
/// classe qui active les bons composant de la scène pour jouer aux jeux
///</summary>

public class Init : MonoBehaviour
{

    public GOPointer gp;
    public NPCManager npcManager;
    
    public NPCController papa;
    public NPCController guide;
    public NPCController chamois;

    public static Task loading;
    public static Task convo;

    public static Task saving;
    
    void Awake()
    {
        gp.Link();
        
        GOPointer.CanvasGuideJeu.SetActive(true);
        GOPointer.EncyclopedieManager.SetActive(true);
        GOPointer.AchievementManager.transform.parent.gameObject.SetActive(true);
        GOPointer.FogOfWarCanvas.transform.parent.gameObject.SetActive(true);
        GOPointer.NPCCollection.SetActive(true);
        //GOPointer.currentMap.transform.parent.gameObject.SetActive(true);
        
        //decor? day&night
    }

    private void OnEnable()
    {
        if(gp==null) Awake();
    }

    async void Start() 
    {
        if(gp==null) Awake();
        if (GOPointer.linking!=null) await GOPointer.linking;
        
        Map.Instance.ChangeColor();

        // Init convo if player start the character the first time
        convo = npcManager.loadConvo();
        
        loading = SaveLoad.LoadState();
        
        //Screen.SetResolution(1280, 720, false);
        
        //FinPartie.Instance.enabled = false;

        GOPointer.EncyButton.SetActive(false);
        GOPointer.MenuManager.SetActive(true);
        GOPointer.ButtonMap.SetActive(false);
        GOPointer.MiniMap.SetActive(false);
        //GOPointer.CanvasButtonAchievment.SetActive(false);

        GOPointer.CameraFogOfWar.SetActive(false);
        GOPointer.FogOfWarCanvas.SetActive(false);
        
        GOPointer.PlayerChamois.SetActive(false);
        GOPointer.PlayerRandonneur.SetActive(false);
        GOPointer.PlayerChasseur.SetActive(false);

        GOPointer.currentPlayer.SetActive(true);
        
        /* TC test Global init
        Debug.Log("J'initialise pour la 1ere fois: le chasseur");
                GOPointer.EncyclopedieManager.GetComponent<EncycloContentChasseur>().initList();
        Debug.Log("J'initialise pour la 1ere fois: le rando");
                GOPointer.EncyclopedieManager.GetComponent<EncycloContentRandonneur>().initList();
         Debug.Log("J'initialise pour la 1ere fois: le chamois");
                GOPointer.EncyclopedieManager.GetComponent<EncycloContentChamois>().initList();
        */


        switch (Global.Personnage)
        {
            case "Chasseur":
                //TC Attention au traitement dans initList() pour charger les infos dyn.    
               // if (DSChasseur.Instance.initOnce)
                Debug.Log("J'initialise pour la 1ere fois: le chasseur");
                GOPointer.EncyclopedieManager.GetComponent<EncycloContentChasseur>().initList();
                GOPointer.ListeChamoisSauvages.SetActive(true);
                break;

            case "Randonneur":
                Debug.Log("J'initialise pour la 1ere fois: le rando");
                GOPointer.EncyclopedieManager.GetComponent<EncycloContentRandonneur>().initList();
                GOPointer.ListeChamoisSauvages.SetActive(false);
                GOPointer.CameraFogOfWar.SetActive(true);
                GOPointer.FogOfWarCanvas.SetActive(true);
                break;

            case "Chamois":
                Debug.Log("J'initialise pour la 1ere fois: le chamois");
                GOPointer.EncyclopedieManager.GetComponent<EncycloContentChamois>().initList();
                GOPointer.ListeChamoisSauvages.SetActive(false);
                break;
        }
        
        QuestManager.Instance.Start();
        QuestManager.Instance.gameObject.SetActive(false);
        
        CameraControllerJoy.Instance.ManualStart();

        if (!PlayerPrefs.HasKey(Global.Personnage))
        {
            if(convo!=null) await convo;
            GOPointer.UIManager.GetComponent<UIManager>().Start();

            switch (Global.Personnage)
            {
                case "Chamois":
                    chamois.gameObject.SetActive(true);
                    chamois.Start();
                    chamois.onclick("init");
                    // TC Je prépare la première sauvegarde
                    saving = SaveLoad.SaveState();
                    break;
                
                case "Chasseur":
                    papa.gameObject.SetActive(true);
                    papa.Start();
                    papa.onclick("init");
                    // TC Je prépare la première sauvegarde
                    saving = SaveLoad.SaveState();
                    break;
                
                case "Randonneur":
                    guide.gameObject.SetActive(true);
                    guide.Start();
                    guide.onclick("init");
                    // TC Je prépare la première sauvegarde
                    saving = SaveLoad.SaveState();
                    break;
            }
        }
        else
        {
            PauseMenu.Instance.Resume();
        }
    }

}
