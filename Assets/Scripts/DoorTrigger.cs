using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    private Direction direction;

    private void Start() {
        direction = GetComponentInParent<DoorController>().GetDirection();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerActions>(out PlayerActions player))
        {
            FloorGenerator.Instance.OnPlayerExitedRoom(direction);
        }
    }
}
