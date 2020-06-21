using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TimeBody : MonoBehaviour {

	bool isRewinding = false;

	LinkedList<PointInTime> pointsInTime;
	Rigidbody rb;
	TurnSystem turnSystem;
	ActionScript rewindActionScript;

	int lastRound;
	int currentRound;
	int currentRoundSnapshot;


	void Start () {

		rewindActionScript = FindObjectOfType<ActionScript>();

		turnSystem = FindObjectOfType<TurnSystem>();
		lastRound = turnSystem.GetCurrentRound();
		SetInitialRound();
		pointsInTime = new LinkedList<PointInTime>();
		rb = GetComponent<Rigidbody>();
	}

	
	void Update () {
		currentRound = turnSystem.GetCurrentRound();

		if (JengaGameManager.instance.player1.rewindAmount == 1 && JengaGameManager.instance.player1.player == turnSystem.currentPlayerTurn.player && turnSystem.state == TurnState.GAMEOVER)
		{
			if (rewindActionScript.isRewinding)
			{
				StartRewind();
			}				
		}

		//if (JengaGameManager.instance.player2.rewindAmount == 1 && turnSystem.state == TurnState.GAMEOVER)
		//{
		//	if (rewindActionScript.isRewinding)
		//		StartRewind();
		//}
	}

	//public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
	//{
	//	Debug.Log("Trigger pressed");
	//}

	void FixedUpdate ()
	{
		if (isRewinding)
		{
			Rewind();
		}
		else
		{
			Record();
		}			
	}

	void Rewind()
	{
		if (pointsInTime.Count > 0)
		{
     		PointInTime pointInTime = pointsInTime.First.Value;
            transform.position = pointInTime.position;
			transform.rotation = pointInTime.rotation;
			pointsInTime.RemoveFirst();
		} else
		{
			if (JengaGameManager.instance.player1.rewindAmount >= 1)
			{
				
				Debug.Log(JengaGameManager.instance.player1.rewindAmount);
				//Make it work with 2 players
				StopRewind();
				JengaGameManager.instance.player1.rewindAmount--;
			}
		}		
	}

	IEnumerable SetInitialRound()
	{
		yield return new WaitForSeconds(5f);
		currentRoundSnapshot = turnSystem.GetCurrentRound();
	}

	void Record ()
	{
		pointsInTime.AddFirst(new PointInTime(transform.position, transform.rotation));

		//if (lastRound != currentRound)
		//{
		//	pointsInTime.AddFirst(new PointInTime(transform.position, transform.rotation));
			

		//	if (currentRoundSnapshot != currentRound)
		//	{
		//		lastRound = currentRound;
		//		currentRoundSnapshot++;
		//		pointsInTime.Clear();
		//	}
		//}		
	}

	public void StartRewind ()
	{
		isRewinding = true;
		rb.isKinematic = true;
	}

	public void StopRewind ()
	{
		isRewinding = false;
		rb.isKinematic = false;
	}
}
