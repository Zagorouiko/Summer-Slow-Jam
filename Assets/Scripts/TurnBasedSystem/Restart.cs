using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
     public TurnSystem Turnsystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Grabber")
        {
            if (transform.parent.name == "Yes")
            {
                Turnsystem.Restart();
            }
            else if (transform.parent.name == "No")
            {
                SceneManager.LoadScene("GameLauncherScene");
            }
        }
    
    }
}
