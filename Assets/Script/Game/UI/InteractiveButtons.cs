using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveButtons : MonoBehaviour
{

    public static GameObject Instance;
    public GameObject talk;
    public GameObject recharge;
    public GameObject validate;
    public GameObject pickUp;
    public GameObject throwTrash;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this.gameObject;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
