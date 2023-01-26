using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidMovetrigger : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    bool inTrigger;

    float distance = 10f;
    Boids[] boids;

    [SerializeField] float timer = 0f;

    void Start()
    {
        boids = transform.GetComponentsInChildren<Boids>();
    }
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider collider)
    {
        for (int i = 0; i < boids.Length; i++)
        {  
          inTrigger = Physics.CheckSphere(transform.position, distance, layerMask);

          if (inTrigger)
          {
            inTrigger = true;
            boids[i].velocity = new Vector3(0, 0, 1);
            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                timer -= Time.deltaTime;
                timer = 0f;
                boids[i].velocity -= new Vector3(0, 0, 1);
            }
          }
        }

    }
}
