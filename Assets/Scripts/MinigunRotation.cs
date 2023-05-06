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
    bool lightActive = false;

    private void Start()
    {
        ParticleSystem.EmissionModule bulletFireEm = bulletFire.emission;
        ParticleSystem.EmissionModule bulletShellsEm = bulletShells.emission;
    }
    private void Update()
    {
        if (rotationActive)
        {
            if (rotationActual < 1) rotationActual = 1;
            RotateMinigun();
            lightActive = !lightActive;
            gunLight.SetActive(lightActive);

        }
        else
        {
            SlowMinigun();
            lightActive = false;
            gunLight.SetActive(false);
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
