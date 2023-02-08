using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public static PlayerAnimations Instance { get; private set; }

    private Animator animator;
    [SerializeField] private CharacterMovement characterMovement;
    private bool isDead, startLandingProcess;


    private void Awake()
    {
        Instance = this;

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDead) return;

        animator.SetBool("IsWalking", characterMovement.IsMoving()); //if IsMoving() then set is moving bool to true
        animator.SetBool("IsRunning", characterMovement.IsRunning()); // if IsRunning() then set is running bool to true
        animator.SetBool("IsJumping", characterMovement.IsJumping());

        animator.SetBool("InAir", !characterMovement.IsGrounded());
        animator.SetBool("IsJumping", characterMovement.PerformedJump());

        if (startLandingProcess)
        {
            if (characterMovement.IsGrounded())
            {
                animator.SetBool("IsLanding", true);
                startLandingProcess = false;
                Invoke("ResetLanding", 1.5f);
            }
        }


    }

    public void PlayDeathAnim()
    {
        animator.SetBool("IsDead", true);
        isDead = true;
    }

    private void ResetLanding()
    {
        animator.SetBool("IsLanding", false);
    }

    public void PlayAimAnim(bool isAiming)
    {
        animator.SetBool("IsShooting", isAiming);
    }

    public void InitLanding()
    {
        startLandingProcess = true;
    }

    private void ResetShootingAnim()
    {
        animator.SetBool("IsShooting", false);
    }

}
