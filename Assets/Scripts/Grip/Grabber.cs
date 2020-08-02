using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    public ConfigurableJoint StrongGrip;//keep track of our jounts 
    public ConfigurableJoint WeakGrip;
    public FixedJoint FixedJoint;

    public List<GameObject> NearObjects = new List<GameObject>();//list of near grabbable objects

    void OnTriggerEnter(Collider other)//when we touch an object check if it has a GrabPoint Script and we can grab then add it to the list if so.
    {
        if (other.GetComponent<GrabPoint>())
        {
            if (other.GetComponent<GrabPoint>().SubGrip && other.transform.parent.GetComponent<Interactable>().gripped)
            {
                NearObjects.Add(other.gameObject);
            }
            else if (!other.GetComponent<GrabPoint>().SubGrip && !other.transform.parent.GetComponent<Interactable>().gripped)
            {
                NearObjects.Add(other.gameObject);
            }
        }
        Debug.Log(NearObjects);
    }

    void OnTriggerExit(Collider other)//remove items from the list
    {
        if (other.GetComponent<GrabPoint>())
        {
            NearObjects.Remove(other.gameObject);
        }
    }
    public GameObject ClosestGrabbable()//cheak the list and return the closest grabbable object.
    {
        GameObject ClosestGameObj = null;
        float Distance = float.MaxValue;
        if (NearObjects != null)
        {
            foreach (GameObject GameObj in NearObjects)
            {
                if ((GameObj.transform.position - transform.position).sqrMagnitude < Distance)
                {
                    ClosestGameObj = GameObj;
                    Distance = (GameObj.transform.position - transform.position).sqrMagnitude;
                }
            }
        }
        return ClosestGameObj;
    }
}