using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomsPlantedManager : MonoBehaviour
{
    public static ShroomsPlantedManager Instance { get; private set; }


    private int shroomsPlanted;
    [SerializeField] private int maxShroomsToPlant;

    public event EventHandler OnShroomPlanted;

    private void Awake()
    {
        Instance = this;
    }

    public void ShroomPlanted()
    {
        shroomsPlanted++;
        OnShroomPlanted?.Invoke(this, EventArgs.Empty);
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
