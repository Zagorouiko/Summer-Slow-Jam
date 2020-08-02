using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public enum TurnState
{
     START, PLAYER1TURN, PLAYER2TURN, GAMEOVER, REWINDING
}

public class TurnSystem : MonoBehaviour
{
    public TurnState state;
    public Text gameStateText;
    public Text roundText;
    public Block currentBlockInPlay;
    public GameObject RestartScreen;
    public bool isRewinding = false;
    public TimeBody sampleBlockPointinTime;
    public int pointintime;

    private BoxCollider gameAreaCollider;
    private int currentRound = 0;
    private Grabber grabber;
    //public playerData currentPlayerTurn;

    [SerializeField] List<Block> BlocksOutsidePlayArea;

    void Start()
    {
        gameAreaCollider = GetComponent<BoxCollider>();
        grabber = FindObjectOfType<Grabber>();
        BlocksOutsidePlayArea = new List<Block>();
        state = TurnState.START;
        gameStateText.text = "Game Starting";
        currentBlockInPlay = null;       
        StartCoroutine(StartFirstRound());
    }

    void FixedUpdate()
    {
        pointintime = sampleBlockPointinTime.countPointsinTime;
        SetColliderTrigger();
        //WaitForPlayer();
        BlockStatus();       
        isToppled();            
    }

    public int GetCurrentRound()
    {
        return currentRound;
    }

    IEnumerator StartFirstRound()
    {
        yield return new WaitForSeconds(3f);
        if (JengaGameManager.instance.player1.player != null && state == TurnState.START)
        {
            //SetPlayer1Interactibility(true);
            state = TurnState.PLAYER1TURN;
            StartCoroutine(Player1Round());
        }
    }

    IEnumerator Player1Round()
    {
        currentBlockInPlay = null;
        yield return new WaitForSeconds(3f);      
        state = TurnState.PLAYER1TURN;
        currentRound++;


        if (!(state == TurnState.GAMEOVER))
        {                      
            BlocksOutsidePlayArea.Clear();            
            roundText.text = "Round: " + currentRound;
            gameStateText.text = (JengaGameManager.instance.player1.player.NickName + "'s Turn");

            //currentPlayerTurn = JengaGameManager.instance.player1;
            //SetPlayer2Interactibility(false);
            //SetPlayer1Interactibility(true);
        }

    }

    IEnumerator Player2Round()
    {
        Debug.Log("PLAYER2ROUND");
        currentBlockInPlay = null;
        yield return new WaitForSeconds(3f);
        state = TurnState.PLAYER2TURN;
        currentRound++;


        if (!(state == TurnState.GAMEOVER))
        {                  
            BlocksOutsidePlayArea.Clear();           
            roundText.text = "Round: " + currentRound;
            gameStateText.text = "Player 2 Turn";

            //currentPlayerTurn = JengaGameManager.instance.player2; 
            //gameStateText.text = JengaGameManager.instance.player2.player.NickName + "'s Turn";
            //SetPlayer1Interactibility(false);
            //SetPlayer2Interactibility(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        CheckIfBlockMovedOutOfPlayArea(other);

        if (other.gameObject.tag == "Block" && state == TurnState.PLAYER1TURN || other.gameObject.tag == "Block" && state == TurnState.PLAYER2TURN)
        {
            Debug.Log("BLOCK EXITED: " + other.gameObject.name);
            BlocksOutsidePlayArea.Add(other.gameObject.GetComponent<Block>());
        }       
        
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

            if (other.gameObject.tag == "Block" && state == TurnState.PLAYER2TURN)
            {
                currentBlockInPlay = other.gameObject.GetComponent<Block>();
                currentBlockInPlay.isInPlay = true;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
               
        if (other.gameObject.GetComponent<Block>() == currentBlockInPlay)
        {
            CheckIfRoundOver();
        }     
    }

    private void CheckIfRoundOver()
    {
        if (currentBlockInPlay == null) { return; }

        if (currentBlockInPlay.isInPlay && !currentBlockInPlay.isHeld && state == TurnState.PLAYER1TURN)
        {
            Debug.Log("FIRING COROUTINE");
            StartCoroutine(Player2Round());
        }

        if (currentBlockInPlay.isInPlay && !currentBlockInPlay.isHeld && state == TurnState.PLAYER2TURN)
        {
            Debug.Log("FIRE PLAYER1ROUND COROUTINE");
            StartCoroutine(Player1Round());
        }
    }

    void isToppled()
    {
        if (!(state == TurnState.REWINDING))
        {
            if (BlocksOutsidePlayArea.Count >= 2 && state == TurnState.PLAYER1TURN)
            {
                currentBlockInPlay = null;
                RestartScreen.SetActive(true);
                gameStateText.text = JengaGameManager.instance.player1.player.NickName + " Lost the Game";
                roundText.text = "";
                state = TurnState.GAMEOVER;
            }

            if (BlocksOutsidePlayArea.Count >= 2 && state == TurnState.PLAYER2TURN)
            {
                currentBlockInPlay = null;
                RestartScreen.SetActive(true);
                gameStateText.text = "Player 2" + " Lost the Game";
                roundText.text = "";
                state = TurnState.GAMEOVER;

            }
        } 
    }

    public void Restart()
    {
        currentBlockInPlay = null;
        BlocksOutsidePlayArea.Clear();
        currentRound = 0;
        RestartScreen.SetActive(false);

        state = TurnState.REWINDING;       

        if (state == TurnState.REWINDING)
        {
            grabber.enabled = false;
            gameStateText.text = "Rewinding Blocks";
        }       
    }

    public void ResetMatch()
    {
        Debug.Log("RESETTING MATCH");
        state = TurnState.PLAYER1TURN;
        grabber.enabled = true;
        StartCoroutine(Player1Round());       
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

    void SetColliderTrigger()
    {
        if (state == TurnState.PLAYER1TURN || state == TurnState.PLAYER2TURN)
        {
            gameAreaCollider.enabled = true;
        } else
        {
            gameAreaCollider.enabled = false;
        }
    }
}

//Code for second player 

//private void WaitForPlayer()
//{
//    if (JengaGameManager.instance.player2.player == null)
//    {
//        gameStateText.text = "Waiting for Players";
//    }
//}

//IEnumerator InitiallyTurnOffPlayer1()
//{
//    yield return new WaitForSeconds(4f);
//    SetPlayer1Interactibility(false);
//}

//private static void SetPlayer1Interactibility(bool set)
//{
//    JengaGameManager.instance.player1FingerCollidersLeft.SetActive(set);
//    JengaGameManager.instance.player1SphereCollidersLeft.SetActive(set);
//    JengaGameManager.instance.player1FingerCollidersRight.SetActive(set);
//    JengaGameManager.instance.player1SphereCollidersRight.SetActive(set);

//    JengaGameManager.instance.player1LeftHand.GetComponent<Hand>().enabled = set;
//    JengaGameManager.instance.player1RightHand.GetComponent<Hand>().enabled = set;

//}

//private static void SetPlayer2Interactibility(bool set)
//{
//    JengaGameManager.instance.player1FingerCollidersLeft.SetActive(set);
//    JengaGameManager.instance.player1SphereCollidersLeft.SetActive(set);
//    JengaGameManager.instance.player1FingerCollidersRight.SetActive(set);
//    JengaGameManager.instance.player1SphereCollidersRight.SetActive(set);

//    JengaGameManager.instance.player2LeftHand.GetComponent<Hand>().enabled = set;
//    JengaGameManager.instance.player2RightHand.GetComponent<Hand>().enabled = set;
//}