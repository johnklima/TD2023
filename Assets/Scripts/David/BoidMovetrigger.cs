using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidMovetrigger : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    bool inTrigger;

    float distance = 80f;
    BoidsDavid[] boids;

    [SerializeField] SphereCollider thisColl;
    
    void Start()
    {
      boids = transform.GetComponentsInChildren<BoidsDavid>();
    }
    void OnTriggerEnter(Collider collider)
    {
        for (int i = 0; i < boids.Length; i++)
        {  
          inTrigger = Physics.CheckSphere(transform.position, distance, layerMask);

          if (inTrigger)
          {
            boids[i].velocity = new Vector3(0, -0.5f, 0);

            thisColl.enabled = false;
          }
        }

    }
}
