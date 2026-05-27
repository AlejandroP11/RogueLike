using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    public static FloorGenerator Instance;

    private void Awake()
    {
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

    public void OnPlayerExitedRoom(Direction direction) {

    }
}
