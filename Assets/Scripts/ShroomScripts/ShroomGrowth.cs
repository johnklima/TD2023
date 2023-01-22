using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomGrowth : MonoBehaviour, Interactable
{

    [SerializeField] private Transform[] shroomStages;
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
            isGrowing = true;
            shroomGrowthEffect.Initialize();
            shroomGrowthEffect.startGrowing = true;
        }
        
    }


    private void Start()
    {
        timer = timeToGrow;
        shroomGrowthEffect = GetComponentInChildren<ShroomGrowthEffectGOL>();
    }

    private void Update()
    {
        //testing purpose, remember to remove
        if (Input.GetKeyDown(KeyCode.T))
        {
            isGrowing = true;
            shroomGrowthEffect.Initialize();
            shroomGrowthEffect.startGrowing = true;
        }

        if (isGrowing)
        {
            timer -= Time.deltaTime;
            if (timer < 0f)
            {
                Grow();
                timer = timeToGrow;
            }
        }
    }

    private void Grow()
    {
        //disable the early object and enable another object (for now)
        shroomStages[stageIndex].gameObject.SetActive(false);
        shroomStages[stageIndex + 1].gameObject.SetActive(true);
        stageIndex++;
        if (stageIndex == shroomStages.Length - 1)
            isGrowing = false;
    }

}
