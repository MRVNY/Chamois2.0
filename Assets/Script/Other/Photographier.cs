using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Photographier : MonoBehaviour
{
    /*
    public Boolean chauriondePrise = false;
    public Boolean peclozPrise = false;

    public Boolean armenazPrise =false;

    public Boolean charbonnetPrise =false;

    public Boolean coutarsePrise = false;
    */
    EncycloContentChasseur ency;

    // Start is called before the first frame update
    void Start()
    {
        ency = GOPointer.EncyclopedieManager.GetComponent<EncycloContentChasseur>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void prendrePhoto()
    {
        if(GOPointer.PlayerChasseur.GetComponent<TriggerZonePhoto>().dansChaurionde == true && DSChasseur.Instance.chauriondePrise == false)
        {
             if (PlayerPrefs.GetInt("soundEffects") == 1)
                        {
                           GOPointer.ChasseurUI.GetComponent<AudioSource>().Play();          
                        }
            //Debug.Log("PHOTO CHAURIONDE");
            //GuideManager.Instance.guideText.SetText("Bravo, vous venez de prendre une magnifique Photo depuis la pointe de Chaurionde!");
            GOPointer.CanvasGuideJeu.GetComponent<GuideManager>().showGuide("Bravo, vous venez de prendre une magnifique Photo depuis la pointe de Chaurionde!");
            //ajouter dans l'ency
            ency.addInfoToList("photoChaurionde", ency.pagesDynamic);
            //ency.addInfoToList("mange", ency.pagesDynamic);
            DSChasseur.Instance.nbPhoto += 1;
            DSChasseur.Instance.nbPhotoMemePartie += 1;
            DSChasseur.Instance.Update();
            
            DSChasseur.Instance.chauriondePrise = true;
            GOPointer.PhotoBtn.SetActive(false);
            Notifier.Instance.NewNotes();
        }
        else if(GOPointer.PlayerChasseur.GetComponent<TriggerZonePhoto>().dansPecloz == true && DSChasseur.Instance.peclozPrise == false)
        {
             if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
               GOPointer.ChasseurUI.GetComponent<AudioSource>().Play();
            }//Debug.Log("PHOTO PECLOZ");
             GOPointer.CanvasGuideJeu.GetComponent<GuideManager>().showGuide("Bravo, vous venez de prendre une magnifique Photo depuis le Mont Pecloz!");
            // ajouter dans l'ency
            ency.addInfoToList("photoPecloz", ency.pagesDynamic);
            DSChasseur.Instance.nbPhoto += 1;
            //Debug.Log("nbPhoto: "+DSChasseur.Instance.nbPhoto);
            DSChasseur.Instance.nbPhotoMemePartie += 1;
            DSChasseur.Instance.Update();
            DSChasseur.Instance.peclozPrise = true;
            GOPointer.PhotoBtn.SetActive(false);
            Notifier.Instance.NewNotes();
            
        }  else if(GOPointer.PlayerChasseur.GetComponent<TriggerZonePhoto>().dansArmenaz == true && DSChasseur.Instance.armenazPrise == false)
        {
             if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.ChasseurUI.GetComponent<AudioSource>().Play();
            }//Debug.Log("PHOTO PECLOZ");
             GOPointer.CanvasGuideJeu.GetComponent<GuideManager>().showGuide("Bravo, vous venez de prendre une magnifique photo depuis le Mont Armenaz!");
            // ajouter dans l'ency
            ency.addInfoToList("photoArmenaz", ency.pagesDynamic);
            DSChasseur.Instance.nbPhoto += 1;
            //Debug.Log("nbPhoto: "+DSChasseur.Instance.nbPhoto);
            DSChasseur.Instance.nbPhotoMemePartie += 1;
            DSChasseur.Instance.Update();
            DSChasseur.Instance.armenazPrise = true;
            GOPointer.PhotoBtn.SetActive(false);
            Notifier.Instance.NewNotes();
            
        }  else if(GOPointer.PlayerChasseur.GetComponent<TriggerZonePhoto>().dansCharbonnet == true && DSChasseur.Instance.charbonnetPrise == false)
        {
             if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.ChasseurUI.GetComponent<AudioSource>().Play();
            }//Debug.Log("PHOTO PECLOZ");
             GOPointer.CanvasGuideJeu.GetComponent<GuideManager>().showGuide("Bravo, vous venez de prendre une magnifique photo depuis le chalet de Charbonnet!");
            // ajouter dans l'ency
            ency.addInfoToList("photoCharbonnet", ency.pagesDynamic);
            DSChasseur.Instance.nbPhoto += 1;
            //Debug.Log("nbPhoto: "+DSChasseur.Instance.nbPhoto);
            DSChasseur.Instance.nbPhotoMemePartie += 1;
            DSChasseur.Instance.Update();
            DSChasseur.Instance.charbonnetPrise = true;
            GOPointer.PhotoBtn.SetActive(false);
            Notifier.Instance.NewNotes();
            
        } else if(GOPointer.PlayerChasseur.GetComponent<TriggerZonePhoto>().dansCoutarse == true && DSChasseur.Instance.coutarsePrise == false)
        {
             if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.ChasseurUI.GetComponent<AudioSource>().Play();
            }//Debug.Log("PHOTO PECLOZ");
             GOPointer.CanvasGuideJeu.GetComponent<GuideManager>().showGuide("Bravo, vous venez de prendre une magnifique photo depuis le refuge de Coutarse!");
            // ajouter dans l'ency
            ency.addInfoToList("photoCoutarse", ency.pagesDynamic);
            DSChasseur.Instance.nbPhoto += 1;
            //Debug.Log("nbPhoto: "+DSChasseur.Instance.nbPhoto);
            DSChasseur.Instance.nbPhotoMemePartie += 1;
            DSChasseur.Instance.Update();
            DSChasseur.Instance.coutarsePrise = true;
            GOPointer.PhotoBtn.SetActive(false);
            Notifier.Instance.NewNotes();
            
        }
    }
}
