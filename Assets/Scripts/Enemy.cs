using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyStats;
    private Rigidbody enemyRb;
    protected GameObject player;
    private bool isDead = false;
    private float currentHealth;
    public GameObject bulletPrefab;
    public Transform firePoint;
    private float lastFireTime;

    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player");
        enemyRb = GetComponent<Rigidbody>();
        currentHealth = enemyStats.health;
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            MoveEnemy();
        }
    }

    private void Update()
    {
        if (!isDead && player != null)
        {
            if (Time.time >= lastFireTime + (1f / enemyStats.fireRate))
            {
                Shoot();
                lastFireTime = Time.time;
            }
        }
    }

    protected virtual void OnCollisionStay(Collision other)
    {
        if (!isDead && player != null)
        {
            if (other.gameObject.TryGetComponent<PlayerActions>(out PlayerActions player))
            {
                player.TakeDamage(enemyStats.contactDamage);
            }
        }
    }

    public void TakeDamage(float damage)
    {   
        if(isDead) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GetComponent<Collider>().enabled = false; // Disable collider to prevent further interactions
        isDead = true;
        Destroy(gameObject, 0.1f);
    }

    protected virtual void MoveEnemy()
    {
        if (player == null)
        {
            enemyRb.linearVelocity = Vector3.zero; 
            return;
        }

        Vector3 targetPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(targetPos);

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > 0.5f)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            enemyRb.linearVelocity = direction * enemyStats.speed;
        }
        else
        {
            enemyRb.linearVelocity = Vector3.zero;
        }
    }

    protected virtual void Shoot()
    {
        // Calculate the direction from the enemy to the player
        Vector3 direction = (player.transform.position - transform.position).normalized;

        // Position the fire point slightly in front of the enemy to prevent immediate collision with the bullet
        firePoint.localPosition = Vector3.forward * 1.2f;

        // Instantiate the bullet at the fire point's position and rotation
        GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Ignore collision between the bullet and the player
        Physics.IgnoreCollision(newBullet.GetComponent<Collider>(), GetComponent<Collider>());

        if (newBullet.TryGetComponent<Bullet>(out Bullet bulletScript))
        {
            bulletScript.Launch(direction, enemyStats.bulletSpeed, enemyStats.range, enemyStats.damage);
        }
    }
}

