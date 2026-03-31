using UnityEngine;

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

            // If the player is outside the charge range, move towards them at a normal speed.
            if (distance > chargeRange)
            {
                // Move towards the player at a normal speed.
                Vector3 direction = (player.transform.position - transform.position).normalized;
                GetComponent<Rigidbody>().linearVelocity = direction * enemyStats.speed;
            }
            // If the player is within the charge range, prepare to charge.
            else
            {
                // If the player is within the charge range, prepare to charge.
                GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
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
        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + chargeDirection * chargeSpeed * Time.fixedDeltaTime);
        if(stateTimer >= chargeDuration)
        {
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
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
        //TODO
    }

    void HandleShooting()
    {
        //TODO 
    }

    protected override void OnCollisionStay(Collision other)
    {
        // Check if the boss collided with the player during the charge.
        if (other.gameObject.TryGetComponent<PlayerActions>(out PlayerActions player))
        {   
            player.TakeDamage(enemyStats.contactDamage);
            if (currentState == BossState.Charging)
            {
                // If the boss is currently charging and hits the player, set the flag to indicate a successful hit and switch to the recovering state.
                hitPlayerDuringCharge = true;
                GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                ChangeState(BossState.Recovering);
                Debug.Log("Boss hit the player during charge, switching to recovering state.");
            }
        }
    }
}
