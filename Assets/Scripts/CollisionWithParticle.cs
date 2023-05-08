using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionWithParticle : MonoBehaviour
{
    ParticleSystem playerShots;

    private void Start()
    {
        playerShots = GameObject.Find("PlayerParticleShots").GetComponent<ParticleSystem>();
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.GetComponent<ParticleSystem>() == playerShots)
        {
            if (gameObject.transform.root.GetComponent<Health>() != null)
            {
                gameObject.transform.root.GetComponent<Health>().DealDamage(1);
            }
            else if (gameObject.GetComponent<Health>() != null) GetComponent<Health>().DealDamage(1);
        }
    }
}
