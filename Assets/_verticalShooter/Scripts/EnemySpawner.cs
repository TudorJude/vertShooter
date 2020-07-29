using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject leftLimit;
    public GameObject rightLimit;

    [Header("Prefab Enemies")]
    public Enemy AsteroidPrefab;

    public float spawnFrequency = 3f;

    private float ySpawnValue;

    private float spawnCounter = 0f;
    private float xLeftSpawnValue;
    private float xRightSpawnValue;
    // Start is called before the first frame update
    void Awake()
    {
        ySpawnValue = leftLimit.transform.position.y;
        xLeftSpawnValue = leftLimit.transform.position.x;
        xRightSpawnValue = rightLimit.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        spawnCounter += Time.deltaTime;
        if(spawnCounter >= spawnFrequency)
        {
            spawnCounter = 0f;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(xLeftSpawnValue, xRightSpawnValue), ySpawnValue, 0f);
        Instantiate(AsteroidPrefab, spawnPosition, Quaternion.identity);
    }
}
