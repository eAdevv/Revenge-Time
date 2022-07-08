using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldEnemy : MonoBehaviour
{
    public GameObject[] chapterEnemyArray;

   
    public void enemyRespawn()
    {
        for(int i = 0; i< chapterEnemyArray.Length; i++)
        {
            chapterEnemyArray[i].GetComponent<EnemySystem>().currentHealth = chapterEnemyArray[i].GetComponent<EnemySystem>().maxHealth;
        }
    }
}
