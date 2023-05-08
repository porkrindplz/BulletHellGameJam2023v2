using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class WaveManager : MonoBehaviour
{
    public WaveScriptableObject[] waves;
    [SerializeField] float timeBetweenWaves;
    public Transform[] missileSpawnPoints;
    public Transform[] enemySpawnPoints;
    public bool countdownActive;
    [SerializeField] GameObject dialogueBox;
    Animator anim;

    [Header("Test a Single Wave Below")]
    [SerializeField] bool testSingleWave;
    [SerializeField] WaveScriptableObject testWave;
    int activeWave = 0;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        if (countdownActive)
        {
            if (WaveComplete(activeWave, waves[activeWave].endingTimeBuffer))
            {
                if (activeWave < waves.Length - 1)
                {
                    activeWave++;
                }
                if (!waves[activeWave].waveStarted)
                {
                    StartCoroutine(StartWave(activeWave));
                    waves[activeWave].waveStarted = true;
                    countdownActive = false;
                }
            }
            if (!testSingleWave) waves[activeWave].waveElapsed += Time.deltaTime;
            else testWave.waveElapsed += Time.deltaTime;
        }

    }
    public void StartFirstWave()
    {
        countdownActive = true;
        if (!testSingleWave)
        {
            StartCoroutine(StartWave(0));
        }
        else
        {
            StartCoroutine(TestingWave());
        }
    }
    IEnumerator StartWave(int waveNum)
    {
        bool messageActive = true;
        while (messageActive)
        {
            if (waves[waveNum].characterDialogue != null && GetComponent<MissileLauncher>().originalEnemyBase)
            {
                for (int i = 0; i < waves[waveNum].characterDialogue.Length; i++)
                {
                    CharacterDialogue message = waves[waveNum].characterDialogue[i];
                    Debug.Log("StartWaveDialogues for wave: " + waves[waveNum] + " message: " + message);
                    dialogueBox.GetComponent<UIInGameDialogue>().Dialogue(message);
                    yield return new WaitForSeconds(message.textTime);
                }
            }
            messageActive = false;
        }
        if (waveNum >= 1)
        {
            CloseDoors();
            GetComponent<MissileLauncher>().StartFireLazerCoroutine(waveNum);
        }
        else
        {
            StartCoroutine(WaveActivation(waveNum));
        }
        yield return null;
    }
    public void WaveActivator(int waveNum)
    {
        StartCoroutine(WaveActivation(waveNum));
    }
    IEnumerator WaveActivation(int waveNum)
    {
        yield return new WaitForSeconds(timeBetweenWaves / 2);
        OpenDoors();
        yield return new WaitForSeconds(timeBetweenWaves / 2);

        foreach (WaveCreator spawn in waves[waveNum].objectsToSpawn)
        {
            if (!countdownActive) break;
            if (activeWave != waveNum) break;
            StartCoroutine(SpawnObjects(activeWave, spawn.delayAtBeginningOfWave, spawn.objectToSpawn, spawn.minorSpawnRate, spawn.majorSpawnRate, spawn.spawnType));
            Debug.Log("Starting Coroutine to Spawn " + spawn.objectToSpawn.name);
        }
    }
    IEnumerator SpawnObjects(int wave, float startDelay, GameObject spawnObject, float minorRate, float majorRate, SpawnTypes type)
    {
        yield return new WaitForSeconds(startDelay);
        while (!WaveComplete(wave))
        {
            if (type == SpawnTypes.Missile)
            {
                Debug.Log("Time to spawn Missiles");
                foreach (Transform spawnPoint in missileSpawnPoints)
                {
                    if (activeWave != wave) { break; }
                    LaunchSpawn(spawnObject, spawnPoint.position, spawnPoint.localRotation);
                    // float rndForce = UnityEngine.Random.Range(0.75f, 1.50f);
                    // GameObject spawnedObj = Instantiate(spawnObject, spawnPoint.position, spawnPoint.localRotation);
                    // spawnedObj.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * rndForce, ForceMode.Impulse);
                    // Debug.Log("Instantiating Missile: " + spawnObject);
                    // EndForce(spawnedObj.GetComponent<Rigidbody>());
                    yield return new WaitForSeconds(minorRate);
                }
            }
            if (type == SpawnTypes.Enemy)
            {
                Debug.Log("Time to spawn Enemies");
                foreach (Transform spawnPoint in enemySpawnPoints)
                {
                    if (activeWave != wave) break;
                    LaunchSpawn(spawnObject, spawnPoint.position, spawnPoint.localRotation);
                    // float rndForce = UnityEngine.Random.Range(0.75f, 1.50f);
                    // GameObject spawnedObj = Instantiate(spawnObject, spawnPoint.position, spawnPoint.localRotation);
                    // spawnedObj.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * rndForce, ForceMode.Impulse);
                    // Debug.Log("Instantiating Missile: " + spawnObject);
                    // EndForce(spawnedObj.GetComponent<Rigidbody>());
                    yield return new WaitForSeconds(minorRate);
                }
            }
            Debug.Log("Cycling through major timer");
            yield return new WaitForSeconds(majorRate);
        }
        if (GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("OpenDoors")) CloseDoors();
    }
    void CloseDoors()
    {
        Debug.Log("Close Doors Now");
        if (anim != null) { anim.ResetTrigger("OpenDoors"); anim.SetTrigger("CloseDoors"); }
    }
    void OpenDoors()
    {
        Debug.Log("Open Doors Now");
        if (GetComponentInChildren<Animator>() != null) GetComponentInChildren<Animator>().SetTrigger("OpenDoors");
    }
    IEnumerator TestingWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        GameObject.Find("DebugWaveText").GetComponent<UIWaveText>().StartDisplay();
        foreach (WaveCreator spawn in testWave.objectsToSpawn)
        {
            StartCoroutine(SpawnTestObjects(testWave, spawn.delayAtBeginningOfWave, spawn.objectToSpawn, spawn.minorSpawnRate, spawn.majorSpawnRate, spawn.spawnType));
            Debug.Log("Starting Coroutine to Spawn " + spawn.objectToSpawn.name);
        }
    }
    IEnumerator SpawnTestObjects(WaveScriptableObject myWave, float startDelay, GameObject spawnObject, float minorRate, float majorRate, SpawnTypes type)
    {
        yield return new WaitForSeconds(startDelay);
        while (!WaveComplete(testWave))
        {
            if (type == SpawnTypes.Missile)
            {
                Debug.Log("Time to spawn Missiles");
                foreach (Transform spawnPoint in missileSpawnPoints)
                {
                    LaunchSpawn(spawnObject, spawnPoint.position, spawnPoint.localRotation);
                    // float rndForce = UnityEngine.Random.Range(0.75f, 1.50f);
                    // GameObject spawnedObj = Instantiate(spawnObject, spawnPoint.position, spawnPoint.localRotation);
                    // spawnedObj.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * rndForce, ForceMode.Impulse);
                    // Debug.Log("Instantiating Missile: " + spawnObject);
                    // EndForce(spawnedObj.GetComponent<Rigidbody>());
                    yield return new WaitForSeconds(minorRate);
                }
            }
            if (type == SpawnTypes.Enemy)
            {
                Debug.Log("Time to spawn Enemies");
                foreach (Transform spawnPoint in enemySpawnPoints)
                {
                    LaunchSpawn(spawnObject, spawnPoint.position, spawnPoint.localRotation);
                    // float rndForce = UnityEngine.Random.Range(0.75f, 1.50f);
                    // GameObject spawnedObj = Instantiate(spawnObject, spawnPoint.position, spawnPoint.localRotation);
                    // spawnedObj.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * rndForce, ForceMode.Impulse);
                    // Debug.Log("Instantiating Missile: " + spawnObject);
                    //EndForce(spawnedObj.GetComponent<Rigidbody>());
                    yield return new WaitForSeconds(minorRate);
                }
            }
            Debug.Log("Cycling through major timer");
            yield return new WaitForSeconds(majorRate);
        }
    }
    IEnumerator EndForce(Rigidbody rb)
    {
        yield return new WaitForSeconds(1);
        rb.velocity.Set(0, 0, 0);
    }
    void LaunchSpawn(GameObject spawnObject, Vector3 spawnPos, Quaternion spawnRot)
    {
        float rndForce = UnityEngine.Random.Range(1.5f, 3f);
        GameObject spawnedObj = Instantiate(spawnObject, spawnPos, spawnRot);
        spawnedObj.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * rndForce, ForceMode.Impulse);
        if (spawnedObj.GetComponent<Missile>() != null)
        {
            spawnedObj.GetComponent<Missile>().target = GameObject.Find("Player").transform;
        }
        Debug.Log("Instantiating Missile: " + spawnObject);
        EndForce(spawnedObj.GetComponent<Rigidbody>());
    }
    bool WaveComplete(int waveNum, float endBufferTime = 0)
    {
        return (waves[waveNum].waveElapsed >= waves[activeWave].waveLength + endBufferTime);
    }
    bool WaveComplete(WaveScriptableObject myWave, float endBufferTime = 0)
    {
        return myWave.waveElapsed >= myWave.waveLength + endBufferTime;
    }

    public int GetWaveNum()
    {
        return activeWave;
    }
    public int GetWaveTotal()
    {
        return waves.Length;
    }
    public int GetActiveWave()
    {
        return activeWave;
    }
}
