using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITimerDisplay : MonoBehaviour
{

    TextMeshProUGUI text;
    TimeSpan timeSpan;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        timeSpan = TimeSpan.FromSeconds(time);
        text.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }
}
