using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{

    public string enemyName;   // Name of the enemy (for reference)
    public GameObject enemyPrefab; // The enemy prefab to spawn
    public float rarity;  // The rarity of this enemy (0-1, where 0 = very rare, 1 = very common)
}

