using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Respawn : MonoBehaviour
{
    PlayerMovement player;

    public float respawnTime;
    public float currentTime;
    public Text respawnTimeText;
    public GameObject respawnScreen;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        currentTime = respawnTime;
    }
    void Update()
    {
        if (player.dieCount <= 3)
        {
            if (player.currentPlayerHealth <= 0)
            {
                currentTime -= Time.deltaTime;             
                respawnTimeText.text = "You will respawn from last checkpoint in " + (int)currentTime + " seconds";
                if (player.dieCount != 3)
                {
                   respawnScreen.SetActive(true);
                }
                if (currentTime < 0)
                {
                   player.playerDie();
                }
            }
        }
        else
        {
            respawnScreen.SetActive(false);
            player.gameOver();
        }
    }
}
