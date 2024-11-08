using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float yOffset; // Adjust this value as needed
    public AudioSource audioSource; // Audio source for playing sounds

    public AudioSource hitAudio; // Audio source for playing hit sounds

    public Animator animator;

   
    // Patrol variables
    private Vector3 patrolPoint;
    private bool patrolPointSet;
    public float patrolPointRange;

    // Attack variables
    private bool alreadyAttacked;
    public float timeBetweenAttacks;

    // States 
    public float sightRange, attackRange;
    
    private bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player 1").transform;
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>(); // Reference to AudioSource
    }

    private void Update()
    {
        // Check if the player is in sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        else if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        else if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!patrolPointSet) SearchPatrolPoint();

        if (patrolPointSet)
        {
            agent.SetDestination(patrolPoint);
            animator.SetBool("isRunning", true);  // Trigger running animation
        }

        Vector3 distanceToPatrolPoint = transform.position - patrolPoint;

        // Patrol point reached
        if (distanceToPatrolPoint.magnitude < 1f)
        {
            patrolPointSet = false;
            animator.SetBool("isRunning", false);  // Stop running animation
        }
    }

    private void SearchPatrolPoint()
    {
        // Randomly set a patrol point within the defined range
        float randomZ = Random.Range(-patrolPointRange, patrolPointRange);
        float randomX = Random.Range(-patrolPointRange, patrolPointRange);

        patrolPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // Check if the point is on the ground
        if (Physics.Raycast(patrolPoint, -transform.up, 2f, whatIsGround))
            patrolPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animator.SetBool("isRunning", true);  // Trigger running animation
    }

private void AttackPlayer()
{
    if (alreadyAttacked) return; // Exit if cooldown is active

    // Make sure enemy doesn't move
    agent.SetDestination(transform.position);
    transform.LookAt(player);

    animator.SetBool("isRunning", false);  // Stop running animation
    animator.SetTrigger("isAttacking");    // Trigger attack animation

    // Play attack sound
        if (audioSource != null )
        {
            audioSource.Play();
        }

    // Attack logic
    Debug.Log("Attacking");

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
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y, player.position.z); // Target player's x and z, fixed y
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

    private void TakeDamage(int damage)
    {
        Destroy(gameObject);
    }

    // private void OnDrawGizmosSelected()
    // {
    //     // Change color for attack range (red)
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, attackRange);

    //     // Change color for sight range (yellow)
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(transform.position, sightRange);
    // }

    public void takeDamageFromPlayer()
    {
        if (hitAudio != null)
        {
            hitAudio.Play(); // Play the hit sound
        }

        if (agent != null)
        {
            StartDissolve();  // Trigger dissolve animation
        }
        // Destroy the entire GameObject
        Destroy(gameObject);
    }

    public void StartDissolve()
    {
        animator.SetTrigger("isDissolving");  // Trigger dissolve animation
    }
}
