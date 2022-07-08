using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    public EnemySystem enemySystem;
    PlayerMovement playerMovement;
    private void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Player"))
        {
            playerMovement.currentPlayerHealth -= enemySystem.enemyAttackDamage;
            playerMovement.gameObject.GetComponent<AudioSource>().PlayOneShot(playerMovement.playerHurt);
            playerMovement.animator.SetTrigger("Hurt");
        }
    }

    private void Update()
    {
        if(GetComponentInParent<EnemySystem>().animator.GetBool("isAttacking") == false)
        {
            this.gameObject.SetActive(false);
        }
    }




}
