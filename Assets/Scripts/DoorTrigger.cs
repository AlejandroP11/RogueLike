using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    // When the player enters the trigger, it will load the specified target scene.
    [SerializeField] private string targetScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered door trigger, loading scene: " + targetScene);
            SceneManager.LoadScene(targetScene);
        }
    }
}
