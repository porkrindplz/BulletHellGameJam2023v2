using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    Health player;
    void Start()
    {
        if (GameObject.Find("Player").GetComponent<Health>() != null)
            player = GameObject.Find("Player").GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = player.GetHealth().ToString();
    }
}
