using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public enum GameState 
    {
        Gameplay,
        Paused,
        GameOver
    }

    public GameState currentState;
    public GameState previousState;

    void Update()
    {
        switch (currentState) 
        {
            case GameState.Gameplay:
                break;
            case GameState.Paused:
                break;
            case GameState.GameOver:
                break;
            default:
                Debug.Log("Invalid State");
                break;
        }

    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            currentState = GameState.Paused;
            Time.timeScale = 0f;
            Debug.Log("Game is paused");
        }
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            currentState = previousState;
            Time.timeScale = 1f;
            Debug.Log("Game is resumed");
        }
    }
}
