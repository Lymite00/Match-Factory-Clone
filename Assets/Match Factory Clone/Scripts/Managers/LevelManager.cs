using System;
using UnityEngine;

public class LevelManager : MonoBehaviour, IGameStateListener
{
    public static LevelManager instance;
    
    [Header("Data")] 
    [SerializeField] private Level[] levels;

    private int levelIndex;
    private const string levelKey = "LevelReached";

    public Item[] Items => currentLevel.GetItems();
    
    [Header("Settings")] 
    private Level currentLevel;

    [Header("Actions")] 
    public static Action<Level> levelSpawned;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        LoadData();
    }

    private void SpawnLevel()
    {
        transform.Clear();

        int validateLevelIndex = levelIndex % levels.Length;
        
        currentLevel = Instantiate(levels[validateLevelIndex], transform);

        levelSpawned?.Invoke(currentLevel);
    }

    private void LoadData()
    {
        levelIndex = PlayerPrefs.GetInt(levelKey);
    }
    
    private void SaveData()
    {
        PlayerPrefs.SetInt(levelKey, levelIndex);
    }

    public void GameStateChangedCallback(EGameState gameState)
    {
        if (gameState == EGameState.GAME)
        {
            SpawnLevel();
        }
        else if (gameState == EGameState.LEVELCOMPLETE)
        {
            levelIndex++;
            SaveData();
        }
    }
}