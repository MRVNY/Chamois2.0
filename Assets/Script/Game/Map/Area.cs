using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    private FogOfWar texture;

    void Start()
    {
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        
        if (col.CompareTag("Player") && Global.Personnage == "Randonneur")
        {
            //Debug.Log("Je rafraichis le pourcentage? Valeur de mapExplored: ..."+DSRandonneur.Instance.mapExplored);
            //TC-: FogOfWar.Instance.rafraichir(this.GetComponent<RectTransform>());
            if (DSRandonneur.Instance.mapExplored==false) {FogOfWar.Instance.calculateAll();}
        }
        

    }
}
