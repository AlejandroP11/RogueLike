using UnityEngine;

public class ResetStaticData : MonoBehaviour
{
    private void Awake() {
        Enemy.ResetStaticData();
    }
}
