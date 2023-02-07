using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomGrowth : MonoBehaviour, Interactable
{

     private Animation anim;
    private int stageIndex = 0;

    [SerializeField] private float timeToGrow = 5f;
    private ShroomGrowthEffectGOL shroomGrowthEffect;
    [SerializeField] private float timer = 15;
    private bool isGrowing;


    public void Interact()
    {
        //start growing the shroom, and enable the shroom-growth-effect
        if (!isGrowing)
        {
            anim.Play();
            shroomGrowthEffect.Initialize();
            gameObject.GetComponent<Collider>().enabled = false;
            shroomGrowthEffect.startGrowing = true;
            isGrowing = true;
        }
        
    }

    private void Start()
    {
        shroomGrowthEffect = GetComponentInChildren<ShroomGrowthEffectGOL>();
        anim = GetComponentInChildren<Animation>();
    }

    private void Update()
    {
        if (isGrowing)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                Debug.Log("done growing");
                ShroomsPlantedManager.Instance.ShroomPlanted();
                isGrowing = false;
            }

        }
    }

}
