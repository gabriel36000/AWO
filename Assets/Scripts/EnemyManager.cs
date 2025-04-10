using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public EnemyData[] enemyTypes; // Array of different enemy types (set in the editor)
    public int maxEnemies = 20; // Maximum number of enemies on the map
    public float spawnInterval = 3f; // Time between spawns

    private List<GameObject> activeEnemies = new List<GameObject>();
    private float spawnTimer = 0f;

    void Update()
    {
        spawnTimer += Time.deltaTime;

        activeEnemies.RemoveAll(enemy => enemy == null);

        if (spawnTimer >= spawnInterval && activeEnemies.Count < maxEnemies)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    void SpawnEnemy()
    {
        // Choose an enemy based on rarity
        EnemyData selectedEnemy = GetRandomEnemyBasedOnRarity();

        // Spawn the enemy
        GameObject newEnemy = Instantiate(selectedEnemy.enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity);
        activeEnemies.Add(newEnemy);
    }

    EnemyData GetRandomEnemyBasedOnRarity()
    {
        float totalRarity = 0f;

        // First, calculate the total rarity
        foreach (var enemy in enemyTypes)
        {
            totalRarity += enemy.rarity;
        }

        // Randomly pick an enemy based on the rarity
        float randomValue = Random.Range(0f, totalRarity);
        float cumulativeRarity = 0f;

        foreach (var enemy in enemyTypes)
        {
            cumulativeRarity += enemy.rarity;
            if (randomValue <= cumulativeRarity)
            {
                return enemy;
            }
        }

        // Fallback, should not happen
        return enemyTypes[0];
    }

    Vector3 GetRandomSpawnPosition()
    {
        // Define your spawn area, e.g., spawn within bounds of your game world
        float minX = -607.8f;  // Left
        float maxX = -322.2f;  // Right
        float minY = 168.6f;   // Bottom
        float maxY = 337.9f;   // Top

        // Randomly generate spawn position within the bounds
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        return new Vector3(x, y, 0f); // Assuming this is a 2D game, hence z = 0;
    }
}
