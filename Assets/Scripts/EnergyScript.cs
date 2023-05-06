using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyScript : MonoBehaviour
{
    
    
    [SerializeField] float currentEnergy = 100f;
    [SerializeField]  float currentMaxEnergy = 100f;
    

    // Properties
    public float Energy()
    {
        return currentEnergy;
    }
    
    public float MaxEnergy()
    {
     return currentMaxEnergy;
    }
    
    //Constructor
    //public EnergyScript(float Energy, float MaxEnergy)
    //{
     // currentEnergy = Energy;
    //  currentMaxEnergy = MaxEnergy;
    //}
    
    //Method for draining Energy
    public void UseEnergy (float EnergyAmount)
    {
        if (currentEnergy > 0) 
        {
            currentEnergy -= EnergyAmount * Time.deltaTime;
        }

    }
}
