using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody bulletRb;
    private float bulletDamage;

    public void Launch(Vector3 direction, float speed, float range, float damage)
    {
        bulletRb = GetComponent<Rigidbody>();
        bulletDamage = damage;
        bulletRb.linearVelocity = direction.normalized * speed;

        // The range is used as a time to live for the bullet, assuming it travels at a constant speed.
        Destroy(gameObject, range); // Destroy the bullet after it has traveled its range
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detectado con: " + other.gameObject.name);
        if (other.TryGetComponent <Enemy>(out Enemy enemy))
        { 
            enemy.TakeDamage(bulletDamage); // Call the TakeDamage method on the enemy
            Destroy(gameObject); // Destroy the bullet after hitting an enemy
        }

        if (other.CompareTag("Wall") || other.CompareTag("Ground"))
        {
            Destroy(gameObject); // Destroy the bullet if it hits a wall
        }
    }
}
