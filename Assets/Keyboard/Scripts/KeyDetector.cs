using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class KeyDetector : MonoBehaviour
{
    public NetworkManager networkManager;
    public TextMeshPro playerTextOutput;
    void Start()
    {
        //playerTextOutput = GameObject.FindGameObjectWithTag("PlayerOutputText").GetComponentInChildren<TextMeshPro>();
    }

    void Update()
    {
        //Debug.Log(playerTextOutput);
    }

    private void OnTriggerEnter(Collider other)
    {
        var key = other.GetComponentInChildren<TextMeshPro>();

        if (key != null)
        {

            if (other.gameObject.GetComponent<KeyFeedback>().keyCanBeHitAgain)
            {
                if (key.text == "SPACE")
                {
                    playerTextOutput.text += " ";
                }
                else if (key.text == "<--")
                {
                    playerTextOutput.text = playerTextOutput.text.Substring(0, playerTextOutput.text.Length - 1);
                } else if (key.text == "Enter")
                {
                    networkManager.ConnectToPhotonServer();
                }
                else
                {
                    Debug.Log(playerTextOutput);
                    playerTextOutput.text += key.text;
                }
            }
        }
    }
}
