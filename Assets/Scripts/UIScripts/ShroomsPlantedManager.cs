using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomsPlantedManager : MonoBehaviour
{
    public static ShroomsPlantedManager Instance { get; private set; }


    private int shroomsPlanted;
    [SerializeField] private int maxShroomsToPlant;
    [SerializeField] private Transform victoryUI;

    public event EventHandler OnShroomPlanted;

    private void Awake()
    {
        Instance = this;
    }

    public void ShroomPlanted()
    {
        Debug.Log("another shroomy planted");
        shroomsPlanted++;
        OnShroomPlanted?.Invoke(this, EventArgs.Empty);
        if(shroomsPlanted == maxShroomsToPlant)
        {
            Cursor.lockState = CursorLockMode.Locked;
            victoryUI.gameObject.SetActive(true);
        }
    }

    public int GetMaxShroomsToPlant()
    {
        return maxShroomsToPlant;
    }

    public int GetShroomsPlanted()
    {
        return shroomsPlanted;
    }
}
