using UnityEngine;
using Valve.VR;
public class Interactable : MonoBehaviour
{
    public int touchCount;

    //keep track of the active/first hand
    public SteamVR_Input_Sources Hand;

    public bool gripped;
    public bool SecondGripped;

    void Start()
    {
        if (gameObject.tag != "Block")
        {
            Debug.LogError("Interactable's tag is not set to block");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //touchCount++;
    }
    private void OnCollisionExit(Collision collision)
    {
        //touchCount--;
    }
}
