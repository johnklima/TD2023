using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public Gradient gradient;

    public Image fill;
    public int healthTest;
    

    private void Start()
    {
        SetMaxHealth(Player.Instance.healthSystem.GetHealth());
        Player.Instance.healthSystem.OnDamageTaken += _OnDamageTaken;
        Player.Instance.healthSystem.OnHealthGain += _OnHealthGain;
    }
    
    private void _OnDamageTaken(object sender, EventArgs e)
    {
        SetHealth(Player.Instance.healthSystem.GetHealth());
    }

    private void _OnHealthGain(object sender, EventArgs e)
    {
        SetHealth(Player.Instance.healthSystem.GetHealth());
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }


    public void SetHealth(int amount)
    {
        slider.value = amount;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
