using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterSystem : MonoBehaviour
{
    public GameObject chapterEnemies;
    CheckPointSystem checkPoint_;

    private void Start()
    {
        checkPoint_ = GameObject.Find("CheckPointController").GetComponent<CheckPointSystem>();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (chapterEnemies.transform.childCount <= 0)
            {
                chapterEnemies.SetActive(false);
            }
            if (chapterEnemies.activeInHierarchy == false)
            {
                gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                checkPoint_.checkPointControl();
            }
        }
    }
}
