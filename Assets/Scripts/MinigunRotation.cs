using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunRotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    float rotationActual = 0.05f;
    [SerializeField] bool rotationActive;
    [SerializeField] ParticleSystem bulletFire;
    [SerializeField] ParticleSystem bulletShells;
    [SerializeField] GameObject gunLight;
    AudioSource playerAudio;
    bool lightActive = false;

    private void Start()
    {
        playerAudio = gameObject.transform.root.GetComponent<AudioSource>();
        ParticleSystem.EmissionModule bulletFireEm = bulletFire.emission;
        ParticleSystem.EmissionModule bulletShellsEm = bulletShells.emission;
    }
    private void Update()
    {
        if (rotationActive)
        {
            if (rotationActual < 1) rotationActual = 1;
            RotateMinigun();
        }
        else
        {
            SlowMinigun();
            lightActive = false;
            gunLight.SetActive(false);
            if (playerAudio.isPlaying)
                playerAudio.Stop();
        }

        transform.Rotate(0, 0, rotationActual);
    }
    public void ActivateRotation(bool active)
    {
        rotationActive = active;
    }
    void RotateMinigun()
    {
        if (rotationActual < rotationSpeed)
        {
            rotationActual *= 1.15f;
        }
        else
        {
            StartEffects();
            gunLight.SetActive(lightActive);
            lightActive = !lightActive;
            if (!playerAudio.isPlaying)
            {
                playerAudio.Play();
            }
        }

    }
    void SlowMinigun()
    {
        StopEffects();
        if (rotationActual > 0)
        {
            rotationActual--;
        }
    }
    void StartEffects()
    {
        if (bulletFire.isPlaying == false)
        {
            bulletFire.Play();
        }
        else
        {
            if (!bulletFire.emission.enabled)
            {
                ParticleSystem.EmissionModule bulletEm = bulletFire.emission;
                bulletEm.enabled = true;
            }
        }
        if (bulletShells.isPlaying == false)
        {
            bulletShells.Play();
        }
        else
        {
            if (!bulletShells.emission.enabled)
            {
                ParticleSystem.EmissionModule bulletShellsEm = bulletShells.emission;
                bulletShellsEm.enabled = true;
            }
        }
    }
    void StopEffects()
    {
        if (bulletFire.emission.enabled)
        {
            ParticleSystem.EmissionModule bulletEm = bulletFire.emission;
            bulletEm.enabled = false;
        }
        if (bulletShells.emission.enabled)
        {
            ParticleSystem.EmissionModule bulletShellsEm = bulletShells.emission;
            bulletShellsEm.enabled = false;
        }
    }
}
