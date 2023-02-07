using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;


public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private int maxHealth;

    public HealthSystem healthSystem;
    [SerializeField] private HealthBar healthBar;
     
    [SerializeField] private Transform gameOverUI;
    
   
    private void Awake()
    {
        Instance = this;

        healthSystem = new HealthSystem(maxHealth);

        healthSystem.OnHealthChanged += _OnHealthChanged;
        healthSystem.OnDied += _OnDied;

    }

    private void Start()
    {
        if (healthBar != null)
            healthBar.SetMaxHealth(maxHealth);

    }

    private void _OnHealthChanged(object sender, EventArgs e)
    {
        if (healthBar != null)
            healthBar.SetHealth(healthSystem.GetHealth());
    }
    

    private void _OnDied(object sender, EventArgs e)
    {
        if(gameOverUI != null)
        {
            gameObject.GetComponent<CharacterMovement>().enabled = false;
            PlayerAnimations.Instance.PlayDeathAnim();
            Invoke("ActivateGameOverUI", 3f);
        }
    }

    private void ActivateGameOverUI()
    {
        gameOverUI.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

}
