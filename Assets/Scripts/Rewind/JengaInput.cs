using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class JengaInput : MonoBehaviour
{
    void Update()
    {
        if (SteamVR_Actions._default.Rewind.active)
        {
            Debug.Log("BUTTON PRESSED");
        }
    }
}
