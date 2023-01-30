using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem
{

    private int currentHealth, maxHealth;

    public event EventHandler OnHealthChanged;
    public event EventHandler OnDied;
    

    public HealthSystem(int maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public void DealDamage(int damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(this, EventArgs.Empty);

        if(currentHealth < 0)
        {
            currentHealth = 0;
            OnDied?.Invoke(this, EventArgs.Empty);
        }
    }

    public void GainHealth(int healthAmount)
    {
        currentHealth += healthAmount;
        OnHealthChanged?.Invoke(this, EventArgs.Empty);

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

}
