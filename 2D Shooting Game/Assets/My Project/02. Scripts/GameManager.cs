using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyObjs;
    public Transform[] spawnPoints;

    public float maxSpawnDelay;
    public float curSpwanDelay;

    private void Update()
    {
        curSpwanDelay += Time.deltaTime;

        if (curSpwanDelay > maxSpawnDelay)
        {
            SpawnEnemy();

            maxSpawnDelay = Random.Range(0.5f, 3f);
            curSpwanDelay = 0;
        }
    }

    private void SpawnEnemy()
    {
        int ranEnemy = Random.Range(0, 3);
        int ranPoint = Random.Range(0, 5);

        Instantiate(enemyObjs[ranEnemy], spawnPoints[ranPoint].position, spawnPoints[ranPoint].rotation);
    }
}
