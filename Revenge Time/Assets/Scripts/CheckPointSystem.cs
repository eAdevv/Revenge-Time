using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointSystem : MonoBehaviour
{
    public GameObject[] checkPoints;
    public int element = 1;
    PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerMovement.checkPoint = checkPoints[0];
    }

    public void checkPointControl()
    {
        if (checkPoints[element].GetComponent<BoxCollider2D>().isTrigger == true)
        {
            playerMovement.checkPointChange(checkPoints[element]);

            if(element < checkPoints.Length - 1)
            {
                element += 1;
            }
        }
    }

}
