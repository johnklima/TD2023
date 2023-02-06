using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShroomsPlantedUI : MonoBehaviour
{

    private TextMeshProUGUI shroomsPlantedText;
    private int maxShroomsToPlant;

    private void Start()
    {
        if (ShroomsPlantedManager.Instance != null)
        {
            ShroomsPlantedManager.Instance.OnShroomPlanted += _OnShroomPlanted;
            maxShroomsToPlant = ShroomsPlantedManager.Instance.GetMaxShroomsToPlant();
        }
    }

    private void _OnShroomPlanted(object sender, EventArgs e)
    {
        shroomsPlantedText.SetText(ShroomsPlantedManager.Instance.GetShroomsPlanted() + " / " + maxShroomsToPlant);
    }

}
