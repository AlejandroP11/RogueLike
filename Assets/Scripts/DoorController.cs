using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject doorVisual;
    [SerializeField] private GameObject doorOpen;
    [SerializeField] private GameObject doorTrigger;

    private bool isConnected = false;
    private bool isCombatLocked = true;
    private Direction direction; 

    private void Awake() {
        RefreshVisual();
        direction = GetComponentInChildren<SpawnPoint>().GetDirection();
    }

    public void Activate() {
        isConnected = true;
        RefreshVisual();
    }

    public void Deactivate() {
        isConnected = false;
        RefreshVisual();
    }

    public void LockCombat() {
        isCombatLocked = true;
        RefreshVisual();
    }

    public void UnlockCombat() {
        isCombatLocked = false;
        RefreshVisual();
    }

    private void RefreshVisual() {
        if (isConnected && !isCombatLocked) {
            doorVisual.SetActive(false);
            doorOpen.SetActive(true);
            doorTrigger.SetActive(true);
        } else {
            doorVisual.SetActive(true);
            doorOpen.SetActive(false);
            doorTrigger.SetActive(false);
        }
    }

    public Direction GetDirection() {
        return direction;
    }
}
