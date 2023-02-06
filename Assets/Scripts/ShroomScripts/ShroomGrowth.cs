using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomGrowth : MonoBehaviour, Interactable
{

    [SerializeField] private Animation anim;
    private int stageIndex = 0;

    [SerializeField] private float timeToGrow = 5f;
    private ShroomGrowthEffectGOL shroomGrowthEffect;
    private float timer;
    private bool isGrowing;


    public void Interact()
    {
        //start growing the shroom, and enable the shroom-growth-effect
        if (!isGrowing)
        {
            anim.Play();
            shroomGrowthEffect.Initialize();
            shroomGrowthEffect.startGrowing = true;
        }
        
    }

    private void Start()
    {
        shroomGrowthEffect = GetComponentInChildren<ShroomGrowthEffectGOL>();
    }

    private void Update()
    {
        
    }

}
