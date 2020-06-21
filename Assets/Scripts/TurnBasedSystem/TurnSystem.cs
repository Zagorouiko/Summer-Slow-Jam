using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public enum TurnState
{
     START, PLAYER1TURN, PLAYER2TURN, GAMEOVER
}

public class TurnSystem : MonoBehaviour
{
    public TurnState state;
    public Text gameStateText;
    public Text roundText;
    public Block currentBlockInPlay;

    private int currentRound = 0;
    public playerData currentPlayerTurn;

    [SerializeField] List<Block> BlocksOutsidePlayArea;

    void Start()
    {
        BlocksOutsidePlayArea = new List<Block>();
        state = TurnState.START;
        //state = TurnState.PLAYER1TURN;
        gameStateText.text = "Game Starting";
        currentBlockInPlay = null;
        StartCoroutine(InitiallyTurnOffPlayer1());
    }

    void Update()
    {
        //Revert back when testing with a player 2
        StartFirstRound();
        WaitForPlayer();

        //StartFirstRoundTESTINGPLAYER2();
        //StartFirstRoundTESTING();

        BlockStatus();
        isToppled();
        

        
    }
    public int GetCurrentRound()
    {
        return currentRound;
    }

    IEnumerator InitiallyTurnOffPlayer1()
    {
        yield return new WaitForSeconds(4f);
        SetPlayer1Interactibility(false);
    }

    private void StartFirstRoundTESTINGPLAYER2()
    {
        if (JengaGameManager.instance.player1.player != null && state == TurnState.START)
        {
            state = TurnState.PLAYER1TURN;
            StartCoroutine(Player2Round());
        }
    }

    private void StartFirstRound()
    {
        if (JengaGameManager.instance.player1.player != null && JengaGameManager.instance.player2.player != null && state == TurnState.START)
        {
            SetPlayer1Interactibility(true);
            state = TurnState.PLAYER1TURN;
            StartCoroutine(Player1Round());
        }
    }

    private void StartFirstRoundTESTING()
    {
        if (JengaGameManager.instance.player1.player != null && state == TurnState.START)
        {
            state = TurnState.PLAYER1TURN;
            StartCoroutine(Player1Round());
        }
    }

    private void WaitForPlayer()
    {
        if (JengaGameManager.instance.player2.player == null)
        {
            gameStateText.text = "Waiting for Players";          
        }       
    }

    IEnumerator Player1Round()
    {
        yield return new WaitForSeconds(3f);
        //state = TurnState.PLAYER2TURN;
        if (!(state == TurnState.GAMEOVER))
        {
            currentPlayerTurn = JengaGameManager.instance.player1;
            currentRound++;
            BlocksOutsidePlayArea.Clear();
            currentBlockInPlay = null;
            roundText.text = "Round: " + currentRound;
            gameStateText.text = (JengaGameManager.instance.player1.player.NickName + "'s Turn");
            SetPlayer2Interactibility(false);
            SetPlayer1Interactibility(true);
        }

    }

    IEnumerator Player2Round()
    {
        yield return new WaitForSeconds(3f);
        //state = TurnState.PLAYER2TURN;
        if (!(state == TurnState.GAMEOVER))
        {
            currentPlayerTurn = JengaGameManager.instance.player2;
            currentRound++;
            BlocksOutsidePlayArea.Clear();
            currentBlockInPlay = null;
            roundText.text = "Round: " + currentRound;
            //gameStateText.text = "Player 2 Turn";
            gameStateText.text = JengaGameManager.instance.player2.player.NickName + "'s Turn";
            SetPlayer1Interactibility(false);
            SetPlayer2Interactibility(true);
        }
    }

    private static void SetPlayer1Interactibility(bool set)
    {
        JengaGameManager.instance.player1FingerCollidersLeft.SetActive(set);
        JengaGameManager.instance.player1SphereCollidersLeft.SetActive(set);
        JengaGameManager.instance.player1FingerCollidersRight.SetActive(set);
        JengaGameManager.instance.player1SphereCollidersRight.SetActive(set);

        JengaGameManager.instance.player1LeftHand.GetComponent<Hand>().enabled = set;
        JengaGameManager.instance.player1RightHand.GetComponent<Hand>().enabled = set;

    }

    private static void SetPlayer2Interactibility(bool set)
    {
        JengaGameManager.instance.player1FingerCollidersLeft.SetActive(set);
        JengaGameManager.instance.player1SphereCollidersLeft.SetActive(set);
        JengaGameManager.instance.player1FingerCollidersRight.SetActive(set);
        JengaGameManager.instance.player1SphereCollidersRight.SetActive(set);

        JengaGameManager.instance.player2LeftHand.GetComponent<Hand>().enabled = set;
        JengaGameManager.instance.player2RightHand.GetComponent<Hand>().enabled = set;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Block")
        {
            BlocksOutsidePlayArea.Add(other.gameObject.GetComponent<Block>());
        }
        
        CheckIfBlockMovedOutOfPlayArea(other);
    }

    private void CheckIfBlockMovedOutOfPlayArea(Collider other)
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
        CheckIfRoundOver();
    }

    private void CheckIfRoundOver()
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

    void isToppled()
    {
        if (BlocksOutsidePlayArea.Count >= 2 && state == TurnState.PLAYER1TURN)
        {          
            gameStateText.text = JengaGameManager.instance.player1.player.NickName + " Lost the Game";
            roundText.text = "";
            state = TurnState.GAMEOVER;
        }

        if (BlocksOutsidePlayArea.Count >= 2 && state == TurnState.PLAYER2TURN)
        {           
            gameStateText.text = JengaGameManager.instance.player2.player.NickName + " Lost the Game";
            roundText.text = "";
            state = TurnState.GAMEOVER;
        }
    }

    //void RestartRoundRewound()
    //{
    //    if (state == TurnState.GAMEOVER && currentPlayerTurn.player == JengaGameManager.instance.player1.player)
    //    {
    //        StartCoroutine(Player1Round());
    //    }

    //    if (state == TurnState.GAMEOVER && currentPlayerTurn.player == JengaGameManager.instance.player2.player)
    //    {
    //        StartCoroutine(Player2Round());
    //    }
    //}

    private void Restart(IEnumerable roundStart)
    {
        throw new NotImplementedException();
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
