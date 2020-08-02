using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

namespace Photon.Pun.UtilityScripts
{  
    public class CopyScript : MonoBehaviourPunCallbacks
    {
        public int index;
        void Update()
        {
            if (photonView.IsMine)
            {
                switch (index)
                {
                    case 1:
                        transform.position = VRManager.instance.head.transform.position;
                        transform.rotation = VRManager.instance.head.transform.rotation;
                        break;
                    case 2:
                        transform.position = VRManager.instance.leftHand.transform.position;
                        transform.rotation = VRManager.instance.leftHand.transform.rotation;
                        break;
                    case 3:
                        transform.position = VRManager.instance.rightHand.transform.position;
                        transform.rotation = VRManager.instance.rightHand.transform.rotation;
                        break;
                }
            }
        }
    }
}
