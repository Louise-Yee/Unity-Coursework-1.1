using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int baseEnemiesPerSpawn = 1; // Initial number of enemies to spawn
    public float baseSpawnInterval = 5f; // Initial time between spawns in seconds
    public int maxEnemies = 20; // Maximum number of enemies allowed at any time

    // Ranges for spawning positions
    public float minX,
        maxX,
        minY,
        maxY,
        minZ,
        maxZ;
    public float minimumDistance = 5.0f;
    public float minimumSpawnDistanceFromPlayer = 10f; // Minimum distance from player for spawning enemies
    public float navMeshCheckRadius = 1.0f;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private List<Vector3> spawnPositions = new List<Vector3>();

    private float spawnInterval;
    private float spawnTimer;
    private int enemiesPerSpawn;

    // Variables for point-based difficulty scaling
    public int pointsPerDifficultyIncrease = 500; // Points required to scale difficulty
    private int lastDifficultyIncreasePoints = 0;

    // Reference to PlayerScore script
    private PlayerScore playerScore;
    public AudioSource deathSound;
    public Transform player;
    private bool isSpawningActive = false;  // Flag to control if spawning should be active

    private void Start()
    {
        playerScore = FindObjectOfType<PlayerScore>(); // Find the PlayerScore script in the scene
        spawnInterval = baseSpawnInterval;
        enemiesPerSpawn = baseEnemiesPerSpawn;
        SpawnFixedNumberOfEnemies(); // Initial spawn of enemies
    }

    private void Update()
    {
        if (!isSpawningActive) return;  // Skip Update logic if spawning is inactive

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval && activeEnemies.Count < maxEnemies)
        {
            SpawnFixedNumberOfEnemies();
            spawnTimer = 0;
        }

        if (playerScore != null && playerScore.currentScore - lastDifficultyIncreasePoints >= pointsPerDifficultyIncrease)
        {
            lastDifficultyIncreasePoints = playerScore.currentScore;
            ScaleDifficulty();
        }
    }

    public void SpawnFixedNumberOfEnemies()
    {
        // Always spawn a fixed number of enemies based on baseEnemiesPerSpawn
        int enemiesToSpawn = Mathf.Min(baseEnemiesPerSpawn, maxEnemies - activeEnemies.Count);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector3 spawnPosition;
            bool validPosition = false;
            int attempts = 0;

            while (!validPosition && attempts < 100)
            {
                Vector3 positionWithinRange = new Vector3(
                    Random.Range(minX, maxX),
                    Random.Range(minY, maxY),
                    Random.Range(minZ, maxZ)
                );

                if (
                    NavMesh.SamplePosition(
                        positionWithinRange,
                        out NavMeshHit hit,
                        navMeshCheckRadius,
                        NavMesh.AllAreas
                    )
                )
                {
                    spawnPosition = hit.position;

                    if (IsPositionValid(spawnPosition) && IsFarEnoughFromPlayer(spawnPosition))
                    {
                        spawnPositions.Add(spawnPosition);
                        GameObject enemy = Instantiate(
                            enemyPrefab,
                            spawnPosition,
                            Quaternion.identity
                        );
                        activeEnemies.Add(enemy);
                        Debug.Log("Enemy spawned at: " + spawnPosition);
                        Debug.Log("Total enemies: " + activeEnemies.Count);
                        enemy.GetComponent<EnemyAI>().OnEnemyDeath += HandleEnemyDeath;
                        validPosition = true;
                    }
                    else{
                        Debug.Log("Invalid position, trying again...");
                    }
                }
                attempts++;
            }
        }
    }

    private bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 existingPosition in spawnPositions)
        {
            if (Vector3.Distance(position, existingPosition) < minimumDistance)
            {
                return false;
            }
        }
        return true;
    }
    private bool IsFarEnoughFromPlayer(Vector3 position)
    {
        if (player == null) return true; // If no player reference, skip distance check
        return Vector3.Distance(position, player.position) >= minimumSpawnDistanceFromPlayer;
    }

    private void HandleEnemyDeath(GameObject enemy)
    {
        print("Enemy is dead");

        if (deathSound != null)
        {
            // Create a temporary GameObject to play the sound
            GameObject tempSoundObject = new GameObject("TempDeathSound");
            AudioSource tempAudioSource = tempSoundObject.AddComponent<AudioSource>();
            tempAudioSource.clip = deathSound.clip;
            tempAudioSource.Play();

            // Destroy the temp object after the sound has finished playing
            Destroy(tempSoundObject, deathSound.clip.length);
        }

        // Remove the enemy from the list and destroy the enemy GameObject immediately
        activeEnemies.Remove(enemy);
        Destroy(enemy);
    }

    private void ScaleDifficulty()
    {
        // Decrease the spawn interval, but don’t go below a certain threshold
        spawnInterval = Mathf.Max(1f, spawnInterval - 0.5f);

        // Optionally, you can also increase the difficulty in other ways, such as making enemies tougher
        Debug.Log($"Difficulty increased: spawnInterval = {spawnInterval}");
    }
    public void ActivateSpawning()
    {
        isSpawningActive = true;  // Method to activate spawning
    }
}
