using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Base Stats")]
    public float health = 10.0f;
    public float speed = 10.0f;

    [Header("Combat Stats")]
    public float damage = 1f;
    public float contactDamage = 1f;
    public float fireRate = 0.5f; // Seconds between shots
    public float bulletSpeed = 10.0f;
    public float range = 1.0f;
}
