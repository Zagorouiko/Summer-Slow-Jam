using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRManager : MonoBehaviour
{
    public GameObject head;
    //public GameObject leftHand;
    public GameObject rightHand;

    public static VRManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }    
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

}
