using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private EGameState gameState;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        SetGameState(EGameState.MENU);
    }

    public void SetGameState(EGameState gameState)
    {
        //!!! important
        this.gameState = gameState;

        IEnumerable<IGameStateListener> gameStateListeners
            = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .OfType<IGameStateListener>();

        foreach (IGameStateListener dependency in gameStateListeners)
        {
            dependency.GameStateChangedCallback(gameState);
        }
    }

    public void StartGame()
    {
        SetGameState(EGameState.GAME);
    }

    public bool IsGame() => gameState == EGameState.GAME;

    public void NextButtonCallback()
    {
        SceneManager.LoadScene(0);
    }

    public void RetryButtonCallback()
    {
        SceneManager.LoadScene(0);
    }
}