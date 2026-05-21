using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    private void Start()
    {
        PlayerActions.Direction oppositeDirection = GetOppositeDirection(PlayerActions.Instance.GetLastEnteredDirection());
        GetSpawnPointByDirection(oppositeDirection);
    }

    private PlayerActions.Direction GetOppositeDirection(PlayerActions.Direction direction) {
        switch (direction) {
            case PlayerActions.Direction.Up:
                return PlayerActions.Direction.Down;
            case PlayerActions.Direction.Down:
                return PlayerActions.Direction.Up;
            case PlayerActions.Direction.Left:
                return PlayerActions.Direction.Right;
            case PlayerActions.Direction.Right:
                return PlayerActions.Direction.Left;
            case PlayerActions.Direction.None:
                return PlayerActions.Direction.None;
            default:
                return PlayerActions.Direction.None;
        }
    }

    private void GetSpawnPointByDirection(PlayerActions.Direction direction) {
        foreach (Transform spawnPoint in spawnPoints) {
            if (spawnPoint.GetComponent<SpawnPoint>().GetDirection() == direction) {
                PlayerActions.Instance.transform.position = spawnPoint.position;
                return;
            }
        }
    }
}
