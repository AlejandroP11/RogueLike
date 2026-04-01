using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{

    public PlayerData playerStats;
    private float currentHealth;

    private Vector2 movementInput;
    private Vector2 shootInput;
    private Rigidbody playerRb;

    public GameObject bulletPrefab;
    public Transform firePoint;

    private float lastFireTime = -Mathf.Infinity;

    private bool isInvincible = false;
    private bool isDead = false;

    // Method to get the current health of the player, used by the UI to update the health bar.
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = playerStats.maxHealth;
        playerRb = GetComponent<Rigidbody>();

        Debug.Log("Game Started! Player Health: " + currentHealth + " Player Damage: " + playerStats.damage + " Player Speed: "
            + playerStats.speed + " Player Fire Rate: " + playerStats.fireRate + " Player Bullet Speed: " + playerStats.bulletSpeed);
    }

    public void Update()
    {
        // Handle shooting in Update to ensure it responds to input every frame.
        if (shootInput.sqrMagnitude > 0 && Time.time >= lastFireTime + (1f / playerStats.fireRate))
        {
            Shoot(new Vector3(shootInput.x, 0, shootInput.y));
            lastFireTime = Time.time;
        }
    }

    private void FixedUpdate()
    {
        // Handle movement in FixedUpdate for consistent physics updates.
        if (playerStats == null) return;

        Vector3 direction = new Vector3(movementInput.x, 0, movementInput.y).normalized;
        playerRb.MovePosition(playerRb.position + direction * playerStats.speed * Time.fixedDeltaTime);

    }

    public void OnMove(InputValue value)
    {
        // This method is called by the Input System when the player provides movement input.
        movementInput = value.Get<Vector2>();
    }

    public void OnShoot(InputValue value)
    {
        // This method is called by the Input System when the player provides shooting input.
        shootInput = value.Get<Vector2>();
    }
    
    private void Shoot(Vector3 direction)
    {
        direction = direction.normalized;
        firePoint.localPosition = direction * 1.2f;

        GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        // Ignore collision between the bullet and the player
        Physics.IgnoreCollision(newBullet.GetComponent<Collider>(), GetComponent<Collider>());
        if (newBullet.TryGetComponent<Bullet>(out Bullet bulletScript))
        {
            bulletScript.Launch(direction, playerStats.bulletSpeed, playerStats.range, playerStats.damage);
        }
    }
    public void TakeDamage(float damage)
    {
        if (isDead) return;
        if (isInvincible) return;

        currentHealth -= damage;
        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);
        StartCoroutine(InvincibilityFrames(1.5f));
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        GameManager.Instance.GameOver();
    }

    IEnumerator InvincibilityFrames(float duration)
    {
        isInvincible = true;
        Debug.Log("Player is invincible for " + duration + " seconds.");
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }
}
