using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDaggerAttack : MonoBehaviour
{
    [SerializeField]private int daggerDamage = 10;
    EnemySystem  enemySystem;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            enemySystem = collision.gameObject.GetComponent<EnemySystem>();
            enemySystem.TakeDamage(daggerDamage);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag.Equals("Ground"))
        {
            Destroy(gameObject);
        }
    }


}
