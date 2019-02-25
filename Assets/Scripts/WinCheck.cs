using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WinCheck : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "astronaut")
        {
            ExperienceManager.Instance.OnSuccess();
        }
    }
}
