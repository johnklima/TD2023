using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockTrigger : MonoBehaviour
{

    public FlockFollowPlayer flock;


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            flock.doFollow = true;
        }

    }
}
