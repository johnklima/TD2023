using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeAvoidance : MonoBehaviour
{
    // private BoidSpore spore;
    private Transform spore;
    private BoidSpore boidSpore;

    [SerializeField] LayerMask avoidMask;
    Collider[] hitObj;
    bool didHit;
    float distance = 1f;

    private void Start()
    {
        spore = transform.parent;
        boidSpore = spore.GetComponent<BoidSpore>();
    }

    void Update()
    {
        didHit = Physics.CheckSphere(transform.position, distance, avoidMask);
        hitObj = Physics.OverlapSphere(transform.position, distance, avoidMask);

        if (didHit)
        {
            for (int i = 0; i < hitObj.Length; i++)
            {
                boidSpore.accumAvoid(hitObj[i].transform.position);
            }
        }
    }
}
