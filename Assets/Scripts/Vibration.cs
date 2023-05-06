using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibration : MonoBehaviour
{
    bool isVibrating;
    [SerializeField] float vibrateSpeed = 60f;
    [SerializeField] float intensity = .2f;

    void Update()
    {
        if (!isVibrating) return;
        transform.localPosition = intensity * new Vector3(
        Mathf.PerlinNoise(vibrateSpeed * Time.time, 1),
        Mathf.PerlinNoise(vibrateSpeed * Time.time, 2),
        Mathf.PerlinNoise(vibrateSpeed * Time.time, 3));

    }
    public void Vibrate(bool active)
    {
        isVibrating = active;
        if (active)
        {
            StartCoroutine(Timer(2));
        }
    }

    IEnumerator Timer(float vibrateTime)
    {
        yield return new WaitForSeconds(vibrateTime);
        isVibrating = false;
        yield return new WaitForSeconds(.5f);
        GetComponent<Animator>().SetTrigger("OpenDoors");
    }
}
