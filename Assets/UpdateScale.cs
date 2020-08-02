using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateScale : MonoBehaviour
{
    void Update()
    {
        if (transform.parent.tag == "GrabHandle")
        {
            transform.localScale = new Vector3(.5f, .5f, .5f);
        } else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
