using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Start a new run by resetting the player's stats and loading the first room scene
        GameManager.Instance.StartNewRun();
        // Load the first room scene when the player clicks the "Play" button
        SceneManager.LoadScene("First_Room");
    }

    public void QuitGame()
    {
        // Log a message to the console before quitting the game
        Debug.Log("Quitting the game...");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
