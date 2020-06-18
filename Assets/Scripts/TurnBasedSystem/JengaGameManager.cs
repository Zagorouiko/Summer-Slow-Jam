using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class JengaGameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    public Player player1;
    public Player player2;

    public static JengaGameManager instance { get; private set; }

    void Start()
    {
        instance = this;

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(-0.256f, 0f, 0.125f), Quaternion.identity);
            player1 = PhotonNetwork.PlayerList[0];

        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Player _player2)
    {
        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
        player2 = _player2;
    }
}
