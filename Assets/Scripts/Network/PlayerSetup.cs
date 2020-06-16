using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

namespace Valve.VR.InteractionSystem
{  
    public class PlayerSetup : MonoBehaviourPunCallbacks
    {
        [SerializeField] GameObject VRCamera;
        [SerializeField] GameObject leftHand;
        [SerializeField] GameObject rightHand;

        [SerializeField] TextMeshProUGUI playerNameText;

        void Start()
        {
            if (photonView.IsMine)
            {
                transform.GetComponent<Player>().enabled = true;
                VRCamera.GetComponent<Camera>().enabled = true;
                leftHand.SetActive(true);
                rightHand.SetActive(true);
            } else
            {
                transform.GetComponent<Player>().enabled = false;
                VRCamera.GetComponent<Camera>().enabled = false;
                leftHand.SetActive(false);
                rightHand.SetActive(false);
            }

            SetPlayerUI();
        }

        void SetPlayerUI()
        {
            if (playerNameText != null)
            {
                playerNameText.text = PhotonView.Find(photonView.ViewID).Owner.NickName;
                //playerNameText.text = photonView.Owner.NickName;
            }            
        }
    }
}

