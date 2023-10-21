using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RPGM.Core;
using RPGM.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;


namespace RPGM.Gameplay
{
    /// <summary>
    /// Main class for implementing NPC game objects & starting dialogs
    /// </summary>
   
    public class NPCController : InteractableController
    {
        private ConversationScript conversations;
        public string firstNode;
        private JObject convoTree;
        // private TextAsset jsonFile;
        public bool randoValide, chassValide;

        public int index;

        public void Start()
        {
            camera = CameraControllerJoy.Instance.GetComponent<Camera>();
            actionButton = GOPointer.interactiveButtons.GetComponent<InteractiveButtons>().talk;
            
            // if(jsonFile != null){
            //     convoTree = JObject.Parse(jsonFile.text);
            //     constructConvoTree();
            // }
            
            actionButton.SetActive(false);
            //randoInit=false;
            //chassInit=false;
            //Debug.Log(randoInit+" ; "+chassInit);
            //firstNode = Global.persoNum[Global.Personnage];
        }

        /// <summary>
        /// Si ce qui rentre en dans la zone du NPC est un joueur, on test son role et on demande l'affichage de la piece de conversation adequate
        /// A utiliser avec les zones de Draw2DShape
        /// </summary>
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Detector") && actionButton!=null)
            {
                actionButton.SetActive(true);
                actionButton.GetComponent<Button>().onClick.RemoveAllListeners();
                actionButton.GetComponent<Button>().onClick.AddListener(() => { onclick(); });
            }
            
            if (collision.gameObject.CompareTag("Detector"))
            {
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
            }
            isTarget = false;
        }

