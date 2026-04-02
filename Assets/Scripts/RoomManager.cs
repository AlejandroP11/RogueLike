using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    private Enemy[] enemies;
    private GameObject[] doors;
    private int enemyCount;

    private void Awake()
    {
        // Ensure only one instance of RoomManager exists
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Find all enemies and doors in the room
        enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        doors = GameObject.FindGameObjectsWithTag("Door");
        enemyCount = enemies.Length;
    }

    public void EnemyDied()
    {
        // Decrease the enemy count and check if all enemies are defeated
        enemyCount--;
        if (enemyCount <= 0)
            OpenDoors();
    }

    private void OpenDoors()
    {
        // Activate the door triggers and deactivate the doors
        foreach (GameObject door in doors)
        {
            // Activate the trigger to allow the player to interact with the door
            Transform trigger = door.transform.Find("DoorTrigger");
            if (trigger != null)
                trigger.gameObject.SetActive(true);

            door.SetActive(false);
        }
    }
}
