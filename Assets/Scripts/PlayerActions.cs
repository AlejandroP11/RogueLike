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

    private float lastFireTime;

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
}