        public void onclick()
        {
            
            if (Global.Personnage=="Randonneur") 
            {
                
                // Cas où le PNJ n'a jamais été contacté
                if (firstNode=="") {
                    firstNode = Global.persoNum[Global.Personnage];
                    //randoInit=true;
                }
                else
                {
                    // Cas où c'est un donneur d'infos
                    if (index!=-1)
                    {                
                         if (DSRandonneur.Instance.donneursInfosValideRando[index]==true)  
                        {
                            firstNode="2.0";
                        //    Debug.Log("J'ai mis à jour rando en 2.0");
                        }
                        else 
                        {
                            firstNode = Global.persoNum[Global.Personnage];
                        //    Debug.Log("J'initialise rando en 2");
                        }
                    } 
                        // Cas où c'est le guideRando pour qu'il détecte la fin de la randonnée                      
                        else 
                        {
                        //Debug.Log("Ai-je rando finie "+DSRandonneur.Instance.randoJustFinished);
                        // Détecter si rando finie et remettre à 2.
                            if (DSRandonneur.Instance.randoJustFinished)
                            {
                                DSRandonneur.Instance.randoJustFinished=false;
                                firstNode = Global.persoNum[Global.Personnage];
                                //Debug.Log("Je sors du if je l'ai mis à "+DSRandonneur.Instance.randoJustFinished);
                            }
                        }                        
                }
                   
               
                // il faut vérifier par quel perso
                /*
                    // s'il n'a jamais été contacté par rando, il faut remettre au départ
                    if ((chassInit) && (!randoInit))
                    {
                        firstNode = Global.persoNum[Global.Personnage];
                        randoInit=true;
                        Debug.Log("Je corrige le prob pour initRando");
                    }
                    else 
                    // dans les autres cas, on a les 
                        if (firstNode=="3" || firstNode=="2" || firstNode=="3.0")
                        {
                            firstNode="2.0";
                            Debug.Log("J'ai mis à jour rando en 2.0");
                        } */
            }
            if (Global.Personnage=="Chasseur") 
            {
                //TC
                /*
                Debug.Log("valeur de firstNode du Chasseur: "+firstNode);
                if (firstNode=="Un Chamois au comportement étonnant.1.1")  {
                    Debug.Log("je détecte l'acception d'Etonn");
                    DSChasseur.Instance.queteEtonnInit=true;
                } */
                // Cas où le PNJ n'a jamais été contacté
                if (firstNode=="") {
                    firstNode = Global.persoNum[Global.Personnage];
                    //TC Je bricole un démarrage rapide pour débugger la quete 3
                    //firstNode = "Un Chamois au comportement étonnant";
                    //chassInit=true;
                }
                else  
                      // Cas des PNJ donneurs d'infos
                    if /*(index!=-1)*/ (index>-1) 
                    {
                        if (DSChasseur.Instance.donneursInfosValideChass[index]==true)
                        {
                            firstNode = "3.0";
                          //  Debug.Log("J'ai mis à jour chasseur en 3.0");
                        }
                        else
                        {
                         firstNode = Global.persoNum[Global.Personnage];
                       //  Debug.Log("J'initialise chasseur en 3");
                        }
                    }
                    else { 
                        
                         switch (index)
                        {
                            case -10:
                                    {
                                         if ((QuestManager.Instance.currentQuest.title=="Une Chasse Photographique") && (!DSChasseur.Instance.quetePhotoInit))
                                        {
                                          // Debug.Log("j'adapte le photographe pour quete 2");
                                            firstNode = "Une Chasse Photographique";
                                            QuestManager.Instance.startPhotoQuest("zonePecloz","zoneChaurionde");
                                            DSChasseur.Instance.quetePhotoInit=true; 
                                        }

                                    }
                            break;
                            case -11:
                                    {
                                       //TC
                                        //Debug.Log("valeur de firstNode du Chasseur: "+firstNode);
                                       if ((QuestManager.Instance.currentQuest.title=="Un Chamois au comportement étonnant") && (!DSChasseur.Instance.queteEtonnInit))
                                        {
                                            //Debug.Log("j'adapte le donneur de quete pour quete 3");
                                            firstNode = "Un Chamois au comportement étonnant";
                                            //QuestManager.Instance.startSleepQuest();
                                            //DSChasseur.Instance.queteEtonnInit=true;
                                        }
                                        else if ((DSChasseur.Instance.queteEtonnInit)) {
                                            firstNode = "asked";
                                            }
                                        
                                    }
                            break;
                            case -12:
                                    {
                                        if ((QuestManager.Instance.currentQuest.title=="Un Chamois mal en point") && (!DSChasseur.Instance.queteMepInit))
                                        {
                                            //Debug.Log("j'adapte le gardeForestier pour quete 4");
                                            firstNode = "Un Chamois mal en point";
                                            //QuestManager.Instance.startSleepQuest();
                                            //DSChasseur.Instance.queteMepInit=true;
                                        }
                                        else if ((DSChasseur.Instance.queteMepInit)) {
                                            firstNode = "asked";
                                            } //startTimeQuest(new DateTime(2024,5,1));
                                    }
                            break;
                        }
                        
                        
                        
                        /*
                        // cas du photographe pour lancer la seconde quête 
                            Debug.Log("currentQuest: "+QuestManager.Instance.currentQuest.title);
                            if ((index==-10) && (QuestManager.Instance.currentQuest.title=="Une Chasse Photographique") && (!DSChasseur.Instance.quetePhotoInit))
                            {
                               Debug.Log("j'adapte le photographe pour quete 2");
                               firstNode = "Une Chasse Photographique";
                               QuestManager.Instance.startPhotoQuest("zonePecloz","zoneChaurionde");
                               DSChasseur.Instance.quetePhotoInit=true;
                            }
                            else   // Cas du donneur de quête 3
                                    if ((index==-11) && (QuestManager.Instance.currentQuest.title=="Un Chamois au comportement étonnant") && (!DSChasseur.Instance.queteEtonnInit))
                            {
                               Debug.Log("j'adapte le donneur de quete pour quete 3");
                               firstNode = "Un Chamois au comportement étonnant";
                               QuestManager.Instance.startSleepQuest();
                               DSChasseur.Instance.queteEtonnInit=true;
                            }else if ((index==-11) && (DSChasseur.Instance.queteEtonnInit)) {
                                    firstNode = "asked";
                                    }
                                    */
                    }  // fin cas PNJ spéciaux
                    
                       
                       /*
                    else               
                        if (firstNode=="2" || firstNode=="2.0" || firstNode=="3") 
                        {
                            firstNode="3.0";
                           
                        } */
            }
            
            if (Global.Personnage=="Chamois") 
            {
                
                // Cas où le PNJ n'a jamais été contacté
                if (firstNode=="") {
                    firstNode = Global.persoNum[Global.Personnage];
                    //chassInit=true;
                }
                else  
                    if (index!=-1) 
                    {
                        if (DSChamois.Instance.donneursInfosValideCham[index-100]==true)
                        {
                            firstNode = "1.0";
                        //    Debug.Log("J'ai mis à jour chamois en 1.0");
                        }
                        else
                        {
                         firstNode = Global.persoNum[Global.Personnage];
                       //  Debug.Log("J'initialise chamois en 1");
                        }
                    }
                    
                       
                       /*
                    else               
                        if (firstNode=="2" || firstNode=="2.0" || firstNode=="3") 
                        {
                            firstNode="3.0";
                           
                        } */
            }
            
            var c = GetConversation();           
            if (c!=null && c.isInIndex(firstNode))
            {
                SpriteRenderer myImage = gameObject.GetComponent<SpriteRenderer>();
                GOPointer.UIManager.GetComponent<UIManager>().startVisualNovel(myImage);
                var ev = Schedule.Add<Events.ShowConversation>();
                ev.conversation = c;
                ev.npc = this;
                ev.gameObject = gameObject;               
                ev.conversationItemKey = firstNode;
                
                
                //SaveLoad.SaveState();
            }
        }
        
