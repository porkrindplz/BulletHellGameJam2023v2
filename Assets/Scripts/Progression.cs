using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progression : MonoBehaviour
{
    [SerializeField] int finalWaveOfFirstPart = 5;
    [SerializeField] GameObject firstEnemyBase;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Vector3[] targetPositions;
    [SerializeField] Vector3[] enemyPositions;
    [SerializeField] CharacterDialogue dialogue;
    bool hasSpawned;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (firstEnemyBase.GetComponent<WaveManager>().GetActiveWave() > finalWaveOfFirstPart && !hasSpawned)
        {
            StartCoroutine(SpawnNewBases());
            hasSpawned = true;
        }
    }
    IEnumerator SpawnNewBases()
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < enemyPositions.Length; i++)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, enemyPositions[i], Quaternion.Euler(0, 0, 0));
            newEnemy.GetComponent<MissileLauncher>().SetArrivalLocation(targetPositions[i]);
            yield return new WaitForSeconds(0.5f);
        }

    }
}
