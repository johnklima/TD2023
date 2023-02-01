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
       
    
    FMOD.Studio.EventInstance e;
    FMOD.Studio.EventInstance ewalk;


    public bool isInCombat = false;
    public float  isOnPebbles = 0;
    public float isOnForest = 0;
    public float isOnDirt = 0;

    private Vector3 prevPos;

    private void Awake()
    {
        e = FMODUnity.RuntimeManager.CreateInstance("event:/Richard'sCave3Music_Attempt001");
        e.start();
       
        ewalk = FMODUnity.RuntimeManager.CreateInstance("event:/Footstep");
        ewalk.start();
        ewalk.setPaused(true);


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

        if(Vector3.Distance(transform.position, prevPos) > 0.02f)
        {
            
            prevPos = transform.position;

            
            ewalk.setPaused(false);

            /* //walking is 2d, no need for this.
            FMOD.ATTRIBUTES_3D structure;
            FMOD.VECTOR pos;

            ewalk.get3DAttributes(out structure);

            pos.x = transform.position.x;
            pos.y = transform.position.y;
            pos.z = transform.position.z;

            structure.position = pos;

            ewalk.set3DAttributes(structure);           
            */

            ewalk.setParameterByName("Dirt", isOnDirt);
            ewalk.setParameterByName("Pebbles", isOnPebbles);
            ewalk.setParameterByName("Forest", isOnForest);

            

        }
        else
        {
            ewalk.setParameterByName("Dirt", 0);
            ewalk.setParameterByName("Pebbles", 0);
            ewalk.setParameterByName("Forest", 0);
            ewalk.setPaused(true);
        }

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

        if(isInCombat)
        {
            e.setParameterByName("Combat", 1.0f);            
        }
        else 
        {
            e.setParameterByName("Combat", 0.0f);
        }

        

    }
}
