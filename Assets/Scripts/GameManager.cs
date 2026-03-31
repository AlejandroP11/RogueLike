using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerData baseStats;
    public PlayerData runStats;

    private void Awake()
    {
        // Implementing the Singleton pattern to ensure only one instance of GameManager exists.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartNewRun()
    {
        // Reset runStats to baseStats at the start of a new run
        runStats.ResetStats(baseStats); 
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene to restart the game
    }
}

