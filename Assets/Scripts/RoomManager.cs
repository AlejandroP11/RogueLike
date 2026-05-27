using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{

    [SerializeField] private bool isBossRoom;
    [SerializeField] private string mainMenuSceneName = "Menu";
    [SerializeField] private DoorController[] doors;

    private Enemy[] enemies;
    private int enemyCount;

    private void Awake()
    {

    }

    private void Start()
    {
        Enemy.OnEnemyDie += Enemy_OnEnemyDie;

        // Find all enemies and doors in the room
        enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        enemyCount = enemies.Length;
    }

    private void OnDestroy() {
        Enemy.OnEnemyDie -= Enemy_OnEnemyDie;
    }

    private void Enemy_OnEnemyDie(object sender, EventArgs e) {
        // Decrease the enemy count and check if all enemies are defeated
        enemyCount--;

        // If all enemies are defeated, open the doors or return to the menu if it's a boss room
        if (enemyCount <= 0) {
            if (isBossRoom) {
                Invoke("ReturnToMenu", 2f);
            } else {
                OpenDoors();
            }
        }
    }

    private void OpenDoors()
    {
        foreach (var door in doors) {
            door.UnlockCombat();
        }
    }

    private void ReturnToMenu()
    {
        // Load the main menu scene after a short delay
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void Configure(HashSet<Direction> connectedDirections) {

    }
}
