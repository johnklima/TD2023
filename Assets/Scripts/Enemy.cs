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

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemyHealthSystem = new HealthSystem(maxHealth);
        enemyHealthSystem.OnDied += _OnDied;
    }
    
    private void _OnDied(object sender, EventArgs e)
    {
        dead = true;
        PlayAnim(animator, isDead, dead);
        StartCoroutine(Timer());
    }
    public void PlayAnim(Animator thisAnimator, string animString, bool boolValue)
    {
        thisAnimator.SetBool(animString, boolValue);
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
    }
}
