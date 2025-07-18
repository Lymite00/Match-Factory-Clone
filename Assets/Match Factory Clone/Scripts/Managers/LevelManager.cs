using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Data")] 
    [SerializeField] private Level[] levels;

    private int levelIndex;
    private const string levelKey = "LevelReached";

    [Header("Settings")] 
    private Level currentLevel;

    [Header("Actions")] 
    public static Action<Level> levelSpawned;

    private void Awake()
    {
        LoadData();
    }

    private void Start()
    {
        SpawnLevel();
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
        levelIndex = PlayerPrefs.GetInt("Level");
    }
    
    private void SaveData()
    {
        PlayerPrefs.SetInt(levelKey, levelIndex);
    }
}