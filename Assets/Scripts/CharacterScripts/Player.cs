using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private int maxHealth;

    public HealthSystem healthSystem;

    private void Awake()
    {
        Instance = this;

        healthSystem = new HealthSystem(maxHealth);

        healthSystem.OnDied += _OnDied;
    }
    
    // Start is called before the first frame update
    void Start()
    {
    }

    private void _OnDied(object sender, EventArgs e)
    {
        //activate some UI stuff / respawn thing
        Debug.Log("You dead");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            healthSystem.DealDamage(10);
            Debug.Log(healthSystem.GetHealth());
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            healthSystem.GainHealth(10);
            Debug.Log(healthSystem.GetHealth());
        }

    }
}
