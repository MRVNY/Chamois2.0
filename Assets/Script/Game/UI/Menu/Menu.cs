using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject resume;
    public Image menuIcon;
    public GameObject pauseIcon;
    private Notifier notifier;
    
    public GameObject NotifMenuDeroulant ;
    public List<GameObject> ListeBoutons;
    public static bool menuOuvre = false;

    public static Task saving;
    
    public static Menu Instance;


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

    async void Start()
    {
        if(GOPointer.GameControl==null) GOPointer.Instance.Link();
        if (Init.loading!=null) await Init.loading;
        if (GOPointer.linking!=null) await GOPointer.linking;
        
        notifier = GOPointer.GameControl.GetComponent<Notifier>();
        Deactivate();
        PauseMenu.Instance.Resume();
    }

    public void ActiveMain()
    {
        NotifMenuDeroulant.SetActive(false);
        if (menuIcon.enabled)
        {
            PauseMenu.Instance.Pause();
            Activate();
        }
        else
        {
            if (PlayerPrefs.GetInt("soundEffects") == 1)
            {
                GOPointer.MenuManager.GetComponent<AudioSource>().Play();
            }
            Deactivate();
        }
    }

    private void Activate()
    {
        //TC desactivé pour l'instant pour éviter de réécrire sur les sauvegardes Ency...
        //saving = SaveLoad.SaveState();

        resume.SetActive(true);
        menuIcon.enabled = false;
        pauseIcon.SetActive(true);
        InteractiveButtons.Instance.SetActive(false);
        
        for (int i = 0; i < ListeBoutons.Count; i++)
        {
            ListeBoutons[i].SetActive(true);
        }
        menuOuvre = true;
        if (PlayerPrefs.GetInt("soundEffects") == 1)
        {
            if(GOPointer.MenuManager==null) GOPointer.Instance.Link();
            GOPointer.MenuManager.GetComponent<AudioSource>().Play();
        }

    }
    
    public async void Deactivate()
    {
        if(saving!=null) await saving;
        if(GOPointer.EncyMenu==null) GOPointer.Instance.Link();

        resume.SetActive(false);
        menuIcon.enabled = true;
        pauseIcon.SetActive(false);
        InteractiveButtons.Instance.SetActive(true);

        GOPointer.EncyMenu.SetActive(false);
        
        if (PlayerPrefs.GetInt("soundEffects") == 1)
        {
            GOPointer.MenuManager.GetComponent<AudioSource>().Play();
        }

        for (int i = 0; i < ListeBoutons.Count; i++)
        {
            ListeBoutons[i].SetActive(false);
        }

        menuOuvre = false;
        PauseMenu.Instance.Resume();
    }
    
    public void Home()
    {
        saving = SaveLoad.SaveState();
        SceneManager.LoadScene("Menu");
    }

    public void Switch(string perso)
    {
        Rocks.sliding = false;
        // sauvegarde du perso précédent
        saving = SaveLoad.SaveState();
        Global.Personnage = perso;
        // sauvegarde du nouveau perso.
        //saving = SaveLoad.SaveState();
        // TC
        // Je tente de charger ce qui a été sauvé avec le nouveau perso. mais ça n'est pas là...
        //Debug.Log("Je tente de loader l'état de la sauvegarde...");
        //SaveLoad.LoadState();
        SceneManager.LoadScene("Game");
    }
    
    public void clearSave()
    {
        SaveLoad.DeleteAllSaveFiles();
        
        PlayerPrefs.DeleteAll();
        // TC Vérifier pourquoi il semble rester en mémoire les DSChamois ou autre.
        DSChamois.Instance= null;
        DSChasseur.Instance= null;
        DSRandonneur.Instance= null;
        SceneManager.LoadScene("Menu");
    }
}
