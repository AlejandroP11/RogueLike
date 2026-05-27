using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    [SerializeField] private Direction spawnDirection;

    public Direction GetDirection(){
        Debug.Log("SpawnPoint: " + spawnDirection);
        return spawnDirection; 
    }
}
