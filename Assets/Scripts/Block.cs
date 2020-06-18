using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool isHeld = false;
    public bool isInPlay = false;

    public void PickedUp()
    {
        Debug.Log("PICKED UP");
        isHeld = true;
    }

    public void Dropped()
    {
        Debug.Log("DROPPED");
        isHeld = false;
    }
}
