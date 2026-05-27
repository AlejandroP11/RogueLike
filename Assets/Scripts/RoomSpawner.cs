using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    private void Start()
    {
        Direction oppositeDirection = GetOppositeDirection(PlayerActions.Instance.GetLastEnteredDirection());
        GetSpawnPointByDirection(oppositeDirection);
    }

    private Direction GetOppositeDirection(Direction direction) {
        switch (direction) {
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
            case Direction.None:
                return Direction.None;
            default:
                return Direction.None;
        }
    }

    private void GetSpawnPointByDirection(Direction direction) {
        foreach (Transform spawnPoint in spawnPoints) {
            if (spawnPoint.GetComponent<SpawnPoint>().GetDirection() == direction) {
                PlayerActions.Instance.transform.position = spawnPoint.position;
                Debug.Log($"Player spawned at {spawnPoint.position} from direction {direction}");
                return;
            }
        }
    }
}
