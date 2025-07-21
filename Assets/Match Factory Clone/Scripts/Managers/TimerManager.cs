using System;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")] 
    [SerializeField] private TextMeshProUGUI timerText;

    private int currentTimer;
    
    private void Awake()
    {
        LevelManager.levelSpawned += OnLevelSpawned;
    }

    private void OnDestroy()
    {
        LevelManager.levelSpawned -= OnLevelSpawned;
    }

    private void OnLevelSpawned(Level level)
    {
        currentTimer = level.Duration;
        timerText.text = SecondsToString(currentTimer);
        
        StartTimer();
    }

    private void StartTimer()
    {
        InvokeRepeating("UpdateTimer",0,1);
    }

    private void UpdateTimer()
    {
        currentTimer--;
        UpdateTimerText();

        if (currentTimer<=0)
            TimerFinished();
    }

    private void TimerFinished()
    {
        GameManager.instance.SetGameState(EGameState.GAMEOVER);
        StopTimer();
    }

    private string SecondsToString(int seconds)
    {
        return TimeSpan.FromSeconds(seconds).ToString().Substring(3);
    }
    
    public void GameStateChangedCallback(EGameState gameState)
    {
        if (gameState == EGameState.LEVELCOMPLETE || gameState == EGameState.GAMEOVER)
        {
            StopTimer();
        }
    }

    private void StopTimer()
    {
        CancelInvoke("UpdateTimer");
    }

    private void UpdateTimerText()
    {
        timerText.text = SecondsToString(currentTimer);
    }
}