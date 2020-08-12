using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawner : MonoBehaviour
{   
    public GameObject leftLimit;
    public GameObject rightLimit;

    [Header("Prefab Enemies")]
    public Enemy EnemyPrefab;

    private float ySpawnValue;

    private float xLeftSpawnValue;
    private float xRightSpawnValue;

    private List<Enemy> enemyList;

    private PlayerShip playerShip;

    //pool
    private ObjectPool<Enemy> enemyPool;

    private void Awake()
    {
        ySpawnValue = leftLimit.transform.position.y;
        xLeftSpawnValue = leftLimit.transform.position.x;
        xRightSpawnValue = rightLimit.transform.position.x;

        enemyList = new List<Enemy>();

        //creating pool
        enemyPool = new ObjectPool<Enemy>(EnemyPrefab, 30);
        enemyPool.Init();
    }

    private void OnEnable()
    {
        BusSystem.General.OnEnemyDestroyed += HandleEnemyDestroy;
        BusSystem.General.OnSpawnEnemies += HandleSpawnEnemies;
    }

    private void OnDisable()
    {
        BusSystem.General.OnEnemyDestroyed -= HandleEnemyDestroy;
        BusSystem.General.OnSpawnEnemies -= HandleSpawnEnemies;
    }

    //custom functions
    public void Init(PlayerShip playerShip)
    {
        this.playerShip = playerShip;
    }

    private void SpawnEnemy()
    {
        if (enemyList.Count >= 30)
            return;
        Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(xLeftSpawnValue, xRightSpawnValue), ySpawnValue, 0f);
        //Instantiate(AsteroidPrefab, spawnPosition, Quaternion.identity);
        Enemy en = enemyPool.Instantiate(transform.root);
        en.gameObject.transform.position = spawnPosition;
        en.Init(xLeftSpawnValue, xRightSpawnValue, playerShip);
        enemyList.Add(en);
    }

    //handlers
    private void HandleEnemyDestroy(Enemy en)
    {
        if (en.enemyType != EnemyPrefab.enemyType)
            return;
        enemyList.Remove(en);
        enemyPool.Destroy(en);
    }

    private void HandleSpawnEnemies(Enemy.EnemyType enemyID, int enemyAmount, float spawnInterval)
    {
        if (EnemyPrefab.enemyType != enemyID)
            return;

        StartCoroutine(SpawnMultipleShips(enemyAmount, spawnInterval));            
    }

    //Ienumetors
    IEnumerator SpawnMultipleShips(int enemyAmount, float spawnInterval)
    {
        if (enemyAmount <= 0)
            yield break;

        float spawnFrequency = spawnInterval / enemyAmount;

        for(int i = 0; i < enemyAmount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnFrequency);
        }
        yield return null;
    }
}
