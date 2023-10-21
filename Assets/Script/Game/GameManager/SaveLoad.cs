using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public static class SaveLoad
{
    private static string savePath;
    
    static SaveLoad()
    {
        savePath = Application.persistentDataPath;
    }
    
    /// <summary>
    /// Fonction qui récupère un objet pour le sauvegarder dans un fichier clé
    /// </summary>
    public static void Save<T>(T objectToSave, string key)
    {
        // chemin du fichier de sauvegarde
        string path = savePath + "/saves/";

        //création du fichier si il n'existe pas
        Directory.CreateDirectory(path);
        
        // convertion des paramètres de sauvegarde en fichiers binaires
        BinaryFormatter formatter = new BinaryFormatter();

        using(FileStream fileStream = new FileStream(path + key + ".txt", FileMode.Create))
        {
            // TC ajout de la recup de l'exception...
            try
            {
                //Debug.Log("hey je serialise un truc: "+key);
                formatter.Serialize(fileStream, objectToSave);
                //formatter.Serialize(fs, addresses);
                //fileStream.BeginWrite(objectToSave);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fileStream.Close();
            }
            
            // TC test en mode text pour débugguer
            StreamWriter writer = new StreamWriter(path+ key + "_.txt", true);
            writer.WriteLine(objectToSave.ToString());
            writer.Close();
            //test en lecture
            //StreamReader reader = new StreamReader(path+ key + "_.txt");
            //Print the text from the file
            //Debug.Log(reader.ReadToEnd());
            //reader.Close();

        }

        //Debug.Log("Sauvegarde réussie:"+path + key + ".txt");
        //Debug.Log(Application.persistentDataPath);
    }

    /// <summary>
    /// Cherche le fichier <param name="key"> clé </param> pour charcher un élément <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T"> T qui peux être n'importe quel type de variable</typeparam>
    public static T Load<T>(string key)
    {
        //Debug.Log("je cherche à loader: "+key);
        if (SaveExists(key))
        {
           
           
            string path = savePath + "/saves/";
            BinaryFormatter formatter = new BinaryFormatter();
            T returnValue = default(T);
            using (FileStream fileStream = new FileStream(path + key + ".txt", FileMode.Open))
            {
                //Debug.Log("hey J'ai un prob de fileStream avec ça: "+key);
                returnValue = (T)formatter.Deserialize(fileStream);
               // Debug.Log("Le Filestream donne:"+returnValue.ToString());
            }

            return returnValue;
        }
        else { //Debug.Log("hey y a pas de save pour ça: "+key);
            return default(T);
        }
    }

    /// <summary>
    /// Vérifie si un fichier de sauvegarde clé existe
    /// </summary>
    public static bool SaveExists(string key)
    {
        string path = savePath + "/saves/" + key + ".txt";
        return File.Exists(path);
    }

    /// <summary>
    /// Supprime tous les fichiers de sauvegardes
    /// </summary>
    public static void DeleteAllSaveFiles()
    {
        string path = savePath + "/saves/";
        DirectoryInfo dir = new DirectoryInfo(path);
        dir.Delete(true);
        Directory.CreateDirectory(path);
        PlayerPrefs.DeleteAll();
    }

    public async static Task SaveState()
    {
        Debug.Log("Je lance un save pour "+Global.Personnage+"...normalement quand j'accède au Menu...");
        
        //pos
        var vect = GOPointer.currentPlayer.transform.position;
        float pos1 = vect.x;
        float pos2 = vect.y;

        Save<float>(pos1, "pos1"+Global.Personnage);
        Save<float>(pos2, "pos2"+Global.Personnage);

        PlayerPrefs.SetInt(Global.Personnage,1);
        
        //fog
        if (Global.Personnage == "Randonneur")
        {
            RenderTexture rtShow = FogOfWar.Instance.rawShow.texture as RenderTexture;
            RenderTexture rtCount = FogOfWar.Instance.rawCount.texture as RenderTexture;

            Texture2D t2Show = toTexture2D(rtShow);
            Texture2D t2Count = toTexture2D(rtCount);

            byte[] bytesShow = t2Show.EncodeToPNG();
            byte[] bytesCount = t2Count.EncodeToPNG();

            Save<byte[]>(bytesShow, "FogShow");
            Save<byte[]>(bytesCount, "FogCount");
        }

        //NPC convo
        foreach (var npc in NPCManager.Instance.currentNPCList)
        {
            Save<string>(npc.firstNode, npc.name);
        }

        foreach (var npc in NPCManager.Instance.listDonneurs)
        {
            Save<string>(npc.firstNode, npc.name);
        }
        
        //ency
        Save<List<ContenuPages>>(GOPointer.currentEncy.pagesDynamic, "Ency"+Global.Personnage);
        
        //achi
        foreach (KeyValuePair<string, Achievement> achi in GOPointer.AchievementManager.achievements)
        {
            Save<bool>(achi.Value.unlocked, achi.Key);
        }
        
        //DS
        switch (Global.Personnage)
        {
            case "Chamois":
                Save<DSChamois>(DSChamois.Instance,"DSChamois");
                break;
            case "Randonneur":
                Save<DSRandonneur>(DSRandonneur.Instance,"DSRandonneur");
                break;
            case "Chasseur":
                Save<DSChasseur>(DSChasseur.Instance,"DSChasseur");
                break;
        }
        
        //Quest
        Save<List<PlayerQuest>>(QuestManager.Instance.foundQuests,"Quest"+Global.Personnage);
    }

    public static async Task LoadState()
    {
        //TC
        Debug.Log("Je lance un load...");

        if (Menu.saving != null)
        {
            await Menu.saving;
        }
        
        // DateTime start = DateTime.Now;
        
        // LoadJob loadJob = new LoadJob();
        // loadJob.Schedule().Complete();
        
        //pos
        float pos1 = Load<float>("pos1"+Global.Personnage);
        float pos2 = Load<float>("pos2"+Global.Personnage);
        if(pos1!=0 && pos2!=0) GOPointer.currentPlayer.transform.position = new Vector3(pos1, pos2, 0);
        
        //Debug.Log("Load pos: " + (DateTime.Now - start));
        // start = DateTime.Now;
        
        //fog
        if (Global.Personnage == "Randonneur")
        {
            Texture
                tShow = FogOfWar.Instance.rawShow
                    .texture; //GOPointer.FogOfWarCanvas.transform.Find("RawShow").GetComponent<RawImage>().texture;
            Texture
                tCount = FogOfWar.Instance.rawCount
                    .texture; //GOPointer.FogOfWarCanvas.transform.Find("RawCount").GetComponent<RawImage>().texture;

            byte[] bytesShow = Load<byte[]>("FogShow");
            byte[] bytesCount = Load<byte[]>("FogCount");

            //Debug.Log("Load bytes: " + (DateTime.Now - start));
            // start = DateTime.Now;

            if (bytesShow != null && bytesCount != null && tShow != null && tCount != null)
            {
                Texture2D t2Show = new Texture2D(tShow.width, tShow.height, TextureFormat.RGBA4444, false);
                Texture2D t2Count = new Texture2D(tCount.width, tCount.height, TextureFormat.RGBA4444, false);

                t2Show.LoadImage(bytesShow);
                t2Count.LoadImage(bytesCount);

                //Debug.Log("Load image: " + (DateTime.Now - start));
                // start = DateTime.Now;

                RenderTexture rtShow = new RenderTexture(t2Show.width, t2Show.height, 0);
                RenderTexture rtCount = new RenderTexture(t2Count.width, t2Count.height, 0);

                RenderTexture.active = rtShow;
                Graphics.Blit(t2Show, rtShow);
                FogOfWar.Instance.rawShow.texture = rtShow;
                FogOfWar.Instance.camShow.targetTexture = rtShow;

                RenderTexture.active = rtCount;
                Graphics.Blit(t2Count, rtCount);
                FogOfWar.Instance.rawCount.texture = rtCount;
                FogOfWar.Instance.camCount.targetTexture = rtCount;
            }

            //Debug.Log("t2 to rt: " + (DateTime.Now - start));
            // start = DateTime.Now;

            FogOfWar.Instance.calculateAll();

            //Debug.Log("calculate: " + (DateTime.Now - start));
            // start = DateTime.Now;
        }

        //NPC convo
        if(Init.convo!=null) await Init.convo;
        
        foreach (var npc in NPCManager.Instance.currentNPCList)
        {
            string tmp = Load<string>(npc.name);
            if (tmp != null)
            {
                npc.firstNode = tmp;
            }
        }
        
        foreach (var npc in NPCManager.Instance.listDonneurs)
        {
            string tmp = Load<string>(npc.name);
            if (tmp != null)
            {
                npc.firstNode = tmp;
            }
        }
        
        //Debug.Log("Load convo: " + (DateTime.Now - start));
        // start = DateTime.Now;
        
        //ency 
        var tmpEncy = Load<List<ContenuPages>>("Ency"+Global.Personnage);
        //Debug.Log("je reloade depuis la save: tmpEncy est chargée :"+tmpEncy);
        //if (tmpEncy != null) {Debug.Log("taille de tmpEncy chargé depuis save?:"+tmpEncy.Count);}
        if (tmpEncy != null)
        {
            if(GOPointer.currentEncy==null) GOPointer.Instance.Link();
            GOPointer.currentEncy.pagesDynamic = tmpEncy;
            //TC 
           // Debug.Log("Je rajoute un item à la main dans les pages dynam du GOPointer");
           // GOPointer.currentEncy.pagesDynamic.Add(new ContenuPages("bablabla"));
            //Debug.Log("taille de currentEncyDyn:"+GOPointer.currentEncy.pagesDynamic.Count);
        }
        
        //Debug.Log("Load ency: " + (DateTime.Now - start));
        
        //achi
        foreach (KeyValuePair<string, Achievement> achi in GOPointer.AchievementManager.achievements)
        {
            bool tmp = false;
            tmp = Load<bool>(achi.Key);
            achi.Value.unlocked = tmp;
        }
        
        //DS
        switch (Global.Personnage)
        {
            case "Chamois":
                var tmp = Load<DSChamois>("DSChamois");
                if (tmp != null) DSChamois.Instance = tmp;
                break;
            case "Randonneur":
                var tmp2 = Load<DSRandonneur>("DSRandonneur");
                if (tmp2 != null) DSRandonneur.Instance = tmp2;
                break;
            case "Chasseur":
                var tmp3 = Load<DSChasseur>("DSChasseur");
                if (tmp3 != null) DSChasseur.Instance = tmp3;
                break;
        }
        
        //Quest
        var tmpQuest = Load<List<PlayerQuest>>("Quest"+Global.Personnage);
        if (tmpQuest != null)
        {
            QuestManager.Instance.foundQuests = tmpQuest;
            QuestManager.Instance.currentQuest = QuestManager.Instance.foundQuests.Last();
        }
        
    }

    static Texture2D toTexture2D(RenderTexture texture)
    {
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA4444, false);
        RenderTexture.active = texture;

        texture2D.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
        texture2D.Apply();

        return texture2D;
    }
    
}
