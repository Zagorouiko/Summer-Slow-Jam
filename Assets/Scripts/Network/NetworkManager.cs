using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun;
using System;


    public class NetworkManager : MonoBehaviourPunCallbacks
  { 
    public GameObject EnterGamePanel;
    public GameObject ConnectionStatusPanel;
    public Text titleText;

    #region Unity Methods
        public void Start()
    {
        EnterGamePanel.SetActive(true);
        ConnectionStatusPanel.SetActive(false);
     }

     void Awake()
     {
         PhotonNetwork.AutomaticallySyncScene = true;
     }

        #endregion

        #region Public Methods
        public void ConnectToPhotonServer()
        {
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
                ConnectionStatusPanel.SetActive(true);
                EnterGamePanel.SetActive(false);
                titleText.enabled = false;
            }
        }

        private void CreateAndJoinRoom()
        {
            string randomName = "Room " + UnityEngine.Random.Range(0, 10);

            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsOpen = true;
            roomOptions.IsVisible = true;
            roomOptions.PublishUserId = true;
            roomOptions.MaxPlayers = 2;

            PhotonNetwork.CreateRoom(randomName, roomOptions);
        }

        #endregion

        #region Photon Callbacks

        public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN. This client is now connected to Master Server in region [" + PhotonNetwork.CloudRegion +
            "] and can join a room. Calling: PhotonNetwork.JoinRandomRoom();");
            Debug.Log(PhotonNetwork.NickName + " connected to server");
            PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedLobby()
    {
        
        Debug.Log("OnJoinedLobby(). This client is now connected to Relay in region [" + PhotonNetwork.CloudRegion + "]. This script now calls: PhotonNetwork.JoinRandomRoom();");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available in region [" + PhotonNetwork.CloudRegion + "], so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
        CreateAndJoinRoom();
    }

        public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected(" + cause + ")");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + ", player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

        public override void OnJoinedRoom()
    {     
        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room in region [" + PhotonNetwork.CloudRegion + "]. Game is now running.");
        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);

        PhotonNetwork.LoadLevel("GameScene");

        //PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(-0.256f, 0f, 0.125f), Quaternion.identity);
        //PhotonNetwork.Instantiate(headPrefab.name, VRManager.instance.head.transform.position, VRManager.instance.head.transform.rotation, 0);
        //PhotonNetwork.Instantiate(leftHandPrefab.name, VRManager.instance.leftHand.transform.position, VRManager.instance.leftHand.transform.rotation, 0);
        //PhotonNetwork.Instantiate(rightHandPrefab.name, VRManager.instance.rightHand.transform.position, VRManager.instance.rightHand.transform.rotation, 0);
        }
    }
    #endregion
