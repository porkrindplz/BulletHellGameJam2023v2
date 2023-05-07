using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    [SerializeField] float lineWidth = 0.5f;
    [SerializeField] float growthSpeed;
    [SerializeField] float maxRange;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float damagePerInterval = 1f;
    [SerializeField] float dealDamageInterval = 0.5f;
    [SerializeField] GameObject lazerCollisionEffect;
    [SerializeField] GameObject lazerStartEffect;
    [SerializeField] GameObject lazerFadeEffectPrefab;
    [SerializeField] float targetDelay = 2f;
    [SerializeField] GameObject lazerLight;
    Vector3 targetPosition;
    float dealDamageCountdown = 0f;
    LineRenderer line = null;
    RaycastHit hit;
    float currentLength = 0;
    bool charged = false;




    private void Start()
    {

    }
    private void OnEnable()
    {
        line = GetComponent<LineRenderer>();
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);
        transform.LookAt(GameObject.Find("Player").transform);
        StartCoroutine(ChargeUp(targetDelay));
        if (lazerLight != null)
        {
            lazerLight.SetActive(true);
            lazerLight.transform.position = transform.position;
        }
    }

    void OnDisable()
    {
        //Instantiate(lazerFadeEffectPrefab, hit.point, Quaternion.identity);
        StopAllCoroutines();
        if (lazerLight != null) lazerLight.SetActive(false);
        charged = false;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);
        line = null;
        currentLength = 0;
        dealDamageCountdown = 0;
        lazerCollisionEffect.GetComponent<ParticleSystem>().Stop();

    }

    private void Update()
    {
        if (line != null && charged)
        {
            line.SetPosition(0, transform.position);
            currentLength += growthSpeed * Time.deltaTime;
            if (Physics.SphereCast(transform.position, lineWidth * .75f, transform.forward, out hit, currentLength, targetLayer))
            {
                if (charged && !lazerCollisionEffect.GetComponent<ParticleSystem>().isPlaying)
                {
                    lazerCollisionEffect.GetComponent<ParticleSystem>().Play();
                }
                lazerCollisionEffect.transform.position = hit.point;
                if (dealDamageCountdown == 0f)
                {
                    if (hit.transform.root.GetComponent<Health>() != null)
                        hit.transform.root.GetComponent<Health>().DealDamage(damagePerInterval);
                }
                dealDamageCountdown += Time.deltaTime;
                if (dealDamageCountdown >= dealDamageInterval)
                {
                    dealDamageCountdown = 0f;
                }
                // else if (hit.collider.gameObject.GetComponent<DamageOnTrigger>() != null)
                // {
                //     if (dealDamageCountdown == 0f)
                //     {
                //         Destroy(hit.collider.gameObject);
                //     }
                //     dealDamageCountdown += Time.deltaTime;
                //     if (dealDamageCountdown == dealDamageInterval)
                //     {
                //         dealDamageCountdown = 0f;
                //     }
                // }
                line.SetPosition(1, hit.point);
                if (lazerLight != null) lazerLight.transform.position = hit.point;

            }
            else
            {
                Vector3 currentPos = transform.position + transform.forward * currentLength;
                line.SetPosition(1, currentPos);
                if (lazerLight != null) lazerLight.transform.position = currentPos;
                dealDamageCountdown = 0f;
                lazerCollisionEffect.GetComponent<ParticleSystem>().Stop();
            }
            currentLength = Mathf.Clamp(currentLength, 0f, maxRange);
        }
    }
    IEnumerator ChargeUp(float chargeTime)
    {
        yield return new WaitForSeconds(chargeTime);
        charged = true;

    }
}

