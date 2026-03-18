using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyStats;
    private Rigidbody enemyRb;
    private GameObject player;
    private bool isDead = false;
    private float currentHealth;

    void Start()
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
}

