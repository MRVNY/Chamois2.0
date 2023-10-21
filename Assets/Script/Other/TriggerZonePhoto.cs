using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TriggerZonePhoto : MonoBehaviour
{
    //public GameObject boutonPhoto;

    public Boolean dansChaurionde = false;
    public Boolean dansPecloz = false;

    public Boolean vuChaurionde = false;
    public Boolean vuPecloz = false;

     public Boolean dansCoutarse = false;
    public Boolean dansArmenaz = false;

    public Boolean dansCharbonnet = false;

    public Boolean vuCoutarse = false;
    public Boolean vuArmenaz = false;

    public Boolean vuCharbonnet =false;

    public Sprite photoChaurionde;
    public Sprite photoPecloz;
    private Sprite currentImage;

    public Sprite photoArmenaz;

    public Sprite photoCoutarse;

    public Sprite photoCharbonnet;

    //public GameObject zoneChaurionde;
    //public GameObject zonePecloz;

    //PolygonCollider2D PCChaurionde;
    //PolygonCollider2D PCPecloz;

    // Start is called before the first frame update
    void Start()
    {
        //PCChaurionde = GOPointer.zoneChaurionde.GetComponent<PolygonCollider2D>();
        //PCPecloz = GOPointer.zonePecloz.GetComponent<PolygonCollider2D>();
       // Debug.Log("J'ai chopé les colliders Photo dans start: "+PCPecloz);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        //PCChaurionde = zoneChaurionde.GetComponent<PolygonCollider2D>();
        //PCPecloz = zonePecloz.GetComponent<PolygonCollider2D>();
        //PCChaurionde = GOPointer.zoneChaurionde.GetComponent<PolygonCollider2D>();
        //PCPecloz = GOPointer.zonePecloz.GetComponent<PolygonCollider2D>();
        //Debug.Log("J'ai chopé les colliders Photo: PCPecloz: "+PCPecloz);
        //Debug.Log("Je détecte une entrée avec un collider zonePhoto");
        //Debug.Log("PCChaurionde vaut: "+PCChaurionde);
        //Debug.Log("collider vaut: "+collider);
        //Debug.Log("entrée fonction");
        if(Global.Personnage == "Chasseur")
        {
            //Debug.Log("ZonePhoto: on est bien le chasseur...");
            if (collider.ToString().Split(' ')[0] == "zoneChaurionde")
            {
                
                //Debug.Log("Vous êtes sur la pointe de la Chaurionde");
                if (!vuChaurionde) {
                        //PlayerChasseur.Instance.vitesse = 0.0f;
                        //currentPlayer.vitesse = 0.0f;
                        // J'essaie de stopper le joueur...
                        Joystick_Link.Instance.setSpeed(0.0f);
                         if (PlayerPrefs.GetInt("soundEffects") == 1)
                        {
                            GOPointer.Notif.GetComponent<AudioSource>().Play();               
                        }
                        GOPointer.CanvasGuideJeu.GetComponent<GuideManager>().showGuide("Vous êtes sur la pointe de la Chaurionde.");
                        vuChaurionde=true;
                    }
                dansChaurionde = true;
                // Si j'ai la quête activée...
                if (collider.CompareTag("PhotoSite")&& !DSChasseur.Instance.chauriondePrise)
                {
                    
                    GOPointer.PhotoBtn.SetActive(true);
                }
                currentImage = photoChaurionde;
                
                
            }
            else if (collider.ToString().Split(' ')[0] == "zonePecloz")
            {
                
                //Debug.Log("Vous êtes sur le mont Pecloz");
                if (!vuPecloz) {
                    //GOPointer.PlayerChasseur.currentSpeed = 0.0f;
                    Joystick_Link.Instance.setSpeed(0.0f);
                    if (PlayerPrefs.GetInt("soundEffects") == 1)
                        {
                            GOPointer.Notif.GetComponent<AudioSource>().Play();               
                        }
                    GOPointer.CanvasGuideJeu.GetComponent<GuideManager>().showGuide("Vous êtes sur le mont Pecloz.");
                     vuPecloz=true;
                     }
                dansPecloz = true;
                // Si la quête est activée
                if (collider.CompareTag("PhotoSite")&& !DSChasseur.Instance.peclozPrise)
                {
                    
                    GOPointer.PhotoBtn.SetActive(true);
                }
                              
                currentImage = photoPecloz;
            }else if (collider.ToString().Split(' ')[0] == "ResearchArmenaz")
                {
                               
                if (!vuArmenaz) {
                    //GOPointer.PlayerChasseur.currentSpeed = 0.0f;
                    if (PlayerPrefs.GetInt("soundEffects") == 1)
                        {
                            GOPointer.Notif.GetComponent<AudioSource>().Play();               
                        }
                    GOPointer.CanvasGuideJeu.GetComponent<GuideManager>().showGuide("Vous êtes dans la zone de recherche scientifique d'Armenaz.");
                     vuArmenaz=true;
                     }
                dansArmenaz = true;
                // Si la quête est activée
                if (collider.CompareTag("PhotoSiteSupp")&& !DSChasseur.Instance.armenazPrise)
                {
                    
                    GOPointer.PhotoBtn.SetActive(true);
                }
                              
                currentImage = photoArmenaz;
                } else if (collider.ToString().Split(' ')[0] == "ResearchCharbonnet")
                {
                               
                if (!vuCharbonnet) {
                    //GOPointer.PlayerChasseur.currentSpeed = 0.0f;
                    if (PlayerPrefs.GetInt("soundEffects") == 1)
                        {
                            GOPointer.Notif.GetComponent<AudioSource>().Play();               
                        }
                    GOPointer.CanvasGuideJeu.GetComponent<GuideManager>().showGuide("Vous êtes dans la zone de recherche scientifique du Charbonnet.");
                     vuCharbonnet=true;
                     }
                dansCharbonnet = true;
                // Si la quête est activée
                if (collider.CompareTag("PhotoSiteSupp") && !DSChasseur.Instance.charbonnetPrise)
                {
                    
                    GOPointer.PhotoBtn.SetActive(true);
                }
                              
                currentImage = photoCharbonnet;
                } else if (collider.ToString().Split(' ')[0] == "ResearchCoutarse")
                {
                               
                if (!vuCoutarse) {
                    //GOPointer.PlayerChasseur.currentSpeed = 0.0f;
                    if (PlayerPrefs.GetInt("soundEffects") == 1)
                        {
                           //if(GOPointer.Map==null) GOPointer.Instance.Link();
                            GOPointer.Notif.GetComponent<AudioSource>().Play();               
                        }
                    GOPointer.CanvasGuideJeu.GetComponent<GuideManager>().showGuide("Vous êtes dans la zone de recherche scientifique de Coutarse.");
                     vuCoutarse=true;
                     }
                dansCoutarse = true;
                // Si la quête est activée
                if (collider.CompareTag("PhotoSiteSupp") && !DSChasseur.Instance.coutarsePrise)
                {
                    
                    GOPointer.PhotoBtn.SetActive(true);
                }
                              
                currentImage = photoCoutarse;
                }
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if(Global.Personnage == "Chasseur")
        {
            string collStr = collider.ToString().Split(' ')[0];
            if (collStr == "zoneChaurionde" || collStr == "zonePecloz"
                || collStr == "ResearchArmenaz" || collStr == "ResearchCharbonnet"
                || collStr == "ResearchCoutarse")
            {
             //   Debug.Log("je remets tous les dansZone à false et je desactive le bouton photo.");
                dansPecloz = false;
                dansChaurionde = false;
                dansArmenaz =false;
                dansCharbonnet = false;
                dansCoutarse = false;
                //boutonPhoto.SetActive(false);
                GOPointer.PhotoBtn.SetActive(false);
            }
        }
    }
}
