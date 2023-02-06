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
        else
            Debug.Log("Healthbar Reference not set");
    }
    

    private void _OnDied(object sender, EventArgs e)
    {
        //activate some UI stuff / respawn thing
        Debug.Log("You dead");
    }

    // Update is called once per frame
    void Update()
    {

        
    }
}
