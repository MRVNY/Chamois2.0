using System;
using System.Collections;
using System.Collections.Generic;
using RPGM.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//TC
//[Serializable]
public class Encyclopedie : MonoBehaviour
{
    public List<ContenuPages> pagesStatic = new List<ContenuPages>();
    public List<ContenuPages> pagesDynamic = new List<ContenuPages>();
    public List<ContenuPages> quete = new List<ContenuPages>();
    public List<PlayerQuest> questList = new List<PlayerQuest>();

    private Encyclopedie showing;
    
    protected ContenuPages page;
    protected string chapitre;
    
    protected static Dictionary<string, EncycloInfos> dynamicInfo = new Dictionary<string, EncycloInfos>();
    protected List<EncycloInfos> staticInfo = new List<EncycloInfos>();
    
    public TextAsset jsonFile;
    //public ArrayList data = new ArrayList();
    public EncyInfo info;

    protected int pageActuelle = 0;

    protected GameObject pG;
    protected GameObject pD;

    public Font font;

    // A grossir pour Android...base 25
    public int fontSize = 35;
    public GameObject encyButtons;
    private GameObject leftButton;
    private GameObject rightButton;
    

    protected void Start()
    {
        pG = GOPointer.PageGauche;
        pD = GOPointer.PageDroite;

        GOPointer.Livre.SetActive(false);
        updateShowing();
        
        leftButton = encyButtons.transform.Find("Gauche").gameObject;
        rightButton = encyButtons.transform.Find("Droite").gameObject;
    }

    protected void updateShowing()
    {
        if (Global.Personnage == "Chamois") showing = gameObject.GetComponent<EncycloContentChamois>();
        else if(Global.Personnage == "Chasseur") { //TC
            // Debug.Log("Pour showing, je récupère le GOPointer de currentEncy:"+GOPointer.currentEncy);
             //showing = GOPointer.currentEncy;
            showing = gameObject.GetComponent<EncycloContentChasseur>();
        }
        else if(Global.Personnage == "Randonneur") showing = gameObject.GetComponent<EncycloContentRandonneur>();
    }

    protected void setPageStatic(List<EncycloInfos> pages)
    {
        page = new ContenuPages();
 
        foreach(EncycloInfos i in pages)
        {
           if (page.getPoidsActuel() >= page.getPoidsMax())
           {
               pagesStatic.Add(page);
               page = new ContenuPages();
           }
           page.Add(i);
        }
        pagesStatic.Add(page);
    }

    //TC : tentative de recup les infos...
    /*
     protected void setPageDynamic(List<ContenuPages> pages)
    {
        page = new ContenuPages();
 
        foreach(ContenuPages i in pages)
        {
           if (page.getPoidsActuel() >= page.getPoidsMax())
           {
               pagesDynamic.Add(page);
               page = new ContenuPages();
           }
           page.Add(i.getInformations());
        }
        pagesDynamic.Add(page);
    }*/

    protected void CurrentPage(int pageActuelle, List<ContenuPages> pages)
    {
        int pageGauche = pageActuelle + 1;
        if (pages.Count <= pageGauche)
        {
            formatagePage(pageGauche, pages, pG);
        }
        else if ( pages.Count > pageGauche )
        {
            formatagePage(pageGauche, pages, pG);
            formatagePage(pageGauche+1, pages, pD);
        }
        leftButton.SetActive(pageActuelle >= 2);
        rightButton.SetActive(pages.Count - pageActuelle >= 2);
    }

    protected void formatagePage(int indexe, List<ContenuPages> pages, GameObject gm)
    {
            ContenuPages page = pages[(indexe-1)];
            int id = 0;

            foreach(EncycloInfos i in page.getInformations())
            {
                if (!string.IsNullOrEmpty(i.getImage()))
                {
                    Sprite sprite = Resources.Load<Sprite>(i.getImage());
                   // Debug.Log("Je cherche à choper une image: "+i.getImage());
                    if (sprite != null)
                    {
                        GameObject image = new GameObject("image_" + id.ToString());
                    //    Debug.Log("L'image sera: image_"+i.ToString());
                        image.transform.SetParent(gm.transform);
                        Image img = image.AddComponent<Image>();
                        img.sprite = sprite;
                        img.preserveAspect = true;
                    }
                }

                if (!string.IsNullOrEmpty(i.getText()))
                {
                    GameObject text = new GameObject("text_" + id.ToString());
                    text.transform.SetParent(gm.transform);
                    text.AddComponent<Text>().text = i.getText();
                    Text txt = text.GetComponent<Text>();
                    txt.font = font;
                    txt.color = Color.black;
                    txt.fontSize = fontSize;
                    txt.resizeTextForBestFit =true;
                }
                id ++;
            }
    }

