using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    [SerializeField] GameObject spawnPrefab;
    [SerializeField] GameObject[] spawnPoints;
    [SerializeField] float spawnRange;
    [SerializeField] float minorSpawnInterval;
    [SerializeField] float majorSpawnInterval;
    [SerializeField] float forwardSpeed;
    [SerializeField] bool trackSpawnCount;
    [SerializeField] int maxActiveObjects;
    List<GameObject> activeSpawnList = new List<GameObject>();
    Vector3 targetPositionOffset;
    Transform player;
    bool startedSpawning;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        int rndOffset = Random.Range(15, 20);
        int rndSide = Random.Range(-1, 2);
        targetPositionOffset = Vector3.right * rndOffset * rndSide;
    }

    // Update is called once per frame
    void Update()
    {
        if (startedSpawning) return;
        transform.LookAt(player.position + targetPositionOffset - (Vector3.right * -5));
        transform.position += transform.forward * forwardSpeed * Time.deltaTime;
        if (Vector3.Distance(transform.position, player.position + targetPositionOffset) <= spawnRange)
        {
            StartCoroutine(SpawnAtInterval(minorSpawnInterval, majorSpawnInterval));
            startedSpawning = true;
        }
    }

    IEnumerator SpawnAtInterval(float minorTime, float majorTime)
    {
        while (true)
        {
            foreach (GameObject spawnPoint in spawnPoints)
            {
                if (trackSpawnCount)
                {
                    for (int i = 0; i < activeSpawnList.Count; i++)
                    {
                        if (activeSpawnList[i] == null) activeSpawnList.Remove(activeSpawnList[i]);
                    }
                    if (activeSpawnList.Count >= maxActiveObjects) break;
                }
                yield return new WaitForSeconds(minorTime);
                GameObject spawn = Instantiate(spawnPrefab, spawnPoint.transform.position, spawnPoint.transform.localRotation);
                spawn.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 20, ForceMode.Impulse);
                if (trackSpawnCount)
                {
                    activeSpawnList.Add(spawn);
                }
            }
            yield return new WaitForSeconds(majorTime);
        }
    }
}
