using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomGrowth : MonoBehaviour, Interactable
{

     private Animation anim;
    private int stageIndex = 0;

    [SerializeField] private float timeToGrow = 5f;
    [SerializeField] private float timer = 15;
    private bool isGrowing;


    public void Interact()
    {
        //start growing the shroom, and enable the shroom-growth-effect
        if (!isGrowing)
        {
            anim.Play();
            gameObject.GetComponent<Collider>().enabled = false;
            //ShroomGrowthEffectGOL.Instance.Initialize();
            ShroomGrowthEffectGOL.Instance.StartGrowing();
            isGrowing = true;
        }
        
    }

    private void Start()
    {
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
                ShroomGrowthEffectGOL.Instance.ResetGOL();
                isGrowing = false;
            }

        }
    }

}
