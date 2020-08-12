using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceController : MonoBehaviour
{
    public Image healthBar;

    public RectTransform pausePanel;

    //pause resume buttons
    public Button pauseButton;
    public Button resumeButton;

    private void Start()
    {
        healthBar.fillAmount = 1f;
        pausePanel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        BusSystem.General.OnShipHit += HandleHealthChanged;
        BusSystem.General.OnGamePaused += HandleGamePaused;
        BusSystem.General.OnGameResumed += HandleGameResumed;

        //onClick subscriptions
        pauseButton.onClick.AddListener
            (
                 HandlePauseGameClicked
            );
        //() => { HandlePauseGame(); }
        resumeButton.onClick.AddListener
            (
                () => { HandleResumeGameClicked(); }
            );
    }

    private void OnDisable()
    {
        BusSystem.General.OnShipHit -= HandleHealthChanged;
        BusSystem.General.OnGamePaused -= HandleGamePaused;
        BusSystem.General.OnGameResumed -= HandleGameResumed;

        pauseButton.onClick.RemoveAllListeners();
        resumeButton.onClick.RemoveAllListeners();
    }

    //handlers
    private void HandleHealthChanged(int currentHealth, int maxHealth)
    {
        healthBar.fillAmount = currentHealth / (float)maxHealth;
    }

    private void HandleGamePaused()
    {
        pausePanel.gameObject.SetActive(true);
    }

    private void HandleGameResumed()
    {
        pausePanel.gameObject.SetActive(false);
    }

    //button on click handlers
    private void HandlePauseGameClicked()
    {        
        BusSystem.UI.PauseGameRequest();
    }

    private void HandleResumeGameClicked()
    {        
        BusSystem.UI.ResumeGameRequest();
    }
}
