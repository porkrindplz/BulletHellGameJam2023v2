using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFollowGunRotation : MonoBehaviour
{
    [SerializeField] Transform target;
    Quaternion initialRotation;


    private void Start()
    {
        initialRotation = Quaternion.Inverse(target.rotation) * transform.rotation;
    }

    private void LateUpdate()
    {
        Quaternion targetRotation = target.rotation * initialRotation;
        transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
    }
}
