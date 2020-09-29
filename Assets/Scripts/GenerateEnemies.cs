using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    public GameObject theEnemy;
    //public int xPos;
    //public int yPos;
    public int enemyCount;

    void Start()
    {
        StartCoroutine(EnemyDrop());
    }

    IEnumerator EnemyDrop() {
        while(enemyCount < 5){
            //xPos = Random.Range(-460, -460);
            //yPos = Random.Range(256, 256);
            Instantiate(theEnemy, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(10.0f);
            enemyCount += 1;
        }
    
    }
}
