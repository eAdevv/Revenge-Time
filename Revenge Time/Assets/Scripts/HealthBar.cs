using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    private float playerCurrentHealth, playerMaxHealth, bossMaxHealth, bossCurrentHealth;
    PlayerMovement playerMovement;
    void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    
    void Update()
    {
        if(gameObject.tag.Equals("PlayerHealthBar"))
        {
            playerHealthBar();
        }
        else
        {
            bossHealthBar();
        }
        
    }


    void bossHealthBar()
    {
        if(playerMovement.bossActive == true)
        {
            bossMaxHealth = GameObject.Find("Boss").GetComponent<EnemySystem>().maxHealth;
            bossCurrentHealth = GameObject.Find("Boss").GetComponent<EnemySystem>().currentHealth;

            healthBar.fillAmount = bossCurrentHealth / bossMaxHealth;

            if(bossCurrentHealth <= 0)
            {
                playerMovement.bossActive = false;
                playerMovement.bossHealthBar.SetActive(false);
            }
        }
    }

    void playerHealthBar()
    {
        playerMaxHealth = playerMovement.playerHealth;
        playerCurrentHealth = playerMovement.currentPlayerHealth;

        healthBar.fillAmount = playerCurrentHealth / playerMaxHealth;
    }
}
