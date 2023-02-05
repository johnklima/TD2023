using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public HealthSystem healthSystem;
    [SerializeField] private int maxHealth = 1;

    [SerializeField] private string deathAnimBoolParatmeter;

    private Animator animator;

    private float timer = 2f;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        healthSystem = new HealthSystem(maxHealth);
        healthSystem.OnDied += _OnDied;
    }
    
    private void _OnDied(object sender, EventArgs e)
    {
        animator.SetBool(deathAnimBoolParatmeter, true);
        isDead = true;
    }

    private void Update()
    {

        if (isDead)
        {
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                gameObject.SetActive(false);
            }
        }

    }

}
