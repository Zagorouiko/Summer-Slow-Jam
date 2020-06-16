﻿using System.Collections;
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

    public Text gameStateText;

    void Start()
    {
        state = TurnState.START;
        gameStateText.text = "Game Starting";

        StartCoroutine(Player1Round());
    }

    IEnumerator Player1Round()
    {
        yield return new WaitForSeconds(3f);
        gameStateText.text = JengaGameManager.instance.Player1.NickName;
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
        if (other.gameObject.tag == "Block" && state == TurnState.PLAYER1TURN)
        {
            StartCoroutine(Player2Round());
        }

        if (other.gameObject.name == "Block" && state == TurnState.PLAYER2TURN)
        {
            StartCoroutine(Player1Round());
        }
    }
}
