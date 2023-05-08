using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSong : MonoBehaviour
{
    [SerializeField] AudioSource song1, song2;
    private void Update()
    {
        if (!song1.isPlaying && !song2.isPlaying)
        {
            song2.Play();
        }
    }
}
