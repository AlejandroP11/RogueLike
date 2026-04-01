using UnityEngine;
using UnityEngine.WSA;

public class BossAgile : Enemy
{
    // Define the different states the boss can be in.
    private enum BossState
    {
        Idle,
        Telegraphing,
        Charging,
        Recovering,
        Shooting
    }

    // Current state of the boss.
    private BossState currentState = BossState.Idle;

    [Header("Attack Settings")]
    [SerializeField] private float chargeRange = 5f; // Range at which the boss will start charging towards the player.
    [SerializeField] private float chargeSpeed = 8f; // Speed at which the boss charges towards the player.
    [SerializeField] private float telegraphDuration = 1f; // Duration of the telegraphing phase before charging.
    [SerializeField] private float chargeDuration = 1f; // Duration of the charging phase.
    [SerializeField] private float recoverDuration = 1f; // Duration of the recovery phase after charging.

    [Header("Visual Feedback")]
    [SerializeField] private Color telegraphColor = new Color(0.5f, 0f, 0.5f); // Purple color to indicate the boss is telegraphing an attack.
    private Color originalColor;
    private MeshRenderer meshRenderer;

    private float stateTimer = 0f; // Timer to track the duration of the current state.
    private Vector3 chargeDirection; // Direction in which the boss will charge towards the player.
    private float failCharges = 0; // Counter for failed charge attempts.
    private bool hitPlayerDuringCharge = false; // Flag to track if the boss hit the player during the charge.

