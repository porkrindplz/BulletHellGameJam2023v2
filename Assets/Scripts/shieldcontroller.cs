using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class shieldcontroller : MonoBehaviour
{
    EnergyScript player;

    //Where to put the Energy UI
    [SerializeField] EnergyDisplay _EnergyDisplay;

    // Assign the shield prefab to this field in the Inspector.
    public GameObject shieldPrefab;

    // The current instance of the shield object.
    private GameObject currentShield;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<EnergyScript>();
    }


    private void Update()
    {
        // Check if the space key is being held down.
        if (Input.GetKey(KeyCode.Space) && player.Energy() > 0)
        {
            // If there isn't a shield active, create a new one.
            if (currentShield == null)
            {
                currentShield = Instantiate(shieldPrefab, transform.position, transform.rotation);
            }
            PlayerUseEnergy(10f);
        }
        else if (player.Energy() <= 0)
        {
            Destroy(currentShield);
            currentShield = null;
        }
        else
        {
            // If the space key is not being held down, destroy the shield.
            if (currentShield != null)
            {
                Destroy(currentShield);
                currentShield = null;
            }
        }


    }


    //Add this to Player Behavior
    private void PlayerUseEnergy(float EnergyAmount)
    {
        player.UseEnergy(EnergyAmount);
    }


}
