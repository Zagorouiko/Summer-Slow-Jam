using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepMove : MonoBehaviour
{
    public Vector3 jump;
    public float jumpForce;

    public bool isGrounded;
    Rigidbody rb;

    float randNum;
    float speed = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2f, 1.0f);
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }

    void Update()
    {
        if (isGrounded)
        {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            randNum = UnityEngine.Random.Range(0f, 1f);

            if (randNum > .5)
            {
                int randRot = UnityEngine.Random.Range(0, 360);
                Quaternion rot = transform.rotation;
                float randomYRotation = transform.rotation.eulerAngles.y + randRot;

                rot.eulerAngles = new Vector3(0.0f, randomYRotation, 0.0f);

                Vector3 targetAngle = new Vector3(0f, randomYRotation, 0f);
                Vector3 currentAngle = transform.eulerAngles;
                currentAngle = new Vector3( 0, Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime), 0);
                transform.eulerAngles = currentAngle;

                jump = Quaternion.Euler(0, transform.eulerAngles.y, 0) * jump;
                transform.rotation = rot;
            }

            isGrounded = false;          
        }
    }
}