        public void onclick(string node)
        {
            
            //Debug.Log("Affiche moi le node:"+node);
            firstNode = node;
            onclick();
            
            // var c = GetConversation();
            // if (c!=null && c.isInIndex(node))
            // {
            //     SpriteRenderer myImage = gameObject.GetComponent<SpriteRenderer>();
            //     GOPointer.UIManager.GetComponent<UIManager>().startVisualNovel(myImage);
            //     var ev = Schedule.Add<Events.ShowConversation>();
            //     ev.conversation = c;
            //     ev.npc = this;
            //     ev.gameObject = gameObject;
            //     ev.conversationItemKey = node;
            // }
        }

        /// <summary>
        /// Pas encore d'utilisation de quete, par defaut retourne le premier element de la liste de dialogues
        /// </summary>
        ConversationScript GetConversation()
        {
            // There are two ways to get the conversation: 1. by conversationscrit 2. by JSON
            if (activeQuest == null && conversations!=null){
                return conversations;
            }
            // foreach (var q in quests)
            // {
            //     if (q == activeQuest)
            //     {
            //         if (q.IsQuestComplete())
            //         {
            //             CompleteQuest(q);
            //             return q.questCompletedConversation;
            //         }
            //         return q.questInProgressConversation;
            //     }
            // }
            return null;
        }

        public void setConvo(JObject convo)
        {
            convoTree = convo;
            constructConvoTree();
        }
        
        public void setFirstNode(string node)
        {
            firstNode = node;
        }

        private void constructConvoTree()
        {
            if (convoTree != null)
            {
                conversations = gameObject.AddComponent<ConversationScript>();
                foreach (JProperty obj in convoTree.OfType<JProperty>())
                {
                    ConversationPiece c1 = new ConversationPiece()
                    {
                        id = obj.Name, text = (string)obj.Value["text"], options = new List<ConversationOption>(),
                        hint = (string)obj.Value["hint"]
                    };
                    if (obj.Value["choices"] != null)
                    {
                        foreach (JProperty cho in obj.Value["choices"].OfType<JProperty>())
                        {
                            c1.options.Add(new ConversationOption() { text = (string)cho.Value, targetId = cho.Name });
                        }
                    }

                    conversations.Add(c1);
                }

                conversations.OnAfterDeserialize();
            }
        }
    }
}