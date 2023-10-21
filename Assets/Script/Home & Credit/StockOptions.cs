using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StockOptions : MonoBehaviour
{
    public float volume;
    public bool enableSounds;

    public bool enableMusic;
    public bool enableHelp;
    public bool enableVibrations;

    public GameObject sliderVolume;
    public GameObject soundCheck;

    public GameObject musicCheck;
    public GameObject helpCheck;
    public GameObject vibrationsCheck;


    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        /*volume = GameObject.Find("Slider").GetComponent<Slider>().value;
        enableSounds = GameObject.Find("Sounds").GetComponent<Toggle>().isOn;
        enableHelp = GameObject.Find("Help").GetComponent<Toggle>().isOn;*/

        volume = (PlayerPrefs.GetFloat("volume", 1));
        enableSounds = (PlayerPrefs.GetInt("soundEffects", 1) != 0);
        enableMusic = (PlayerPrefs.GetInt("musicEffects", 1) != 0);
        enableHelp = (PlayerPrefs.GetInt("inGameHelp", 1) != 0);
        enableVibrations = (PlayerPrefs.GetInt("vibrations", 1) != 0);

        sliderVolume.GetComponent<Slider>().value = volume;
        soundCheck.GetComponent<Toggle>().isOn = enableSounds;
        musicCheck.GetComponent<Toggle>().isOn = enableMusic;
        helpCheck.GetComponent<Toggle>().isOn = enableHelp;
        vibrationsCheck.GetComponent<Toggle>().isOn = enableVibrations;
    }

    // Update is called once per frame

    public void saveOptions()
    {
        //Debug.Log("c'est bon saveOptions est bien lancé...");
        volume = sliderVolume.GetComponent<Slider>().value;
        enableSounds = soundCheck.GetComponent<Toggle>().isOn;
        enableMusic = musicCheck.GetComponent<Toggle>().isOn;
        enableHelp = helpCheck.GetComponent<Toggle>().isOn;
        enableVibrations = vibrationsCheck.GetComponent<Toggle>().isOn;

        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.SetInt("soundEffects", (enableSounds ? 1 : 0));
        PlayerPrefs.SetInt("musicEffects", (enableMusic ? 1 : 0));
        PlayerPrefs.SetInt("inGameHelp", (enableHelp ? 1 : 0));
        PlayerPrefs.SetInt("vibrations", (enableVibrations ? 1 : 0));
        PlayerPrefs.Save();
    }

    public void clearSave()
    {
        SaveLoad.DeleteAllSaveFiles();
        
        PlayerPrefs.DeleteAll();

        SceneManager.LoadScene("Menu");
    }
}
