using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyDisplay : MonoBehaviour
{
    Slider EnergySlider;
    EnergyScript player;
    
    //get the Slider component
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<EnergyScript>();
        EnergySlider = GetComponent<Slider>();
        {
            EnergySlider.maxValue = player.MaxEnergy();
            EnergySlider.value = player.MaxEnergy();
        }

    }
    
    void Update()
    {
        EnergySlider.value = player.Energy();
    }

    //Set the Slider's max Values
   // public void SetMaxEnergy(float MaxEnergy)
   // {
        //EnergySlider.maxValue = MaxEnergy;
        //EnergySlider.value = MaxEnergy;
   // }
   // public void SetEnergy(float Energy)
   // { 
     //   EnergySlider.value = Energy;
   // }

}
