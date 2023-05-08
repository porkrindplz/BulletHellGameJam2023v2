using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    Slider HealthSlider;
    Health player;

    //get the Slider component
    public void Start()
    {

        if (GameObject.Find("Player").GetComponent<Health>() != null)
        {
            HealthSlider = GetComponent<Slider>();
            player = GameObject.Find("Player").GetComponent<Health>();
            HealthSlider.maxValue = player.initHealth;
            HealthSlider.value = player.initHealth;

        }
    }

    void Update()
    {
        HealthSlider.value = player.GetHealth();

    }
}