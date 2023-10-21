using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChamoisInfosTab : MonoBehaviour
{
  public static Transform[] tab;

    void Awake() {

        tab = new Transform[transform.childCount];
        for (int i =0; i < tab.Length; i++)
        {
            tab[i] = transform.GetChild(i);
        }     
    }
}
