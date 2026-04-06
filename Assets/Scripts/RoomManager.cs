using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    [SerializeField] private bool isBossRoom;
    [SerializeField] private string mainMenuSceneName = "Menu";

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

        // If all enemies are defeated, open the doors or return to the menu if it's a boss room
        if (enemyCount <= 0)
        {
            if (isBossRoom)
            {
                Invoke("ReturnToMenu", 2f); 
            }
            else
            {
                OpenDoors();
            }
        }
    }
    private void OpenDoors()
    {
        // Activate the door triggers and deactivate the doors
        foreach (GameObject door in doors)
        {
            // Activate the door trigger
            Transform trigger = door.transform.Find("DoorTrigger");
            if (trigger != null)
            {
                trigger.gameObject.SetActive(true);
            }

            // Deactivate the door's renderer and collider to make it non-solid and invisible
            if (door.TryGetComponent<Renderer>(out Renderer rd)) rd.enabled = false;
            if (door.TryGetComponent<Collider>(out Collider col)) col.enabled = false;

            Debug.Log("All enemies defeated, opening doors.");
        }
    }

    private void ReturnToMenu()
    {
        // Load the main menu scene after a short delay
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
