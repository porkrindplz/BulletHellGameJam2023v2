using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSong : MonoBehaviour
{
    bool musicOver;
    [SerializeField] AudioSource song1, song2;
    private void Update()
    {
        if (!song1.isPlaying && !song2.isPlaying && !musicOver)
        {
            song2.Play();
            musicOver = true;
        }
    }
}
