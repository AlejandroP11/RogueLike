using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    [SerializeField] private PlayerActions.Direction spawnDirection;

    public PlayerActions.Direction GetDirection(){
        return spawnDirection; 
    }
}
