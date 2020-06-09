using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TurnState
{
    START, PLAYER1TURN, PLAYER2TURN, WON, LOST
}

public class TurnSystem : MonoBehaviour
{
    public TurnState state;
    JengaPlayer player1;
    //public JengaPlayer player2;

    public Text gameStateText;

    void Start()
    {
        state = TurnState.START;
        gameStateText.text = "Game Starting";
        player1 = FindObjectOfType<JengaPlayer>();

        StartCoroutine(Player1Round());
    }

    void Update()
    {

    }

    IEnumerator Player1Round()
    {
        yield return new WaitForSeconds(3f);
        gameStateText.text = "Player 1 Turn";
        state = TurnState.PLAYER1TURN;
    }

    IEnumerator Player2Round()
    {
        yield return new WaitForSeconds(3f);
        gameStateText.text = "Player 2 Turn";
        state = TurnState.PLAYER2TURN;
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.name +"block exited");
        if (other.gameObject.tag == "Block" && state == TurnState.PLAYER1TURN)
        {
            Debug.Log("block exited");
            StartCoroutine(Player2Round());
        }

        if (other.gameObject.name == "Block" && state == TurnState.PLAYER2TURN)
        {
            StartCoroutine(Player1Round());
        }
    }
}
