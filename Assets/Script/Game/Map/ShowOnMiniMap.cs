using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnMiniMap : MonoBehaviour
{
    public RectTransform bigMap;
    public RectTransform miniMap;
    public GameObject chamoisIcon;
    public GameObject chasseurIcon;
    public GameObject randoIcon;
    private GameObject playerIcon;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        chamoisIcon.SetActive(false);
        chasseurIcon.SetActive(false);
        randoIcon.SetActive(false);
        
        switch (Global.Personnage)
        {
            case "Chamois":
                playerIcon = chamoisIcon;
                player = GOPointer.PlayerChamois;
                break;
            case "Chasseur":
                playerIcon = chasseurIcon;
                player = GOPointer.PlayerChasseur;
                break;
            case "Randonneur":
                playerIcon = randoIcon;
                player = GOPointer.PlayerRandonneur;
                break;
        }
        playerIcon.SetActive(true);

        UpdatePos();
    }

    private void OnEnable()
    {
        if(playerIcon != null && player != null)
        {
            UpdatePos();
        }
    }

    // private void Update()
    // {
    //     UpdatePos();
    // }

    private void UpdatePos()
    {
        Debug.Log("J'update bien la minimap");
        Vector2 playerPos = player.transform.position;
        bigMap = Map.Instance.MainMap.GetComponent<RectTransform>();
        playerIcon.transform.position = translatePosition(playerPos);
        //TC tentative d'ajustement...
        playerIcon.transform.position= new Vector2(playerIcon.transform.position.x+40,playerIcon.transform.position.y-60);
    }

    Vector2 translatePosition(Vector2 pos)
    {
        Debug.Log("miniMap.rect.height: "+miniMap.rect.height);
        Debug.Log("bigMap.rect.height: "+bigMap.rect.height);
        return ((miniMap.rect.height / bigMap.rect.height) * (pos - (Vector2)bigMap.position)) + (Vector2)miniMap.position;
    }
}
