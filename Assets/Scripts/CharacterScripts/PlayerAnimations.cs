using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public static PlayerAnimations Instance { get; private set; }

    private Animator animator;
    [SerializeField] private CharacterMovement characterMovement;
    private bool isDead;

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
    }

    public void PlayDeathAnim()
    {
        animator.SetBool("IsDead", true);
        isDead = true;
    }

    public void PlayShootAnim()
    {
        animator.SetBool("IsShooting", true);
        Invoke("ResetShootingAnim", .05f);
    }

    private void ResetShootingAnim()
    {
        animator.SetBool("IsShooting", false);
    }

}
