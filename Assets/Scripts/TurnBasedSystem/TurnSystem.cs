using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TurnState
{
     START, PLAYER1TURN, PLAYER2TURN, GAMEOVER
}

public class TurnSystem : MonoBehaviour
{
    public TurnState state;
    public Text gameStateText;

    public Block currentBlockInPlay;

    //Get both player prefabs (hands)

    void Start()
    {
        state = TurnState.START;
        //state = TurnState.PLAYER1TURN;
        gameStateText.text = "Game Starting";
        currentBlockInPlay = null;
    }

    void Update()
    {
        //Revert back when testing with a player 2
        //StartFirstRound();
        StartFirstRoundTESTING();
        BlockStatus();

        //} else if (JengaGameManager.instance.player2 == null)
        //{
        //    WaitForPlayer();
        //}

        //if (isToppled())
        //{
        //    state = TurnState.GAMEOVER;
        //}
    }

    private void StartFirstRound()
    {
        if (JengaGameManager.instance.player1 != null && JengaGameManager.instance.player2 != null && state == TurnState.START)
        {
            state = TurnState.PLAYER1TURN;
            StartCoroutine(Player1Round());
        }
    }

    private void StartFirstRoundTESTING()
    {
        if (JengaGameManager.instance.player1 != null && state == TurnState.START)
        {
            state = TurnState.PLAYER1TURN;
            StartCoroutine(Player1Round());
        }
    }

    private void WaitForPlayer()
    {
        //If new player joins set state to START
        gameStateText.text = "Waiting for Players";
    }

    IEnumerator Player1Round()
    {
        //NOT EXECUTING HERE
        currentBlockInPlay = null;
        yield return new WaitForSeconds(3f);
        gameStateText.text = JengaGameManager.instance.player1.NickName + "'s Turn";
        //Turn off player2 interactibility
        //Turn on player1 interactibility
    }

    IEnumerator Player2Round()
    {
        currentBlockInPlay = null;
        yield return new WaitForSeconds(3f);
        gameStateText.text = "Player 2 Turn";
        //gameStateText.text = JengaGameManager.instance.player2.NickName + "'s Turn";
        //Turn off player1 interactibility
        //Turn on player2 interactibility
    }

    void OnTriggerExit(Collider other)
    {
        if (currentBlockInPlay == null)
        {
            if (other.gameObject.tag == "Block" && state == TurnState.PLAYER1TURN)
            {
                currentBlockInPlay = other.gameObject.GetComponent<Block>();
                currentBlockInPlay.isInPlay = true;
            }

            if (other.gameObject.name == "Block" && state == TurnState.PLAYER2TURN)
            {
                currentBlockInPlay = other.gameObject.GetComponent<Block>();
                currentBlockInPlay.isInPlay = true;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        CheckIfEndOfRound();
    }

    private void CheckIfEndOfRound()
    {
        if (currentBlockInPlay == null) { return; }

        if (currentBlockInPlay.isInPlay && !currentBlockInPlay.isHeld && state == TurnState.PLAYER1TURN)
        {
            StartCoroutine(Player2Round());
        }

        if (currentBlockInPlay.isInPlay && !currentBlockInPlay.isHeld && state == TurnState.PLAYER2TURN)
        {
            StartCoroutine(Player1Round());
        }
    }

    private bool isToppled()
    {
        throw new NotImplementedException();
        //Game Over condition
        //Check if more than 2 blocks exited the collider
    }

    void BlockStatus()
    {
        if (currentBlockInPlay == null) { return; }

        if (!currentBlockInPlay.isHeld)
        {
            currentBlockInPlay.isInPlay = false;
        }

        if (currentBlockInPlay.isHeld)
        {
            currentBlockInPlay.isInPlay = true;
        }
    }
}
