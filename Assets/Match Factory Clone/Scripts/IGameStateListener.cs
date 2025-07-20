using UnityEngine;

public interface IGameStateListener
{
    void GameStateChangedCallback(EGameState gameState);
}
