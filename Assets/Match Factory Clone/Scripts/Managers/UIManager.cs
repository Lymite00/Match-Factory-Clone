using UnityEngine;

public class UIManager : MonoBehaviour, IGameStateListener
{
    [Header("Panels")] 
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    
    public void GameStateChangedCallback(EGameState gameState)
    {
        menuPanel.SetActive(gameState == EGameState.MENU);
        gamePanel.SetActive(gameState == EGameState.GAME);
        winPanel.SetActive(gameState == EGameState.LEVELCOMPLETE);
        losePanel.SetActive(gameState == EGameState.GAMEOVER);
    }
}