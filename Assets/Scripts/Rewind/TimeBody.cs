using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TimeBody : MonoBehaviour {

	bool isRewinding = false;

	float recordTime = 1000f;

	LinkedList<PointInTime> pointsInTime;

	Rigidbody rb;

	// Use this for initialization
	void Start () {
		pointsInTime = new LinkedList<PointInTime>();
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (SteamVR_Actions._default.Rewind.active)
			//StartRewind();
		if (!SteamVR_Actions._default.Rewind.active)
			StopRewind();
	}

	void FixedUpdate ()
	{
		if (isRewinding)
			Rewind();
		else
			Record();
	}

	void Rewind ()
	{
		if (pointsInTime.Count > 0)
		{
			PointInTime pointInTime = pointsInTime.First.Value;
            transform.position = pointInTime.position;
			transform.rotation = pointInTime.rotation;
			pointsInTime.RemoveFirst();
		} else
		{
			StopRewind();
		}
		
	}

	void Record ()
	{
		if (pointsInTime.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
		{
			//pointsInTime.RemoveLast();
		}

		pointsInTime.AddFirst(new PointInTime(transform.position, transform.rotation));
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
