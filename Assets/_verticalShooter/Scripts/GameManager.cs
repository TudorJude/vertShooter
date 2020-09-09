using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public EnemySpawner enemySpawner;

    public PlayerShip playerShip;
    [Header("Audio Clips")]
    public AudioClip hitSound;
    [Header("Prefabs")]
    public HitParticle hitParticlePrefab;

    private AudioSource audioSource;

    //pools
    private ObjectPool<HitParticle> hitParticlesPool;

    private float spawnCounter = 0;

    private bool isPaused;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        spawnCounter = 0;
        //init pools
        hitParticlesPool = new ObjectPool<HitParticle>(hitParticlePrefab, 50, 50);
        hitParticlesPool.Init();

        //Initialize stuff
        enemySpawner.Init(playerShip);
    }

    private void OnEnable()
    {
        //ship related events
        BusSystem.General.OnShipDied += HandleShipDeath;

        BusSystem.UI.OnGamePauseClicked += PauseGame;
        BusSystem.UI.OnGameResumeClicked += ResumeFunction;

        //effect related events
        BusSystem.Effects.OnBulletImpact += HandleBulletImpact;
    }

    private void OnDisable()
    {
        //ship related events
        BusSystem.General.OnShipDied -= HandleShipDeath;

        BusSystem.UI.OnGamePauseClicked -= PauseGame;
        BusSystem.UI.OnGameResumeClicked -= ResumeFunction;

        //effect related events
        BusSystem.Effects.OnBulletImpact -= HandleBulletImpact;
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

    //handlers
    private void HandleShipDeath(GameObject ship)
    {
        ship.SetActive(false);
    }

    private void HandleBulletImpact(Vector3 bulletImpactPos)
    {
        HitParticle hitPart = hitParticlesPool.Instantiate();
        hitPart.transform.position = bulletImpactPos;
        hitPart.OnCleanMe = () => 
        {
            hitParticlesPool.Destroy(hitPart);
        };

        //play a sound
        audioSource.clip = hitSound;
        audioSource.Play();
    }

}
