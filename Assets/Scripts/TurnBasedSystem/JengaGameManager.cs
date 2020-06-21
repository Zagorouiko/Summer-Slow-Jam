using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public struct playerData
{
    public Player player;
    public int rewindAmount;
}

public class JengaGameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    public playerData player1;
    public playerData player2;

    public GameObject player1Head;
    public GameObject player2Head;

    public GameObject femaleHeadPrefab;
    public GameObject maleHeadPrefab;

    PhotonView player1ViewID;

    public GameObject player1SphereCollidersLeft;
    public GameObject player1FingerCollidersLeft;
    public GameObject player1SphereCollidersRight;
    public GameObject player1FingerCollidersRight;
    public GameObject player1LeftHand;
    public GameObject player1RightHand;


    public GameObject player2SphereCollidersLeft;
    public GameObject player2FingerCollidersLeft;
    public GameObject player2SphereCollidersRight;
    public GameObject player2FingerCollidersRight;
    public GameObject player2LeftHand;
    public GameObject player2RightHand;

    public static JengaGameManager instance { get; private set; }

    GameObject player1GO;
    GameObject player2GO;

    void Start()
    {
        instance = this;

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(-0.08f, 0f, -0.66f), Quaternion.identity);

            player1 = new playerData
            {
                player = PhotonNetwork.PlayerList[0],
                rewindAmount = 1
            };

            StartCoroutine(SetPlayer1Interactables());
                     
        }
    }

    IEnumerator SetPlayer1Interactables()
    {
        yield return new WaitForSeconds(3f);
        maleHeadPrefab = Instantiate(player1Head);
        player1ViewID = FindObjectOfType<PhotonView>();
        player1GO = PhotonView.Find(player1ViewID.ViewID).gameObject;

        var VRCamera = player1GO.transform.Find("SteamVRObjects/VRCamera").gameObject;
        maleHeadPrefab.transform.parent = VRCamera.transform;

        maleHeadPrefab.transform.position = new Vector3(0.8f, 0, -0.14f);
        maleHeadPrefab.transform.rotation = new Quaternion(0, 0, 0, 0);

        player1SphereCollidersLeft = player1GO.transform.Find("HandColliderLeft(Clone)/spheres").gameObject;
        player1FingerCollidersLeft = player1GO.transform.Find("HandColliderLeft(Clone)/fingers").gameObject;

        player1SphereCollidersRight = player1GO.transform.Find("HandColliderRight(Clone)/spheres").gameObject;
        player1FingerCollidersRight = player1GO.transform.Find("HandColliderRight(Clone)/fingers").gameObject;

        player1LeftHand = player1GO.transform.Find("SteamVRObjects/LeftHand").gameObject;
        player1RightHand = player1GO.transform.Find("SteamVRObjects/RightHand").gameObject;

    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Player _player2)
    {
        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
        player2 = new playerData
        {
            player = _player2,
            rewindAmount = 1
        };

        SetPlayer2Interactables();

    }

    private void Update()
    {
        maleHeadPrefab.transform.position = player1GO.transform.Find("SteamVRObjects/VRCamera").gameObject.transform.position;
        maleHeadPrefab.transform.rotation = player1GO.transform.Find("SteamVRObjects/VRCamera").gameObject.transform.rotation;

        if (PhotonNetwork.CountOfPlayers > 1)
        {
            femaleHeadPrefab.transform.position = player2GO.transform.Find("SteamVRObjects/VRCamera").gameObject.transform.position;
            femaleHeadPrefab.transform.rotation = player2GO.transform.Find("SteamVRObjects/VRCamera").gameObject.transform.rotation;
        }

    }

    private void SetPlayer2Interactables()
    {
        PhotonView[] players = new PhotonView[PhotonNetwork.PlayerList.Length];
        players = FindObjectsOfType<PhotonView>();

        foreach (PhotonView player in players)
        {
            if (!(player.ViewID == player1ViewID.ViewID))
            {
                femaleHeadPrefab = Instantiate(player2Head);
                player2GO = PhotonView.Find(player.ViewID).gameObject;

                var VRCamera = player2GO.transform.Find("SteamVRObjects/VRCamera").gameObject;
                femaleHeadPrefab.transform.parent = VRCamera.transform;

                

                femaleHeadPrefab.transform.position = new Vector3(0, 0, -0.14f);
                femaleHeadPrefab.transform.rotation = new Quaternion(0, 0, 0, 0);

                player2SphereCollidersLeft = player2GO.transform.Find("HandColliderLeft(Clone)/spheres").gameObject;
                player2FingerCollidersLeft = player2GO.transform.Find("HandColliderLeft(Clone)/fingers").gameObject;

                player2SphereCollidersRight = player2GO.transform.Find("HandColliderRight(Clone)/spheres").gameObject;
                player2FingerCollidersRight = player2GO.transform.Find("HandColliderRight(Clone)/fingers").gameObject;

                player2LeftHand = player1GO.transform.Find("SteamVRObjects/LeftHand").gameObject;
                player2RightHand = player1GO.transform.Find("SteamVRObjects/RightHand").gameObject;

            }
            else
            {
                return;
            }
        }
    }
}
