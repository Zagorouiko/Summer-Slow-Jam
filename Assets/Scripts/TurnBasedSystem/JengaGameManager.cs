using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class JengaGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject playerPrefab;
    public Player Player1;
    public Player Player2;

    public static JengaGameManager instance;

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            var playerGO = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(-0.256f, 0f, 0.125f), Quaternion.identity);
            Player1 = playerGO.GetComponent<Player>();
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Player player2)
    {
        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
        Player2 = player2;
    }
}
