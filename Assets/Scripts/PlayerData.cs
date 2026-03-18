using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Base Stats")]
    public float maxHealth = 10.0f;
    public float speed = 10.0f;

    [Header("Combat Stats")]
    public float damage = 1f;
    public float fireRate = 0.5f; // Seconds between shots
    public float bulletSpeed = 10.0f;
    public float range = 1.0f;

    // A helper function to reset your stats
    public void ResetStats(PlayerData source)
    {
        maxHealth = source.maxHealth;
        speed = source.speed;
        damage = source.damage;
        fireRate = source.fireRate;
        bulletSpeed = source.bulletSpeed;
    }
}
