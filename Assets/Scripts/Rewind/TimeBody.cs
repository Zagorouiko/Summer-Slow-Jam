using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TimeBody : MonoBehaviour {

	LinkedList<PointInTime> pointsInTime;
	Rigidbody rb;
	TurnSystem turnSystem;

	public int countPointsinTime;


	void Start () {

		turnSystem = FindObjectOfType<TurnSystem>();
		pointsInTime = new LinkedList<PointInTime>();
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate ()
	{
		countPointsinTime = pointsInTime.Count;

		if (turnSystem.state == TurnState.REWINDING)
		{
			Rewind();
		}
		else
		{
			rb.isKinematic = false;
			Record();
		}			
	}

	void Rewind()
	{
		if (pointsInTime.Count > 0)
		{
			rb.isKinematic = true;
     		PointInTime pointInTime = pointsInTime.First.Value;
            transform.position = pointInTime.position;
			transform.rotation = pointInTime.rotation;
			pointsInTime.RemoveFirst();
		}

		if (pointsInTime.Count == 0)
		{
			turnSystem.ResetMatch();
		}		
	}

	void Record ()
	{
		pointsInTime.AddFirst(new PointInTime(transform.position, transform.rotation));
	}
}
