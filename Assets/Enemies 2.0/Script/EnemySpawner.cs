using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Your enemy prefab
    public int numberOfEnemies; // Number of enemies to maintain at all times

    // Define ranges for x, y, and z coordinates
    public float minX, maxX;
    public float minY, maxY; // Typically the y-range will be around the ground height
    public float minZ, maxZ;
    public float minimumDistance; // Minimum distance between enemies
    public float navMeshCheckRadius = 1.0f; // Radius for NavMesh sampling

    private List<GameObject> activeEnemies = new List<GameObject>();
    private List<Vector3> spawnPositions = new List<Vector3>();

    private void Start()
    {
        SpawnEnemies();
    }

    private void Update()
    {
        // Check if we need to spawn more enemies
        if (activeEnemies.Count < numberOfEnemies)
        {
            SpawnEnemies();
        }
    }

    private void SpawnEnemies()
    {
        int enemiesToSpawn = numberOfEnemies - activeEnemies.Count;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector3 spawnPosition;
            bool validPosition = false;
            int attempts = 0;

            // Try to find a valid spawn position
            while (!validPosition && attempts < 100) // Limit attempts to avoid infinite loops
            {
                // Generate a position within specified x, y, z ranges
                Vector3 positionWithinRange = new Vector3(
                    Random.Range(minX, maxX),
                    Random.Range(minY, maxY),
                    Random.Range(minZ, maxZ)
                );

                Debug.Log($"Attempt {attempts + 1}: Generated position {positionWithinRange}");

                // Use NavMesh to find the correct position on the ground within range
                if (NavMesh.SamplePosition(positionWithinRange, out NavMeshHit hit, navMeshCheckRadius, NavMesh.AllAreas))
                {
                    spawnPosition = hit.position; // This uses the correct y position from the NavMesh

                    // Check if the position is valid (not too close to another enemy)
                    if (IsPositionValid(spawnPosition))
                    {
                        spawnPositions.Add(spawnPosition);
                        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                        activeEnemies.Add(enemy);
                        enemy.GetComponent<EnemyAI>().OnEnemyDeath += HandleEnemyDeath; // Subscribe to enemy death event
                        validPosition = true;
                        Debug.Log($"Spawned enemy at {spawnPosition}");
                    }
                }

                attempts++;
            }

            // If we couldn't find a valid position after 100 attempts, log a warning
            if (!validPosition)
            {
                Debug.LogWarning($"Failed to find a valid spawn position for enemy {i + 1} after {attempts} attempts.");
            }
        }
    }

    private bool IsPositionValid(Vector3 position)
    {
        // Check if the new spawn position is too close to any existing ones
        foreach (Vector3 existingPosition in spawnPositions)
        {
            if (Vector3.Distance(position, existingPosition) < minimumDistance)
            {
                return false; // Too close to another enemy
            }
        }
        return true; // Valid position
    }

    private void HandleEnemyDeath(GameObject enemy)
    {
        // Remove the enemy from the active enemies list and spawn a new one
        activeEnemies.Remove(enemy);
        Destroy(enemy);
    }
}
