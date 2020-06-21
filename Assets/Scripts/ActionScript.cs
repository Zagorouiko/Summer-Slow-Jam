using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
public class ActionScript : MonoBehaviour
{
    public SteamVR_Action_Boolean RewindAction;
    public SteamVR_Input_Sources handType;

    public bool isRewinding;

     void Start()
    {
        RewindAction.AddOnStateDownListener(ButtonDown, handType);
        RewindAction.AddOnStateUpListener(ButtonUp, handType);
    }

    public void ButtonDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Button is down");
        isRewinding = true;
    }

    public void ButtonUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        isRewinding = false;
        Debug.Log("Button is up");
    }
}