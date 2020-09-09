using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    public GameObject leftLimit;
    public GameObject rightLimit;
    public GameObject lowerLimit;

    [Header("Wave")]
    public WaveData waveToSpawn;

    private float uppwerYSpawnValue;
    private float lowerYSpawnValue;

    private float xLeftSpawnValue;
    private float xRightSpawnValue;

    private List<Enemy> enemyList;

    private PlayerShip playerShip;

    //pool
    private ObjectPool<Enemy> enemyPool;

    //spawn matrix
    private int matrixRows = 5;
    private int matrixCols = 4;
    private Vector3[][] enemySpawnMatrix;

    private void Awake()
    {
        uppwerYSpawnValue = leftLimit.transform.position.y;
        lowerYSpawnValue = lowerLimit.transform.position.y;
        xLeftSpawnValue = leftLimit.transform.position.x;
        xRightSpawnValue = rightLimit.transform.position.x;

        enemyList = new List<Enemy>();

        //creating pool
        enemyPool = new ObjectPool<Enemy>(waveToSpawn.enemyToSpawnPrefab, 30);
        enemyPool.Init();

        //matrix init
        //x length
        float xMiniSegmentLength = (xRightSpawnValue - xLeftSpawnValue) / (matrixCols);
        float yMiniSegmentLength = (uppwerYSpawnValue - lowerYSpawnValue) / (matrixRows + 1);
        Vector3 centerOffset = new Vector3(xMiniSegmentLength / 2, -yMiniSegmentLength / 2, 0f);
        Vector3 topLeftCorner = leftLimit.transform.position;

        enemySpawnMatrix = new Vector3[matrixCols][];
        for (int i = 0; i < matrixCols; i++)
        {
            enemySpawnMatrix[i] = new Vector3[matrixRows];
            for (int j = 0; j < matrixRows; j++)
            {
                enemySpawnMatrix[i][j] = topLeftCorner + new Vector3(xMiniSegmentLength * i, -yMiniSegmentLength * j, 0f) + centerOffset;
            }
        }
    }

    private void OnEnable()
    {
        BusSystem.General.OnEnemyDestroyed += HandleEnemyDestroy;
    }

    private void OnDisable()
    {
        BusSystem.General.OnEnemyDestroyed -= HandleEnemyDestroy;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SpawnWave(waveToSpawn);
        }
    }

    //custom functions
    public void Init(PlayerShip playerShip)
    {
        this.playerShip = playerShip;
    }

    private void SpawnEnemy(Vector3 spawnPos, float spawnDelay)
    {
        if (enemyList.Count >= 30)
            return;
        StartCoroutine(SpawnShipWithDelay(spawnPos, spawnDelay));
    }

    private void SpawnWave(WaveData wave)
    {
        float maxSpawnDelay = -1;
        List<float> tempFloatList;
        for (int i = 0; i < matrixRows; i++)
        {
            switch (i)
            {
                case 0: tempFloatList = wave.row0; break;
                case 1: tempFloatList = wave.row1; break;
                case 2: tempFloatList = wave.row2; break;
                case 3: tempFloatList = wave.row3; break;
                case 4: tempFloatList = wave.row4; break;
                default: tempFloatList = new List<float>(); break;
            }

            for (int j = 0; j < matrixCols; j++)
            {
                if (tempFloatList[j] > 0)
                {
                    SpawnEnemy(enemySpawnMatrix[j][i], tempFloatList[j]);
                    if (maxSpawnDelay < tempFloatList[j])
                    {
                        maxSpawnDelay = tempFloatList[j];
                    }
                }
            }
            StartCoroutine(ActivateWave(maxSpawnDelay));
        }
    }

    //handlers
    private void HandleEnemyDestroy(Enemy en)
    {
        if (en.enemyType != waveToSpawn.enemyToSpawnPrefab.enemyType)
            return;
        enemyList.Remove(en);
        enemyPool.Destroy(en);
    }

    //IEnumerators
    IEnumerator SpawnShipWithDelay(Vector3 spawnPos, float spawnDelay)
    {
        yield return new WaitForSeconds(spawnDelay);
        Vector3 spawnPosition = spawnPos;
        //Instantiate(AsteroidPrefab, spawnPosition, Quaternion.identity);
        Enemy en = enemyPool.Instantiate(transform.root);
        en.gameObject.transform.position = spawnPosition;
        en.Init(xLeftSpawnValue, xRightSpawnValue, playerShip);
        enemyList.Add(en);
    }

    IEnumerator ActivateWave(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (var enemy in enemyList)
        {
            enemy.Activate();
        }
    }
}
