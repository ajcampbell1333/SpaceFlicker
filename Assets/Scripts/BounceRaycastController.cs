using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;



public class BounceRaycastController : BaseRaycast
{
    protected override Vector3 Position
    {
        get
        {
            return transform.position;
        }
    }

    protected override Vector3 Direction
    {
        get
        {
            return transform.forward;
        }
    }

    protected override Vector3 Up
    {
        get
        {
            return transform.up;
        }
    }
}
