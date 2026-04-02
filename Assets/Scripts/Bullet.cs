using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody bulletRb;
    private float bulletDamage;
    private bool isPlayerBullet = false;

    public void Launch(Vector3 direction, float speed, float range, float damage, bool playerBullet)
    {
        bulletRb = GetComponent<Rigidbody>();
        bulletDamage = damage;
        bulletRb.linearVelocity = direction.normalized * speed;
        isPlayerBullet = playerBullet;

        // Calculate the time to live for the bullet based on its speed and range
        float timeToLive = range / speed;

        // The range is used as a time to live for the bullet, assuming it travels at a constant speed.
        Destroy(gameObject, timeToLive); // Destroy the bullet after it has traveled its range
    }

    // Overloaded method for backward compatibility, defaults to playerBullet = false
    public void Launch(Vector3 direction, float speed, float range, float damage)
    {
        bulletRb = GetComponent<Rigidbody>();
        bulletDamage = damage;
        bulletRb.linearVelocity = direction.normalized * speed;

        // Calculate the time to live for the bullet based on its speed and range
        float timeToLive = range / speed;

        // The range is used as a time to live for the bullet, assuming it travels at a constant speed.
        Destroy(gameObject, timeToLive); // Destroy the bullet after it has traveled its range
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected: " + other.gameObject.name);
        if (other.TryGetComponent <Enemy>(out Enemy enemy))
        {
            if (isPlayerBullet)
            {
                enemy.TakeDamage(bulletDamage); // Call the TakeDamage method on the enemy
                Destroy(gameObject); // Destroy the bullet after hitting an enemy
            }
        }

        if (other.TryGetComponent<PlayerActions>(out PlayerActions player))
        {
            player.TakeDamage(bulletDamage); // Call the TakeDamage method on the player
            Destroy(gameObject); // Destroy the bullet after hitting an player
        }

        if (other.CompareTag("Wall") || other.CompareTag("Ground"))
        {
            Destroy(gameObject); // Destroy the bullet if it hits a wall
        }
    }
}
