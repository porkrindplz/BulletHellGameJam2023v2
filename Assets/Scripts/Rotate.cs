using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    Vector3 rotation;
    private void Start()
    {
        float rndX = Random.Range(0.25f, 0.75f);
        float rndY = Random.Range(0.25f, 0.75f);
        float rndZ = Random.Range(0.25f, 0.75f);
        rotation = new Vector3(rndX, rndY, rndZ);
    }

    private void Update()
    {
        transform.Rotate(rotation);
    }

}
