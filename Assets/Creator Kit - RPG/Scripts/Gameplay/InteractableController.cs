using System.Collections.Generic;
using System.Linq;
using RPGM.Core;
using RPGM.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace RPGM.Gameplay
{
    /// <summary>
    /// Main class for implementing NPC game objects & starting dialogs
    /// </summary>
   
    public class InteractableController : MonoBehaviour
    {
        protected InteractiveButtons Buttons;
        protected GameObject actionButton;
        protected new Camera camera;
        protected bool isTarget = false;
        
        //Tc
        //protected bool isNettoyeur = false;

        //protected bool isNettoyeurRando =false;
        public int collectedTrashNumber =0;
        public string type = "NPC";

        private string message = "";

        // Quete non-utilise
        protected Quest activeQuest = null;
        protected Quest[] quests;

        //Permet d'acceder a des fonctions, notamment dialog.Hide()
        protected GameModel model = Schedule.GetModel<GameModel>();
    
        void Start()
        {
            if (type != "NPC")
            {
                Buttons = GOPointer.interactiveButtons.GetComponent<InteractiveButtons>();
                camera = CameraControllerJoy.Instance.GetComponent<Camera>();
            }

            if (type == "Recharge" && Global.Personnage == "Chasseur")
            {
                actionButton = Buttons.recharge;
            }

            if(type == "Rando" && Global.Personnage == "Randonneur")
            {
                actionButton = Buttons.validate;
            }
            
            if(type == "Trash" && (Global.Personnage == "Chasseur" || Global.Personnage == "Randonneur"))
            {
                actionButton = Buttons.pickUp;
            }
            
            if(type == "TrashCan" && (Global.Personnage == "Chasseur" || Global.Personnage == "Randonneur"))
            {
                actionButton = Buttons.throwTrash;
            }
            
            if (actionButton != null)
            {
                actionButton.SetActive(false);
            }

        }
        protected void OnEnable()
        {
            // Ummm... there're no Quests in any children
            quests = gameObject.GetComponentsInChildren<Quest>();
        }
        
        protected void Update()
        {
            if(actionButton!=null && isTarget){
                actionButton.transform.position = Vector3.up * 100 + camera.WorldToScreenPoint(transform.position);
                //Debug.Log(transform.position);
            }

        }

        /// <summary>
        /// Si ce qui rentre dans la zone du NPC est un joueur, on teste son role et on demande l'affichage de la piece de conversation adequate
        /// A utiliser avec les zones de Draw2DShape
        /// </summary>
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Detector") && actionButton!=null)
            {
                actionButton.SetActive(true);
                actionButton.GetComponent<Button>().onClick.RemoveAllListeners();
                actionButton.GetComponent<Button>().onClick.AddListener(() => { onclick(); });
                isTarget = true;
            }
        }

        /// <summary>
        /// Si le joueur quitte le rayon du NPC, on cache le dialogue
        /// </summary>
        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Detector") && actionButton!=null)
            {
                actionButton.GetComponent<Button>().onClick.RemoveAllListeners();
                actionButton.SetActive(false);
                isTarget = false;
            }
        }

        void onclick()
        {
            if(message!=""){
                GOPointer.CanvasGuideJeu.GetComponent<GuideManager>().showGuide(message);
                //Debug.Log(message);
                
            }
            
            if(type=="Rando"){
                if(RandoManager.currentPoint <= RandoManager.totalPoints-1){
                    GOPointer.RandoManager.nextRando(this);
                }
            }

            if (type == "Trash")
            {
                if (Global.Personnage == "Chasseur")
                {
                        //TC modif
                if (chasseurDechet.dechetsMain<2)
                {
                     if (PlayerPrefs.GetInt("soundEffects") == 1)
                    {
                        GOPointer.DonneursInfos.GetComponent<AudioSource>().Play();
                         
                     }   
                    chasseurDechet.dechetsMain++;
                    // TC je marque ce déchet comme ramassé...
                    DSChasseur.Instance.dechetsRamasses[collectedTrashNumber]=true;
                    //GOPointer.ListDechets[collectedTrashNumber]=true;
                    
                    Destroy(gameObject);
                    

                // TODO ajouter un son
                chasseurDechet.updateView();
                }
                else
                {
                     // TODO Ajouter un son
                    // Debug.Log("Vous transportez déjà pas mal de déchets, allez vider dans une poubelle...");
                    if (PlayerPrefs.GetInt("soundEffects") == 1)
                        {
                            GOPointer.UIManager.GetComponent<AudioSource>().Play();               
                        }
                    setMessage("Vous transportez déjà pas mal de déchets, allez d'abord vider ceux-ci dans une poubelle...");
                    GOPointer.CanvasGuideJeu.GetComponent<GuideManager>().showGuide(message);
                    setMessage("");

                }
                }
                else if (Global.Personnage == "Randonneur")
                {
                           //TC modif
                if (randonneurDechet.dechetsMain<2)
                {
                     if (PlayerPrefs.GetInt("soundEffects") == 1)
                    {
                        
                        GOPointer.DonneursInfos.GetComponent<AudioSource>().Play();
               
                     }   
                    randonneurDechet.dechetsMain++;
                    // TC je marque ce déchet comme ramassé...
                    DSChasseur.Instance.dechetsRamasses[collectedTrashNumber]=true;
                    //GOPointer.ListDechets[collectedTrashNumber]=true;
                    
                    Destroy(gameObject);
                    

                // TODO ajouter un son
                //randonneurDechet.updateView();
                }
                else
                {
                     // TODO Ajouter un son
                    // Debug.Log("Vous transportez déjà pas mal de déchets, allez vider dans une poubelle...");
                    if (PlayerPrefs.GetInt("soundEffects") == 1)
                        {
                            GOPointer.UIManager.GetComponent<AudioSource>().Play();               
                        }
                    setMessage("Vous transportez déjà pas mal de déchets, allez d'abord vider ceux-ci dans une poubelle...");
                    GOPointer.CanvasGuideJeu.GetComponent<GuideManager>().showGuide(message);
                    setMessage("");

                }
                }
                
                   
            }
            // Gestion de la poubelle et déblocage des badges "ami de la nature, 3, 10, 25, 50"
            if(type == "TrashCan")
            {
               if (Global.Personnage == "Chasseur")
                {
                    if (chasseurDechet.dechetsMain!=0)
                {
                DSChasseur.Instance.dechets+=chasseurDechet.dechetsMain;
                //collectedTrashNumber+= chasseurDechet.dechetsMain;
                //Debug.Log("chasseurDechet: "+DSChasseur.Instance.dechets); 
                //&(GOPointer.AchievementManager.achievements["Badge Chasse I"].EarnAchievement())
                DSChasseur.Instance.Update();
                
                // test rapide des 4 badges
                //chasseurDechet.dechetsMain += 8;
                chasseurDechet.dechetsMain = 0;
                 if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.TrashCanBtn.GetComponent<AudioSource>().Play();
            }
                chasseurDechet.updateView();
                if (!DSChasseur.Instance.isNettoyeur)
                {
                   // Debug.Log(GOPointer.AchievementManager.achievements["Badge Chasse I"].EarnAchievement());
                    DSChasseur.Instance.isNettoyeur =true;
                    setMessage("Bravo! Essayez de nettoyer tout ce qui pourrait trainer dans le massif des Bauges...");
                    GOPointer.CanvasGuideJeu.GetComponent<GuideManager>().showGuide(message);
                    setMessage("");
                }
                    
                } 
                } else if (Global.Personnage == "Randonneur")
                {
                     if (randonneurDechet.dechetsMain!=0)
                {
                DSRandonneur.Instance.nbDechets+=randonneurDechet.dechetsMain;
                //collectedTrashNumber+= chasseurDechet.dechetsMain;
                //Debug.Log("chasseurDechet: "+DSChasseur.Instance.dechets); 
                //&(GOPointer.AchievementManager.achievements["Badge Chasse I"].EarnAchievement())
                DSRandonneur.Instance.Update();
                
                // test rapide des 4 badges
                //chasseurDechet.dechetsMain += 8;
                randonneurDechet.dechetsMain = 0;
                 if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.TrashCanBtn.GetComponent<AudioSource>().Play();
            }
                //randonneurDechet.updateView();
                if (!DSRandonneur.Instance.isNettoyeur)
                {
                   // Debug.Log(GOPointer.AchievementManager.achievements["Badge Chasse I"].EarnAchievement());
                    DSRandonneur.Instance.isNettoyeur =true;
                    if (PlayerPrefs.GetInt("soundEffects") == 1)
                        {
                            GOPointer.PlayerRandonneur.GetComponent<AudioSource>().Play();               
                        }
                    setMessage("Bravo! Bon comportement, vous ne pouvez transporter que 2 déchets à la fois mais cela vous rapporte des points...");
                    GOPointer.CanvasGuideJeu.GetComponent<GuideManager>().showGuide(message);
                    setMessage("");
                }
                    
                } 
                }
                
            }

         // Gestion de la boîte de munitions.
            if(type == "Recharge")
            {
                //Debug.Log("Je recharge en lançant recupMunitions()");
                GOPointer.PlayerChasseur.GetComponent<Munitions>().recupereMunitions();
            }
            
        }

        public void CompleteQuest(Quest q)
        {
            if (activeQuest != q) throw new System.Exception("Completed quest is not the active quest.");
            foreach (var i in activeQuest.requiredItems)
            {
                model.RemoveInventoryItem(i.item, i.count);
            }
            activeQuest.RewardItemsToPlayer();
            activeQuest.OnFinishQuest();
            activeQuest = null;
        }

        public void StartQuest(Quest q)
        {
            if (activeQuest != null) throw new System.Exception("Only one quest should be active.");
            activeQuest = q;
        }

        public void setMessage(string msg){
            message = msg;
        }
    }
}