using UnityEngine;
using Valve.VR;

public class GripController : MonoBehaviour
{
    public SteamVR_Input_Sources Hand;//Get all of our inputs
    public SteamVR_Action_Boolean ToggleGripButton;
    public SteamVR_Action_Pose position;
    public SteamVR_Behaviour_Skeleton HandSkeleton;//I had two hands connected two one, one hand was just an outline so I could use it as a indication of a grabbable object.
    public SteamVR_Behaviour_Skeleton PreviewSkeleton;
    public Grabber grabber;//get our grabber prefab we made earlier

    private GameObject ConnectedObject;//The object we a currently holding
    private Transform OffsetObject;//used to stor our Grip point prefabs
    private bool SecondGrip;//are we the second hand to grip the object
    private void Update()
    {
        if (ConnectedObject != null)
        {
            Debug.Log(ConnectedObject.name);
        }
        
        if (ConnectedObject != null)//If we are holding something
        {
            if (!SecondGrip)//and we are not the second hand
            {


                if (ConnectedObject.GetComponent<Interactable>().touchCount == 0 && !ConnectedObject.GetComponent<Interactable>().SecondGripped)//If the held object isn't touching anything 
                {
                    grabber.FixedJoint.connectedBody = null;//disconnect the rigid jont to reset it
                    //grabber.StrongGrip.connectedBody = null;//and all the other joints just to be sure
                    //these two lines move the connected object to the hands position over time to give a snappy feel to picking stuff up, otherwise you could just teleport the object to the right position

                    ConnectedObject.transform.position = Vector3.MoveTowards(ConnectedObject.transform.position, transform.position - (ConnectedObject.transform.rotation * OffsetObject.GetComponent<GrabPoint>().Offset), .1f);//move and rotate the object to the hands position 
                    ConnectedObject.transform.rotation = Quaternion.RotateTowards(ConnectedObject.transform.rotation, transform.rotation * Quaternion.Inverse(OffsetObject.localRotation), 10);
                    grabber.FixedJoint.connectedBody = ConnectedObject.GetComponent<Rigidbody>();//reconnect the rigid joint

                }
                else if (ConnectedObject.GetComponent<Interactable>().touchCount > 0 || ConnectedObject.GetComponent<Interactable>().SecondGripped)//the object is touching something so use a configurable joint.
                {

                    grabber.FixedJoint.connectedBody = null;
                    //grabber.StrongGrip.connectedAnchor = OffsetObject.GetComponent<GrabPoint>().Offset;
                    //grabber.StrongGrip.connectedBody = ConnectedObject.GetComponent<Rigidbody>();

                }

            }
            else if (SecondGrip)// if we are the second hand just use the weak grip that doesnt have an agular drive
            {

                if (!ConnectedObject.GetComponent<Interactable>().gripped)//if the other hand lets go use a configurable joint
                {
                    grabber.FixedJoint.connectedBody = null;
                    //grabber.StrongGrip.connectedAnchor = OffsetObject.GetComponent<GrabPoint>().Offset;
                    //grabber.StrongGrip.connectedBody = ConnectedObject.GetComponent<Rigidbody>();
                }
                else
                {
                    grabber.FixedJoint.connectedBody = null;
                    //grabber.StrongGrip.connectedBody = null;
                    //grabber.WeakGrip.connectedBody = ConnectedObject.GetComponent<Rigidbody>();
                }
            }
            if (ToggleGripButton.GetStateUp(Hand))//check for if we want to let go
            {
                Release();
            }
            if (PreviewSkeleton)//hide the preview hand since we are holding somthing 
                PreviewSkeleton.transform.gameObject.SetActive(false);
        }
        else//if we arn't holding anything
        {
            if (grabber.ClosestGrabbable() && PreviewSkeleton)//if there is somthing close and we have a preview hand assigned 
            {
                PreviewSkeleton.transform.gameObject.SetActive(true);//activate the hand 
                OffsetObject = grabber.ClosestGrabbable().transform;//parent it to the object
                if (grabber.ClosestGrabbable().GetComponent<SteamVR_Skeleton_Poser>())//if the object has a skeleton poser 
                {
                    if (!OffsetObject.GetComponent<GrabPoint>().SubGrip && !OffsetObject.transform.parent.GetComponent<Interactable>().gripped || OffsetObject.GetComponent<GrabPoint>().SubGrip && OffsetObject.transform.parent.GetComponent<Interactable>().gripped)//if the object is not gripped at all or is gripped by both hands
                    {
                        PreviewSkeleton.transform.SetParent(OffsetObject, false);
                        PreviewSkeleton.BlendToPoser(OffsetObject.GetComponent<SteamVR_Skeleton_Poser>(), 0f);
                    }
                }
            }
            else//if there is no object in range deactivate the hand
            {
                PreviewSkeleton.transform.gameObject.SetActive(false);
            }
            if (ToggleGripButton.GetStateDown(Hand))//check for if we want to grip
            {
                Grip();
            }
        }
    }
    private void Grip()
    {
        GameObject NewObject = grabber.ClosestGrabbable();//get the closest grabbable 
        if (NewObject != null)//if it is defined
        {
            OffsetObject = grabber.ClosestGrabbable().transform;//set the offset
            ConnectedObject = OffsetObject.transform.parent.gameObject;//set the root of the grabbable as the connected object
            ConnectedObject.GetComponent<Rigidbody>().useGravity = false;
            ConnectedObject.GetComponent<Block>().PickedUp();
            ConnectedObject.GetComponent<Rigidbody>().mass = 1;

            if (ConnectedObject.GetComponent<Interactable>().gripped)//if the object has already been grabbed (we know that it's not a grip point that has already been grabbed since those arn't put in the grabbable array)
            {
                SecondGrip = true;
                ConnectedObject.GetComponent<Interactable>().SecondGripped = true;
                grabber.WeakGrip.connectedBody = ConnectedObject.GetComponent<Rigidbody>();//attach the correct joint.
                grabber.WeakGrip.connectedAnchor = OffsetObject.localPosition;//set the offest 
            }
            else//if we are the first to grab it 
            {
                ConnectedObject.GetComponent<Interactable>().Hand = Hand;
                ConnectedObject.GetComponent<Interactable>().gripped = true;
            }
            if (OffsetObject.GetComponent<SteamVR_Skeleton_Poser>() && HandSkeleton)//if the object has a defined skeleton poser update our hand
            {
                HandSkeleton.transform.SetParent(OffsetObject, false);
                HandSkeleton.BlendToPoser(OffsetObject.GetComponent<SteamVR_Skeleton_Poser>(), 0f);
            }


        }
    }
    private void Release()
    {
        grabber.FixedJoint.connectedBody = null;//disconnect everything
        //grabber.StrongGrip.connectedBody = null;
        //grabber.WeakGrip.connectedBody = null;
        ConnectedObject.GetComponent<Rigidbody>().velocity = position.GetVelocity(Hand);
        ConnectedObject.GetComponent<Rigidbody>().angularVelocity = position.GetAngularVelocity(Hand);//set the rotational velocitiy too
        ConnectedObject.GetComponent<Rigidbody>().useGravity = true;
        ConnectedObject.GetComponent<Rigidbody>().mass = 50;
        ConnectedObject.GetComponent<Block>().Dropped(); 
        if (!SecondGrip)//if we were the first to grab the object
        {

            ConnectedObject.GetComponent<Interactable>().gripped = false;

        }
        else//if we were  the second hand
        {
            ConnectedObject.GetComponent<Interactable>().SecondGripped = false;
            SecondGrip = false;
        }

        ConnectedObject = null;
        if (OffsetObject.GetComponent<SteamVR_Skeleton_Poser>() && HandSkeleton)//disconnect the hand if needed.
        {
            HandSkeleton.transform.SetParent(transform, false);
            HandSkeleton.BlendToSkeleton();
        }

        OffsetObject = null;
    }
}
