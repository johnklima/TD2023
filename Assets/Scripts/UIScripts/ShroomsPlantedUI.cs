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

        shroomsPlantedText = GetComponentInChildren<TextMeshProUGUI>();

        if (ShroomsPlantedManager.Instance != null)
        {
            ShroomsPlantedManager.Instance.OnShroomPlanted += _OnShroomPlanted;
            maxShroomsToPlant = ShroomsPlantedManager.Instance.GetMaxShroomsToPlant();
        }

        shroomsPlantedText.SetText("0/" + maxShroomsToPlant);
    }

    private void _OnShroomPlanted(object sender, EventArgs e)
    {
        shroomsPlantedText.SetText(ShroomsPlantedManager.Instance.GetShroomsPlanted() + " / " + maxShroomsToPlant);
    }

}
