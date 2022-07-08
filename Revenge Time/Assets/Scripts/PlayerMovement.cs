using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{
    [Header("Health and Movement")]
    public float playerHealth;
    public float currentPlayerHealth;
    public int dieCount = 0;
    [Space]
    public float runSpeed = 40f;
    public bool jump = false;

    [Header("For Boss Fight")]
    public GameObject firing; // For Boss Fire Attack
    public bool isFiring = false; // For Boss Fire Attack
    public bool bossActive;

    [Header("Necessary Objects")]
    public AudioClip playerJump;
    public AudioClip playerHurt;
    public AudioClip gameOver_ ;
    [Space]
    public Animator animator;
    public GameObject checkPoint;
    public GameObject bossHealthBar;
    public GameObject gameOverScreen;
    public GameObject[] m_hearths;

    private bool crouch = false;
    private float horizontalMove = 0f;
    private CharacterController2D controller;
    private GameObject gameBgMusic, bossBgMusic;

    Respawn respawn;


    void Start()
    {
        Time.timeScale = 1;
        bossActive = false;
        controller = GameObject.Find("Player").GetComponent<CharacterController2D>();
        respawn = GameObject.Find("RespawnControl").GetComponent<Respawn>();
        currentPlayerHealth = playerHealth;
        gameBgMusic = GameObject.Find("Game_Music");
        bossBgMusic = GameObject.Find("Boss_Music");
        bossBgMusic.SetActive(false);
        Cursor.visible = false;
    }

    void Update()
    {
        #region Movement
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump") && Time.timeScale > 0)
        {
            jump = true;
            animator.SetBool("isJumping", jump);
            GetComponent<AudioSource>().PlayOneShot(playerJump,0.5f);
        }

        if (Input.GetButtonDown("Crouch") && Time.timeScale > 0)
            crouch = true;
        else if (Input.GetButtonUp("Crouch"))
            crouch = false;
        #endregion

        #region Die
        if (currentPlayerHealth <= 0 && dieCount <= 3)
        {
            this.gameObject.SetActive(false);
            dieCount += 1;

        }
        #endregion

        #region Firing Control
        if (isFiring)
        {
            currentPlayerHealth -= 0.15f;
            firing.SetActive(true);
        }
        else
        {
            firing.SetActive(false);
        }
        #endregion

        if(GameObject.Find("TheEnd") == true)
        {
            for(int i = 0; i<=m_hearths.Length-1; i++)
            {
                m_hearths[i].SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
    }

    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("isCrouching", isCrouching);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag.Equals("Portal_start"))
        {
            Debug.Log("Sa");
            gameObject.transform.position = GameObject.FindGameObjectWithTag("Portal_end").transform.position;
        }


        if (collision.gameObject.tag.Equals("Obstacle"))
        {
            currentPlayerHealth = 0;
        }  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("BossWall"))
        {          
            bossHealthBar.SetActive(true);
            bossActive = true;
            gameBgMusic.SetActive(false);
            bossBgMusic.SetActive(true);

        }

        if(collision.gameObject.tag.Equals("Detect"))
        {
            GameObject.FindGameObjectWithTag("BossWall").GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }


    public void checkPointChange(GameObject newPoint)
    {
        checkPoint = newPoint;
    }

    public void playerDie()
    {
        m_hearths[dieCount - 1].SetActive(false);
        currentPlayerHealth = playerHealth;
        gameObject.SetActive(true);
        gameObject.transform.position = new Vector2(checkPoint.transform.position.x + 0.2f, checkPoint.transform.position.y + 0.2f);

        GameObject.FindGameObjectWithTag("BossWall").GetComponent<BoxCollider2D>().isTrigger = true;

        var pointControl_ = GameObject.Find("CheckPointController").GetComponent<CheckPointSystem>();
        var chapterControl_ = pointControl_.checkPoints[pointControl_.element].GetComponent<ChapterSystem>().chapterEnemies;
        //chapterControl_.GetComponent<HoldEnemy>().enemyRespawn();
        respawn.currentTime = respawn.respawnTime;
        respawn.respawnScreen.SetActive(false);


    }

    public void gameOver()
    {
        firing.SetActive(false);
        gameObject.SetActive(false);
        gameBgMusic.SetActive(false);
        bossBgMusic.SetActive(false);
        

        gameOverScreen.SetActive(true);

    }

   
} 