    protected override void Start()
    {
        base.Start();
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            originalColor = meshRenderer.material.color;
        }
    }
    void Update()
    {
        // Update the timer for the current state.
        stateTimer += Time.deltaTime;

        // Handle state transitions and behavior based on the current state.
        switch (currentState) {
            case BossState.Idle:
                HandleIdle();
                break;
            case BossState.Telegraphing:
                HandleTelegraphing();
                break;
            case BossState.Charging:
                HandleCharging();
                break;
            case BossState.Recovering:
                HandleRecovering();
                break;
            case BossState.Shooting:
                HandleShooting();
                break;
        }
    }

    void ChangeState(BossState newState)
    {
        // Reset the timer and update the current state.
        currentState = newState;
        stateTimer = 0;    
        Debug.Log("Boss cambió a: " + newState);
    }

    protected override void MoveEnemy(){ } // The boss's movement is handled in the charging state, so we override this method to do nothing.

    void HandleIdle()
    {
        // In the idle state, the boss will simply move towards the player at a normal speed until they are within the charge range.
        if (player != null) 
        {
            // Calculate the distance to the player.
            float distance = Vector3.Distance(transform.position, player.transform.position);
            // Make the boss face the player while idle.
            transform.LookAt(player.transform);

            // If the player is outside the charge range, move towards them at a normal speed.
            if (distance > chargeRange)
            {
                // Move towards the player at a normal speed.
                Vector3 direction = (player.transform.position - transform.position).normalized;
                enemyRb.linearVelocity = direction * enemyStats.speed;
            }
            // If the player is within the charge range, prepare to charge.
            else
            {
                // If the player is within the charge range, prepare to charge.
                enemyRb.linearVelocity = Vector3.zero;
                ChangeState(BossState.Telegraphing);
                Debug.Log("Boss State: " + currentState);
            }
        }
    }

    void HandleTelegraphing()
    {
        if(stateTimer <= Time.fixedDeltaTime)
        {
            // Calculate the direction to charge towards the player, ignoring the y-axis to keep the boss on the same plane.
            Vector3 dir = (player.transform.position - transform.position);
            chargeDirection = new Vector3(dir.x, 0, dir.z).normalized;
            transform.forward = chargeDirection;
            // Change the boss's color to indicate that it is telegraphing an attack.
            if (meshRenderer != null)
            {
                meshRenderer.material.color = telegraphColor;
            }

            Debug.Log("Target locked: " + chargeDirection);
        }

        if (stateTimer >= telegraphDuration)
        {
            // After the telegraphing duration, switch to the charging state.
            ChangeState(BossState.Charging);

            // Reset the boss's color back to the original color.
            if (meshRenderer != null)
            {
                meshRenderer.material.color = originalColor;
            }
        }
    }

    void HandleCharging()
    {
        enemyRb.linearVelocity = chargeDirection * chargeSpeed;
        if(stateTimer >= chargeDuration)
        {
            enemyRb.linearVelocity = Vector3.zero;
            if (!hitPlayerDuringCharge)
            {
                failCharges++;
                Debug.Log("Boss failed to hit the player during charge. Fail charges: " + failCharges);
            }
            else
            {
                Debug.Log("Boss successfully hit the player during charge.");
            }
            hitPlayerDuringCharge = false; // Reset the flag for the next charge attempt.
            Debug.Log("Boss finished charging, switching to recovering state.");
            ChangeState(BossState.Recovering);
        }
    }

    void HandleRecovering()
    {
        enemyRb.linearVelocity = Vector3.zero;
        if (stateTimer >= recoverDuration)
        {
            if (failCharges == 3)
            {
                // If the boss has failed to hit the player 3 times, switch to the shooting state.
                Debug.Log("Boss has failed to hit the player 3 times, switching to shooting state.");
                ChangeState(BossState.Shooting);
            }
            else
            {
                // After the recovery duration, switch back to the idle state to start the cycle again.
                Debug.Log("Boss finished recovering, switching back to idle state.");
                ChangeState(BossState.Idle);
            }
        }
    }

    void HandleShooting()
    {
        // During the shooting state, the boss will shoot towards the player for a short duration before switching back to the idle state.
        if (stateTimer <= Time.fixedDeltaTime)
        {
            transform.LookAt(player.transform);
            Shoot();
        }

        // After shooting, the boss will switch back to the idle state to start the cycle again.
        if (stateTimer >= 1f)
        {
            // Reset the fail charge counter after shooting to give the boss a fresh start for the next cycle.
            failCharges = 0;
            Debug.Log("Boss finished shooting, switching back to idle state.");
            ChangeState(BossState.Idle);
        }
    }

    protected override void OnCollisionEnter(Collision other)
    {
        // Check if the boss collided with the player during the charge.
        if (other.gameObject.TryGetComponent<PlayerActions>(out PlayerActions player))
        {   
            player.TakeDamage(enemyStats.contactDamage);
            if (currentState == BossState.Charging)
            {
                // If the boss is currently charging and hits the player, set the flag to indicate a successful hit and switch to the recovering state.
                hitPlayerDuringCharge = true;
                enemyRb.linearVelocity = Vector3.zero;
                Debug.Log("Boss hit the player during charge, switching to recovering state.");
                ChangeState(BossState.Recovering);
            }
        }

        // Check if the boss collided with a wall during the charge.
        if (other.gameObject.CompareTag("Wall"))
        {
            if (currentState == BossState.Charging)
            {
                // If the boss is currently charging and hits a wall, reset the flag for hitting the player, stop the boss's movement, increment the fail charge counter, and switch to the recovering state.
                hitPlayerDuringCharge = false;
                enemyRb.linearVelocity = Vector3.zero;
                failCharges++;
                Debug.Log("Boss hit a wall during charge, switching to recovering state.");
                ChangeState(BossState.Recovering);
            }
        }
    }

    protected override void Shoot()
    {
        // Calculate the direction from the boss to the player, ignoring the y-axis to keep the bullets on the same plane.
        Vector3 direction = player.transform.position - transform.position;
        Vector3 shootDirection = new Vector3(direction.x, 0, direction.z).normalized;

        // Create an array to hold the different directions for the bullets, with some variation to create a wider attack pattern.
        Vector3[] directions = new Vector3[5];
        for(int i = 0; i < directions.Length; i++)
        {
            // Create a spread of directions for the bullets, with some variation to create a wider attack pattern.
            directions[i] = Quaternion.AngleAxis(10f * (i - 2), Vector3.up) * shootDirection;
        }

        // Position the fire point slightly in front of the enemy to prevent immediate collision with the bullet
        firePoint.localPosition = Vector3.forward * 1.2f;

        // Instantiate the bullets and set their properties based on the enemy's stats.
        GameObject[] bullets = new GameObject[5];
        for (int i = 0; i < 5; i++)
        {
            // Instantiate the bullet at the fire point's position and rotation, and store it in the bullets array.
            GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullets[i] = newBullet;
        }

        // Ignore collisions between the bullets and the boss to prevent the bullets from hitting the boss immediately after being fired.
        for (int i = 0; i < bullets.Length; i++)
        {
            Physics.IgnoreCollision(bullets[i].GetComponent<Collider>(), GetComponent<Collider>());
        }

        // Ignore collisions between the bullets themselves to prevent them from colliding with each other and creating unintended behavior.
        for (int i = 0; i < bullets.Length; i++)
        {
            for (int j = i + 1; j < bullets.Length; j++)
            {
                Physics.IgnoreCollision(
                    bullets[i].GetComponent<Collider>(),
                    bullets[j].GetComponent<Collider>()
                );
            }
        }

        // Launch each bullet in the corresponding direction with the enemy's stats.
        for (int i = 0; i < bullets.Length; i++)
        {
            // Check if the bullet has a Bullet script component, and if so, call the Launch method to set its properties and behavior.
            if (bullets[i].TryGetComponent<Bullet>(out Bullet bulletScript))
            {
                bulletScript.Launch(directions[i], enemyStats.bulletSpeed, enemyStats.range, enemyStats.damage);
            }
        }
    }
}
