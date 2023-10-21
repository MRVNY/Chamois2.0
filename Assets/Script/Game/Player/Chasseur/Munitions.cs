using System.Collections;
using TMPro;
using UnityEngine;

public class Munitions : MonoBehaviour
{
    private int nbBalles;
    private Hashtable h = new Hashtable();
    public int nbBallesMax; 

    TextMeshProUGUI text;
    void Start()
    {
        nbBalles = nbBallesMax;
        text = GOPointer.MunitionsTexte;
        updateView();
    }

    public void perdUneBalle(){
        if (nbBalles>0) {nbBalles -= 1;}
        updateView();
        aDesMunitions();
    }

    public bool aDesMunitions()
    {
        if (nbBalles <= 0)
        {
            // TC : On va éviter de perdre quand on n'a plus de munitions...
            // mais afficher un message...
            GOPointer.CanvasGuideJeu.SetActive(true);
            GuideManager.Instance.guideText.SetText("Vous n'avez plus de munitions! Essayez d'en retrouver...");
            //GameOver.Instance.End("Vous n'avez plus de munitions");
            return false;
        }

        return true;
    }

    /// <summary>
    /// fonction qui remet le nombre de balles aux max
    /// </summary>
    public void recupereMunitions()
    {
        //Debug.Log("je recupereMunitions(): max= "+nbBallesMax+";nbBalles= "+nbBalles);
        if (nbBalles==nbBallesMax) 
        {
           //GuideManager.Instance.guideText.SetText("Vous avez déjà le plein de munitions...");
           //setMessage("Bravo! Essayez de nettoyer tout ce qui pourrait trainer dans le massif des Bauges...");
            if (PlayerPrefs.GetInt("soundEffects") == 1)
                        {
                            GOPointer.UIManager.GetComponent<AudioSource>().Play();               
                        }
            GOPointer.CanvasGuideJeu.GetComponent<GuideManager>().showGuide("Vous avez déjà le plein de munitions...");
        }
        else {
             if (PlayerPrefs.GetInt("soundEffects") == 1)
                        {
                            GOPointer.RechargeBox.GetComponent<AudioSource>().Play();               
                        }
            nbBalles = nbBallesMax;}
        updateView();
    }

    private void updateView()
    {
        text.SetText("" + nbBalles);
    }
}
