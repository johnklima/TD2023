using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public HealthSystem healthSystem;
    [SerializeField] private int maxHealth = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        healthSystem = new HealthSystem(maxHealth);
        //healthSystem.OnHealthChanged += _OnHealthChanged;
        healthSystem.OnDied += _OnDied;
    }

    /*private void _OnHealthChanged(object sender, EventArgs e)
    {
        if (healthBar != null)
            healthBar.SetHealth(healthSystem.GetHealth());
        else
            Debug.Log("Healthbar Reference not set");
    }*/
    
    private void _OnDied(object sender, EventArgs e)
    {
        //activate some UI stuff / respawn thing
        gameObject.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
