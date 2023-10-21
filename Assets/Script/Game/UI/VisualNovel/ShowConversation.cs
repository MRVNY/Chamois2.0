﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using RPGM.Core;
using RPGM.Gameplay;
using RPGM.UI;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RPGM.Events
{
    /// <summary>
    /// This event will start a conversation with an NPC using a conversation script.
    /// </summary>
    /// <typeparam name="ShowConversation"></typeparam>
    public class ShowConversation : Event<ShowConversation>
    {
        public NPCController npc;
        public GameObject gameObject;
        public ConversationScript conversation;
        public string conversationItemKey;

        private DSRandonneur _ds;
        
        public ShowConversation(){
            _ds = DSRandonneur.Instance;
        }

        public override void Execute()
        {
            ConversationPiece ci;
            //default to first conversation item if no key is specified, else find the right conversation item.
            if (string.IsNullOrEmpty(conversationItemKey))
            {
                ci = conversation.items[0];
            }
            else
            {
                ci = conversation.Get(conversationItemKey);
                // if (conversationItemKey.Length == 1)
                // {
                //     GOPointer.VisualNovel.nextButton.enabled = false;
                //     GOPointer.VisualNovel.nextButtonImage.enabled = false;
                // }
            }

            //if this item contains an unstarted quest, schedule a start quest event for the quest.
            // if (ci.quest != null)
            // {
            //     if (!ci.quest.isStarted)
            //     {
            //         var ev = Schedule.Add<StartQuest>(1);
            //         //ev.quest = ci.quest;
            //         ev.npc = npc;
            //     }
            //     if (ci.quest.isFinished && ci.quest.questCompletedConversation != null)
            //     {
            //         ci = ci.quest.questCompletedConversation.items[0];
            //     }
            // }

            /// <summary>
            /// Voir le script ConversationPiece, et l'utilisation de hint
            /// </summary>
            if (!string.IsNullOrEmpty(ci.hint))
            {
                foreach (var hint in ci.hint.Split(";"))
                {
                    if (hint.Length > 4 && hint.Substring(0, 4) == "node")
                    {
                        NPCManager.Instance.switchNode(hint);
                    }

                    else if (hint.Length > 5 && hint.Substring(0, 5) == "quest")
                    {
                        NPCManager.Instance.questAction(hint);
                    }

                    else if (Global.Personnage == "Chasseur")
                    {
                        NPCManager.Instance.actionChasseur(hint);
                        var i=-1;
                             do 
                             {
                                i++;
                             }  while (DonneurInfosTab.tab[i].name!=npc.ToString().Split(' ')[0]);        
                            //Debug.Log("ok i vaut:"+i);
                            DSChasseur.Instance.donneursInfosValideChass[i]=true;
                            //npc.randoValide=true;
                            //npc.firstNode="asked";
                    }

                    else if (Global.Personnage == "Chamois")
                    {
                        NPCManager.Instance.actionChamois(hint);
                         var i=-1;
                             do 
                             {
                                i++;
                             }  while (ChamoisInfosTab.tab[i].name!=npc.ToString().Split(' ')[0]);        
                            //Debug.Log("ok iCham vaut:"+i);
                            DSChamois.Instance.donneursInfosValideCham[i]=true;
                    }

                    else if (Global.Personnage == "Randonneur")
                    {
                        if (hint.Length > 3 && hint.Substring(0, 3) == "ran")
                        {
                            RandoManager.Instance.startRando(hint.Substring(4));
                            Debug.Log("Nom de la rando lancée: "+hint.Substring(4));
                            //DSRandonneur.Instance.randoLancee=hint.Substring(4);
                        }
                        else
                        {
                            NPCManager.Instance.actionRando(hint);
                            //Debug.Log("ShowConv: Je valide une bonne info pour npc:?"+npc);
                            //choper le bon rang pour sauvegarder l'info...                          
                             var i=-1;
                             do 
                             {
                                i++;
                             }  while (DonneurInfosTab.tab[i].name!=npc.ToString().Split(' ')[0]);        
                            //Debug.Log("ok i vaut:"+i);
                            DSRandonneur.Instance.donneursInfosValideRando[i]=true;
                            //npc.randoValide=true;
                            //npc.firstNode="asked";
                        }
                    }
                }
            }

            //show the dialog
            VisualNovel dialog = model.getDialog();
            dialog.Show(ci.text, ci.options);
            //Debug.Log("voici l'id du texte que je montre:"+ci.id);
            // Je mets à jour le booléen pour la quête 3...
            if (ci.id=="Un Chamois au comportement étonnant.1.1")  {
                    //Debug.Log("je détecte l'acception d'Etonn");
                    DSChasseur.Instance.quete_en_cours=3;
                    QuestManager.Instance.startSleepQuest();
                    DSChasseur.Instance.queteEtonnInit=true;
                }
            else if (ci.id=="Un Chamois mal en point.1")
            {
                DSChasseur.Instance.quete_en_cours=3;
                QuestManager.Instance.startSleepQuest();
                DSChasseur.Instance.queteMepInit=true;
            }    
            // TC
            /*
            if (ci.audio != null)
            {
                UserInterfaceAudio.PlayClip(ci.audio);
            }
            */

            //speak some gibberish at two speech syllables per word.
            // TC Je kill le son pourri et surtout décalé...
            //UserInterfaceAudio.Speak(gameObject.GetInstanceID(), ci.text.Split(' ').Length * 2, 1);

            //if this conversation item has an id, register it in the model.
            if (!string.IsNullOrEmpty(ci.id))
                model.RegisterConversation(gameObject, ci.id);

            //setup conversation choices, if any.
            if (ci.options.Count == 0)
            {
                dialog.dialogLayout.fullScreenButton.Nullify();
                dialog.dialogLayout.fullScreenButton.gameObject.SetActive(false);
            }
            else if (ci.options.Count == 1 && ci.options[0].text == "")
            {
                //if there's no buttons but we need to jump to a node
                dialog.dialogLayout.fullScreenButton.Nullify();
                dialog.dialogLayout.fullScreenButton.onClickEvent += () =>
                {
                    dialog.Hide();
                    var next = ci.options[0].targetId;

                    if (conversation.ContainsKey(next))
                    {
                        var c = conversation.Get(next);
                        var ev = Schedule.Add<ShowConversation>();
                        ev.conversation = conversation;
                        ev.gameObject = gameObject;
                        ev.conversationItemKey = next;
                    }
                };
            }
            else
            {
                dialog.dialogLayout.fullScreenButton.Nullify();
                dialog.dialogLayout.fullScreenButton.gameObject.SetActive(false);
                for (int i = 0; i < ci.options.Count; i++)
                {
                    //dialog.SetButton(i, ci.options[i].text);

                    dialog.dialogLayout.buttons[i].Nullify();
                    dialog.dialogLayout.buttons[i].onClickEvent += () =>
                    {
                        //hide the old text, so we can display the new.
                        dialog.Hide();

                        //This is the id of the next conversation piece.
                        int index = dialog.getClickedButton();
                        var next = ci.options[index].targetId;

                        //Make sure it actually exists!
                        if (conversation.ContainsKey(next))
                        {
                            //find the conversation piece object and setup a new event with correct parameters.
                            var c = conversation.Get(next);
                            var ev = Schedule.Add<ShowConversation>();
                            ev.conversation = conversation;
                            ev.gameObject = gameObject;
                            ev.conversationItemKey = next;
                        }
                        else
                        {
                            Debug.LogError($"No conversation with ID:{next}");
                        }
                    };
                }
            }
        }
    }
}