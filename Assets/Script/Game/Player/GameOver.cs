using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Text recap;
    public static GameOver Instance;

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
        gameObject.SetActive(false);

    }

    public void End(string msg)
    {
        UIManager.Instance.UIPause();
        
        switch (Global.Personnage)
        {
            case "Chamois":
                receiveDataChamois(msg);
                break;
            case "Chasseur":
                receiveDataChasseur(msg);
                break;
            case "Randonneur":
                receiveDataRandonneur(msg);
                break;
        }
    }

    public void receiveDataChamois(string msg)
    {
        DSChamois.Instance.sendData();
        Hashtable h = DSChamois.Instance.h;
        int tps = (int) h["tps"];
        //float tps = (float) h["tps"];
        int nourriture = (int) h["nouriture"];
        int score = (int)h["score"];
        int scBouffe = (int)h["scBouffe"];
        int scBlessure = (int)h["scBlessure"];
        int blessure = (int)h["blessure"];
        int scoreTps = (int)h["scoreTps"];

        recap.text = msg + "\nVous avez survécu pendant " + (int)tps + " jours\net vous avez mangé " + nourriture + " repas" + "\n\n Votre score est de : " + score + " pts" + "\n (Repas : " + nourriture + " soit " + scBouffe + " pts)" + "\n (Blessures : " + blessure + " soit " + scBlessure + " pts retirés)" + "\n (Temps de jeu : " + ((int)tps*2) + "s soit " + scoreTps + " pts)";
        
        gameObject.SetActive(true);
    }
    
    public void receiveDataChasseur(string msg)
    {
        DSChasseur.Instance.sendData();
        Hashtable h = DSChasseur.Instance.h;
        int dechets = (int)h["Dechets"];
        int scDechets = (int)h["scDechets"];
        int bonChamois = (int)h["bonChamois"];
        int scbonChamois = (int)h["scbonChamois"];
        int mauvaisChamois = (int)h["mauvaisChamois"];
        int scmauvaisChamois = (int)h["scmauvaisChamois"];
        int score = (int)h["score"];
        Time.timeScale = 0f;
        recap.text = msg + "\nvous avez un score final de: " + score + "pts" 
                     + "\n (Déchets Ramassés : " + dechets + " soit " + scDechets + "pts)" 
                     + "\n (Mauvais Chamois tué(s) : " + mauvaisChamois + " soit " + scmauvaisChamois + "pts en moins)" 
                     + "\n (Bon(s) Chamois identifié(s) : " + bonChamois + " : soit " + scbonChamois + "pts)";
        gameObject.SetActive(true);
    }

    public void receiveDataRandonneur(string msg)
    {
        DSRandonneur.Instance.sendData();
        Hashtable h = DSRandonneur.Instance.h;
        int epionScore = (int)h["epionScore"];
        int batterieScore = (int)h["batterieScore"];
        int dentPortesScore = (int)h["dentPortesScore"];
        int grandRocScore = (int)h["grandRocScore"];
        int pointesChauriondeScore = (int)h["pointesChauriondeScore"];
        int morbierScore = (int)h["morbierScore"];
        int nivoletScore = (int)h["nivoletScore"];
        int galoppazScore = (int)h["galoppazScore"];
        int colombierScore = (int)h["colombierScore"];
        int arcalodScore = (int)h["arcalodScore"];
        int trelodScore = (int)h["trelodScore"];
        int scoreTotal = (int)h["scoreTotal"];

        recap.text = msg + "\nVous avez effectué un score de " + scoreTotal + "pts" + "\n\n (L'Épion donne " + epionScore + "pts)    " + "(Fort de la Batterie donne " + batterieScore + "pts)    " + "(Dent des Portes octroie " + dentPortesScore + "pts)    " + "\n (Grand Roc attribue " + grandRocScore + "pts)    " + "(Pointes de la Chaurionde vaut " + pointesChauriondeScore + "pts)    " + "(Mont Morbier, c'est " + morbierScore + "pts)    " + "\n (Croix du Nivolet correspond à " + nivoletScore + "pts)    " + "(Pointe de la Galoppaz récolte " + galoppazScore + "pts)    " + "(Mont Colombier obtient " + colombierScore + "pts)    " + "\n (Pointe de l'Arcalod fournit " + arcalodScore + "pts)    " + "(Mont Trélod récompense de " + trelodScore + "pts)";
        gameObject.SetActive(true);
    }

    public void backToMenu()
    {
        SceneManager.LoadScene("Menu") ;
    }
    
    public void restartGame()
    {
        SaveLoad.DeleteAllSaveFiles();
        // TC Le restart semble garder des instances en mémoire...
        DSChamois.Instance= null;
        DSChasseur.Instance= null;
        DSRandonneur.Instance= null;
        SceneManager.LoadScene("Game");
    }
}
