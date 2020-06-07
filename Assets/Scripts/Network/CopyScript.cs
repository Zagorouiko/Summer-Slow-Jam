using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

namespace Photon.Pun.UtilityScripts
{
    public class CopyScript : MonoBehaviourPunCallbacks
    {
        void Update()
        {
            if (photonView.IsMine)
            {
                transform.position = VRManager.instance.head.transform.position;
                transform.rotation = VRManager.instance.head.transform.rotation;
            }
        }
    }
}
