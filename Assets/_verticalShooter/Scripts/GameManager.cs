using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public EnemySpawner asteroidSpawner;
    public EnemySpawner enemyShip1Spawner;

    public PlayerShip playerShip;

    private float spawnCounter = 0;

    private bool isPaused;

    private void Awake()
    {
        spawnCounter = 0;

        //Initialize stuff
        asteroidSpawner.Init(playerShip);
        enemyShip1Spawner.Init(playerShip);
    }

    private void OnEnable()
    {
        //ship related events
        BusSystem.General.OnShipDied += HandleShipDeath;

        BusSystem.UI.OnGamePauseClicked += PauseGame;
        BusSystem.UI.OnGameResumeClicked += ResumeFunction;
    }

    private void OnDisable()
    {
        //ship related events
        BusSystem.General.OnShipDied -= HandleShipDeath;

        BusSystem.UI.OnGamePauseClicked -= PauseGame;
        BusSystem.UI.OnGameResumeClicked -= ResumeFunction;
    }

    public void PauseGame()
    {
        if (isPaused)
            return;
        isPaused = true;
        Debug.Log("Paused game");
        Time.timeScale = 0f;

        BusSystem.General.PauseGame();
    }

    public void ResumeFunction()
    {
        if (!isPaused)
            return;
        isPaused = false;
        Debug.Log("Resumed game");
        Time.timeScale = 1f;

        BusSystem.General.ResumeGame();
    }

    private void Update()
    {
        spawnCounter += Time.deltaTime;
        if(spawnCounter >= 6f)
        {
            SpawnEnemies();
            spawnCounter = 0;
        }
    }

    public void SpawnEnemies()
    { 
        BusSystem.General.SpawnEnemies(Enemy.EnemyType.Asteroid, 5, 2f);
    }

    //handlers
    private void HandleShipDeath(GameObject ship)
    {
        ship.SetActive(false);
    }
}
