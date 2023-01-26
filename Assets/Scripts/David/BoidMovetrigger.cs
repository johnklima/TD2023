using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidMovetrigger : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    bool inTrigger;
    [SerializeField] SphereCollider thisColl;

    float distance = 10f;
    Boids[] boids;

    void Start()
    {
      boids = transform.GetComponentsInChildren<Boids>();
    }
    void OnTriggerEnter(Collider collider)
    {
        for (int i = 0; i < boids.Length; i++)
        {  
          inTrigger = Physics.CheckSphere(transform.position, distance, layerMask);

          if (inTrigger)
          {
            inTrigger = true;
            boids[i].velocity = new Vector3(0, -0.5f, 0);

            thisColl.enabled = false;
          }
        }

    }
}
