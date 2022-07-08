using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireArea : MonoBehaviour
{

    PlayerMovement playerMovement;
    private void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Player"))
        {
            if(!playerMovement.isFiring)
            {
                playerMovement.isFiring = true;   
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Player"))
        {
            if(playerMovement.isFiring)
            {
                playerMovement.isFiring = false;
            }
        }
    }

}