    protected void addInfoToList(string action, List<ContenuPages> liste, Dictionary<string, EncycloInfos> dico)
    {
         if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.NPCCollection.GetComponent<AudioSource>().Play();
               
            }
        if(liste==pagesDynamic) Notifier.Instance.NewNotes();
        
        //TC Ajout d'une valeur au compteur d'informations
        if (Global.Personnage=="Randonneur") {
          //Debug.Log("Le randonneur a chopé une info!");
          //DSRandonneur.Instance.nbInfos;
          //GOPointer.InfoRNCFS.GetComponentsInChildren<TextMeshProUGUI>[0]().SetText("Connaissances RNCFS : \n{0}% / 11", DSRandonneur.Instance.nbInfos);
          TextMeshProUGUI[] texte =  GOPointer.InfoRNCFS.GetComponentsInChildren<TextMeshProUGUI>(); 
          texte[0].SetText("Connaissances RNCFS : \n{0} / 11", DSRandonneur.Instance.nbInfos);   
        } 

       // Debug.Log("Que vaut liste? "+liste);

        if (!dico.ContainsKey(action))
            return;
        ContenuPages page = new ContenuPages();
        if (liste.Count > 0)
        {

            if (liste[liste.Count - 1].getPoidsActuel() <= liste[liste.Count - 1].getPoidsMax())
                liste[liste.Count - 1].Add(dico[action]);
            
            else
            {
                page.Add(dico[action]);
                liste.Add(page);
            }
        }
        else
        {
            page.Add(dico[action]);
            liste.Add(page);
        }
    }

    public void onClickGauche()
    {
        List<ContenuPages> pages = showing.getPages();

        if (pages != null && pages.Count > 0 && pageActuelle > 0)
        {
            onPageChanged(pG);
            onPageChanged(pD);
            pageActuelle -= 2;
            CurrentPage(pageActuelle, pages);
        }
    }
    
    public void onClickDroite()
    {
        List<ContenuPages> pages = showing.getPages();

        if (pages != null && pages.Count > 0 && pageActuelle +2 < pages.Count)
        {
            onPageChanged(pG);
            onPageChanged(pD);
            pageActuelle = Math.Min(pageActuelle+2,pages.Count);
            CurrentPage(pageActuelle, pages);
        }

    }

    public void onChapterSelected(string chapitre)
    {
        GOPointer.Livre.SetActive(true);
        GOPointer.EncyMenu.SetActive(false);

        //TC : j'efface les boutons qui pourraient apparaître par dessus...
        GOPointer.interactiveButtons.SetActive(false);
        encyButtons.SetActive(true);

        pageActuelle = 0;
        GOPointer.currentEncy.chapitre = chapitre;
        //Debug.Log("chapitre vaut: "+chapitre);
        //TC : effacement de la bonne notif de l'encyclopédie...
        if (chapitre=="statique") {
            UIManager.Instance.openObj();
        }
        else UIManager.Instance.endNotes();

        List<ContenuPages> pages = getPages();
        //Debug.Log("Voilà le nombre-contenu de pages pour: "+chapitre+"..."+pages.Count);
        if(pages != null && pages.Count > 0)
        {
            GOPointer.currentEncy.CurrentPage(pageActuelle, pages);
        }
        leftButton.SetActive(pageActuelle > 2);
        rightButton.SetActive(pages.Count - pageActuelle > 2);
}

    public void onEncyclopedieClosed()
    {
        onPageChanged(pG);
        onPageChanged(pD);
        GOPointer.Livre.SetActive(false);
        encyButtons.SetActive(false);
        UIManager.Instance.endEncy();
        //TC : je réactive les boutons qui pourraient avoir pu tenter d'apparaître par dessus...
        //Debug.Log("je reactive les boutons dans ENcyclo/onEncyClosed");
        GOPointer.interactiveButtons.SetActive(true);
    }

    private void onPageChanged(GameObject gm)
    {
        if (gm != null)
        {
            foreach(Transform i in gm.transform)
            {
                Destroy(i.gameObject);
            }
        }
    }

    public List<ContenuPages> getPages()
    {
        switch (showing.chapitre)
        {
            case "statique":
            { //Debug.Log("Showing nous donne les pages statiques...");
                return showing.pagesStatic;}
            case "dynamique":
            {   //Debug.Log("Showing nous donne les pages dynamiques...");
                return showing.pagesDynamic;
            }               
            case "quete":
            { //Debug.Log("Showing nous donne les quêtes");
                return showing.quete;
            }               
            default:
            { //Debug.Log("Showing nous donne rien !!! PRob");
                return null;
            }
                
        }
    }
}
