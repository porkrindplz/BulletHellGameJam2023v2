using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MissileLauncher : MonoBehaviour
{
    [SerializeField] bool controlFireHere = true;
    [SerializeField] GameObject[] firePoints;
    public GameObject wildMissilePrefab;
    public GameObject spiralMissilePrefab;
    public GameObject straightMissilePrefab;
    GameObject activeMissile;
    [SerializeField] float fireInterval;
    [Header("Lazer")]
    [SerializeField] GameObject lazerPrefab;
    [SerializeField] GameObject lazerFirePoint;
    [SerializeField] float lazerInterval;
    [SerializeField] float lazerDuration;
    [SerializeField] UIInGameDialogue dialogueBox;
    [SerializeField] CharacterDialogue[] lazerDialogue;
    List<CharacterDialogue> randomLazerDialogue = new List<CharacterDialogue>();
    GameObject activeLazer;
    public MissileType missileType;
    public Transform target;
    [Header("MainEnemy Arrival Controls")]
    [SerializeField] Vector3 arrivalLocation = new Vector3(15, 25, 75);
    [SerializeField] float arrivalSpeed;
    bool arrived = true;
    bool coolingDown;

    public enum MissileType
    {
        Wild,
        Spiral,
        Straight,
        None
    }
    private void Awake()
    {
        InitData();
    }
    void InitData()
    {
        foreach (CharacterDialogue dialogue in lazerDialogue)
        {
            dialogue.complete = false;
        }
    }
    private void Start()
    {
        if (gameObject.name == "Enemy")
        {
            arrived = false;
            StartCoroutine(WarpArrival());
        }
    }

    private void Update()
    {
        if (!arrived) return;
        if (missileType == MissileType.None) return;
        if (missileType == MissileType.Wild)
        {
            activeMissile = wildMissilePrefab;
        }
        else if (missileType == MissileType.Spiral)
        {
            activeMissile = spiralMissilePrefab;
        }
        else if (missileType == MissileType.Straight)
        {
            activeMissile = straightMissilePrefab;
        }

        if (coolingDown) return;
        if (!controlFireHere) return;
        StartCoroutine(FireMissile(activeMissile, fireInterval));
        StartCoroutine(FireLazer(lazerInterval, lazerDuration, true));
    }

    IEnumerator FireMissile(GameObject currentMissile, float time)
    {
        coolingDown = true;
        foreach (GameObject firePoint in firePoints)
        {
            GameObject missileObject = Instantiate(currentMissile, firePoint.transform.position, firePoint.transform.rotation);
            missileObject.GetComponent<Missile>().target = target;
            //if (missileType == MissileType.Spiral)
            //{
            yield return new WaitForSeconds(time);
            //}
        }
        coolingDown = false;
    }
    public void Fire()
    {
        foreach (GameObject firePoint in firePoints)
        {
            GameObject missileObject = Instantiate(activeMissile, firePoint.transform.position, firePoint.transform.rotation);
            missileObject.GetComponent<Missile>().target = target;
        }
    }

    public void StartFireLazerCoroutine(int waveNum)
    {
        if (GetComponentInChildren<Animator>() != null)
        {
            GetComponentInChildren<Animator>().SetTrigger("Lazer");
        }
        if (waveNum == 1)
        {
            Debug.Log("Adding Lazer Dialogues");
            dialogueBox.Dialogue(lazerDialogue[0]);
            for (int i = 1; i < lazerDialogue.Length - 1; i++)
            {
                randomLazerDialogue.Add(lazerDialogue[i]);
            }
        }
        else
        {
            int rndDialogue = Random.Range(0, randomLazerDialogue.Count);
            dialogueBox.Dialogue(lazerDialogue[rndDialogue]);
            randomLazerDialogue.Remove(lazerDialogue[rndDialogue]);
            Debug.Log("Removing Lazer Dialogues");
        }
        StartCoroutine(FireLazer(lazerInterval, lazerDuration, false, waveNum));
    }
    public IEnumerator FireLazer(float interval, float duration, bool repeat, int waveNum = -1)
    {
        bool loop = true;
        while (loop == true)
        {
            yield return new WaitForSeconds(interval);
            FireLazer();
            yield return new WaitForSeconds(duration);
            DestroyLazer();
            if (!repeat)
            {
                loop = false;
            }
        }
        if (GetComponent<WaveManager>() != null && waveNum > -1)
        {
            GetComponent<WaveManager>().countdownActive = true;
            Debug.Log("Lazer Complete");
            GetComponent<WaveManager>().WaveActivator(waveNum);
        }
    }

    public void FireLazer()
    {
        if (activeLazer == null)
        {
            activeLazer = Instantiate(lazerPrefab, lazerFirePoint.transform.position, lazerFirePoint.transform.rotation, lazerFirePoint.transform);
        }
        else if (!activeLazer.activeInHierarchy)
        {
            activeLazer.SetActive(true);
        }
    }
    public void DestroyLazer()
    {
        if (activeLazer != null && activeLazer.activeInHierarchy)
        {
            activeLazer.SetActive(false);
        }
    }
    IEnumerator WarpArrival()
    {
        while (transform.position != arrivalLocation)
        {
            transform.position = Vector3.MoveTowards(transform.position, arrivalLocation, arrivalSpeed);
            yield return null;
        }
        arrived = true;
        GetComponentInChildren<Vibration>().Vibrate(true);
        if (GetComponent<WaveManager>() != null)
        {
            GetComponent<WaveManager>().StartFirstWave();
        }
        else
        {
            Debug.Log("Missing WaveManager script on this component!");
        }
    }
}


