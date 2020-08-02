using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBlock : MonoBehaviour
{

    public float timeTakenDuringLerp = 1f;
    public float speed;

    public float distanceToMove;

    private bool _isLerping;

    private Vector3 _endPosition;

    private float _timeStartedLerping;
    private float _timeStartedLerpingBackwards;

    private float percentageComplete;

    public int blocktype;

    public Vector3 direction;
    public Quaternion rotation;

    void Start()
    {
        rotation = Quaternion.Euler(0, transform.parent.localRotation.y, 0);
        StartLerping();
    }

    void FixedUpdate()
    {
        
        if (_isLerping)
        {
            float timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / timeTakenDuringLerp;

            transform.position = Vector3.Lerp(transform.position, _endPosition, speed * Time.deltaTime);

            if (percentageComplete >= 1.0f)
            {
                StartLerpingBackwards();
            }
        }

        if (!_isLerping)
        {
            float timeSinceStarted = Time.time - _timeStartedLerpingBackwards;
            percentageComplete = timeSinceStarted / timeTakenDuringLerp;
            transform.position = Vector3.Lerp(transform.position, _endPosition, speed * Time.deltaTime);

            if (percentageComplete >= 1.0f)
            {
                StartLerping();
            }
        }
    }

    void StartLerping()
    {
        _isLerping = true;
        _timeStartedLerping = Time.time;

        direction = new Vector3();
        if (blocktype == 0)
        {
            direction = Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, transform.parent.eulerAngles.z) * Vector3.forward;
        }

        if (blocktype == 1)
        {
            direction = Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, transform.parent.eulerAngles.z) * Vector3.right;
        }

        if (blocktype == 2)
        {
            direction = -(Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, transform.parent.eulerAngles.z) * Vector3.forward);
        }

        _endPosition = transform.position + direction * distanceToMove;
    }

    void StartLerpingBackwards()
    {
        _isLerping = false;
        _timeStartedLerpingBackwards = Time.time;
        Vector3 direction = new Vector3();

        if (blocktype == 0)
        {
            direction = -(Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, transform.parent.eulerAngles.z) * Vector3.forward);
        }

        if (blocktype == 1)
        {
            direction = -(Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, transform.parent.eulerAngles.z) * Vector3.right);
        }

        if (blocktype == 2)
        {
            direction = Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, transform.parent.eulerAngles.z) * Vector3.forward;
        }

        _endPosition = transform.position + direction * distanceToMove;
    }
}
