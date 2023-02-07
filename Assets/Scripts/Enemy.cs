using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public HealthSystem enemyHealthSystem;
    [SerializeField] private int maxHealth = 1;

    [SerializeField] private string isDead;

    private Animator animator;

    private float timer = 0.5f;

    bool dead;
    
    public FMODUnity.StudioEventEmitter Music;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemyHealthSystem = new HealthSystem(maxHealth);
        enemyHealthSystem.OnDied += _OnDied;
    }

    private void Update()
    {
        if (dead)
        {

             
            Music.SetParameter("Combat", 0);
            timer -= Time.deltaTime;
            if (timer <= 0)
            gameObject.SetActive(false);
        }
    }

    private void _OnDied(object sender, EventArgs e)
    {
        animator.enabled = true;
        dead = true;
        if (animator = GetComponent<Animator>())
        {
          PlayAnim(animator, isDead, dead);
        }
    }

    public void PlayAnim(Animator thisAnimator, string animString, bool boolValue)
    {
        thisAnimator.SetBool(animString, boolValue);
    }

}
