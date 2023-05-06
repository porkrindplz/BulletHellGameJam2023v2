using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIWaveText : MonoBehaviour
{
    TextMeshProUGUI text;
    WaveManager waveManager;
    string additionalText;
    int currentWaveNo = -1;


    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        waveManager = GameObject.Find("Enemy").GetComponent<WaveManager>();
        text.text = "";

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad > 1)
        {
            if (waveManager.GetWaveNum() == 0)
            {
                additionalText = ": Here we go, wavebros";
            }
            else if (waveManager.GetWaveNum() == 1)
            {
                additionalText = ": Not too bad, wavebros";
            }
            else if (waveManager.GetWaveNum() == 2)
            {
                additionalText = ": Don't get cocky, wavebros";
            }
            else if (waveManager.GetWaveNum() == 3)
            {
                additionalText = ": Uhhh, wavebros, we got too cocky";
            }
            else additionalText = "";
            if (waveManager.GetWaveNum() > currentWaveNo)
            {
                currentWaveNo = waveManager.GetWaveNum();
            }
        }
    }
    IEnumerator ShowNewWaveText()
    {
        if (waveManager.GetWaveNum() < waveManager.GetWaveTotal())
        {
            text.text = "Wave " + waveManager.GetWaveNum().ToString() + additionalText;
        }
        else
        {
            text.text = "Engage!";
        }
        yield return new WaitForSeconds(4);
        text.text = "";
    }
    public void StartDisplay()
    {
        StartCoroutine(ShowNewWaveText());
    }
}
