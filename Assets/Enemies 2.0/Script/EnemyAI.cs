using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround,
        whatIsPlayer;
    public float yOffset; // Adjust this value as needed

    public Animator animator;

    // Patrol variables
    private Vector3 patrolPoint;
    private bool patrolPointSet;
    public float patrolPointRange;

    // Attack variables
    private bool alreadyAttacked;
    public float timeBetweenAttacks;

    // States
    public float sightRange,
        attackRange;

    private bool playerInSightRange,
        playerInAttackRange;

    public delegate void EnemyDeathHandler(GameObject enemy);
    public event EnemyDeathHandler OnEnemyDeath;

    // public event Action<GameObject> OnEnemyDeath; // Event for enemy death

    // Call this method when the enemy dies


    private void Awake()
    {
        player = GameObject.Find("Player 1").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Check if the player is in sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // Perform line-of-sight check
        bool hasLineOfSight = false;
        if (playerInSightRange || playerInAttackRange)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            RaycastHit hit;

            // Cast a ray towards the player, checking for obstacles
            if (
                Physics.Raycast(
                    transform.position + Vector3.up * yOffset,
                    directionToPlayer.normalized,
                    out hit,
                    sightRange
                )
            )
            {
                if (hit.collider.CompareTag("Player")) // Ensure the ray hits the player
                {
                    hasLineOfSight = true;
                }
            }
        }

        // Only proceed if the player is within sight and the enemy has a clear line of sight
        if (!playerInSightRange && !playerInAttackRange)
            Patroling();
        else if (playerInSightRange && !playerInAttackRange && hasLineOfSight)
            ChasePlayer();
        else if (playerInSightRange && playerInAttackRange && hasLineOfSight)
            AttackPlayer();
    }

    private void Patroling()
    {
        if (!patrolPointSet)
            SearchPatrolPoint();

        if (patrolPointSet)
        {
            agent.SetDestination(patrolPoint);
            animator.SetBool("isRunning", true); // Trigger running animation
        }

        Vector3 distanceToPatrolPoint = transform.position - patrolPoint;

        // Patrol point reached
        if (distanceToPatrolPoint.magnitude < 1f)
        {
            patrolPointSet = false;
            animator.SetBool("isRunning", false); // Stop running animation
        }
    }

    private void SearchPatrolPoint()
    {
        // Randomly set a patrol point within the defined range
        float randomZ = Random.Range(-patrolPointRange, patrolPointRange);
        float randomX = Random.Range(-patrolPointRange, patrolPointRange);

        patrolPoint = new Vector3(
            transform.position.x + randomX,
            transform.position.y,
            transform.position.z + randomZ
        );

        // Check if the point is on the ground
        if (Physics.Raycast(patrolPoint, -transform.up, 2f, whatIsGround))
            patrolPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animator.SetBool("isRunning", true); // Trigger running animation
    }

    private void AttackPlayer()
    {
        if (alreadyAttacked)
            return; // Exit if cooldown is active

        // Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        animator.SetBool("isRunning", false); // Stop running animation
        animator.SetTrigger("isAttacking"); // Trigger attack animation

        // Get a projectile from the pool
        GameObject proj = ObjectPool.Instance.GetPooledObject();
        if (proj != null)
        {
            // Set the starting position of the projectile with an offset from the enemy’s y position
            Vector3 projectilePosition = new Vector3(
                transform.position.x,
                transform.position.y + yOffset, // Offset from the enemy's y position
                transform.position.z
            );

            proj.transform.position = projectilePosition;

            // Set the projectile to look towards the player’s position with a fixed y-level targeting
            Vector3 targetPosition = new Vector3(
                player.position.x,
                player.position.y,
                player.position.z
            ); // Target player's x and z, fixed y
            proj.transform.LookAt(targetPosition);

            proj.SetActive(true); // Activate the projectile

            // Optionally set the projectile's velocity if using Rigidbody
            Rigidbody rb = proj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = (targetPosition - proj.transform.position).normalized;
                rb.velocity = direction * rb.velocity.magnitude; // Set the velocity towards the target position
            }
        }

        // Start cooldown timer
        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void takeDamageFromPlayer()
    {
        if (agent != null)
        {
            Die(); // Trigger dissolve animation
        }
    }

    private void Die()
    {
        // Invoke the OnEnemyDeath event
        OnEnemyDeath?.Invoke(gameObject); // Notify subscribers about this enemy’s death
    }
}
