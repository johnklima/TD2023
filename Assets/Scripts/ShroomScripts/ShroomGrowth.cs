using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomGrowth : MonoBehaviour, Interactable
{

    [SerializeField] private Animation anim;
    private int stageIndex = 0;

    [SerializeField] private float timeToGrow = 5f;
    [SerializeField] private float timer = 15;
    private bool isGrowing;

    ShroomGrowthEffectGOL growthEffGOL; // do we need to set a variable version to something?


    private void Start()
    {
        if(anim == null)
        anim = GetComponentInChildren<Animation>();
    }
    public void Interact()
    {
        //start growing the shroom, and enable the shroom-growth-effect
        if (!isGrowing)
        {
            Debug.Log("Got interacted with");
            anim.Play();
            gameObject.GetComponent<Collider>().enabled = false;
            //ShroomGrowthEffectGOL.Instance.Initialize();
            ShroomGrowthEffectGOL.Instance.StartGrowing();
            isGrowing = true;
        }
        
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
                ShroomGrowthEffectGOL.Instance.ResetGOL();
                isGrowing = false;
            }

        }
    }

}
