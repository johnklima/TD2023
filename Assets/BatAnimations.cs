using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAnimations : MonoBehaviour
{

    Animator animator;
    [SerializeField] Boids boid;

    string isFlying = "isFlying";
    string isAttacking = "isAttacking";



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool(isFlying, true);


    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            animator.SetBool(isFlying, false);
            animator.SetBool(isAttacking, true);
            boid.isFlocking = false;

        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            animator.SetBool(isFlying, true);
            animator.SetBool(isAttacking, false);

        }
    }
}
