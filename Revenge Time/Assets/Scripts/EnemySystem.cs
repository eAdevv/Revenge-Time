using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySystem : MonoBehaviour
{
    [Header("Enemy General Settings")]
    #region General
    public int maxHealth = 100;
    public int currentHealth;
    public float player_boss_distance;
    [Space]
    public Animator animator;
    public AnimationClip attackAnimation;
    public GameObject hitBoxEnemy;
    public AudioClip enemyHurt , bossDead;
    public GameObject endGameScreen;

    #endregion

    [Header("Enemy Attack Settings")]
    #region Attack
    public float rayCastLength;
    public float attackDistance;
    public float moveSpeed;
    public float timer;
    public int enemyAttackDamage = 20;
    [Space]
    public Transform rayCast;
    public LayerMask raycastMask;
    public Transform leftLimit;
    public Transform rightLimit;

    private RaycastHit2D hit;
    private Transform target;
    private bool cooling;
    private bool attackMode;
    private bool inRange;
    private float intTimer;
    private float distance;
    #endregion

    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Awake()
    {
        SelectTarget();
        intTimer = timer;
    }

    private void Update()
    {

        if(!attackMode)
        {
            Move();
        }

        if(!InsideofLimits() && !inRange && !animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimation.ToString()))
        {
            SelectTarget();
        }
        if(inRange)
        {
            hit = Physics2D.Raycast(rayCast.position, transform.right, rayCastLength, raycastMask);
            RaycastDebugger();
        }

        if (hit.collider != null)
        {
            EnemyLogic();
        }
        else if (hit.collider == null)
        {
            inRange = false;
        }

        if (inRange == false)
        {
            StopAttack();
        }

        if(gameObject.name.Equals("Boss") && GameObject.Find("Player") == true)
        {
            player_boss_distance = Vector2.Distance(gameObject.transform.position , GameObject.Find("Player").GetComponent<Transform>().position);

            if(player_boss_distance < 5f)
            {
                animator.SetBool("isAttacking", false);
                animator.SetBool("isFireAttacking", true);
            }
            else
            {
                animator.SetBool("isFireAttacking", false);
            }

        }

    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            target = collision.transform;
            inRange = true;
            Flip();
        }
    }


    public void TakeDamage(int damage)
    {

        if (this.gameObject.name != "Boss")
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            GetComponent<AudioSource>().PlayOneShot(enemyHurt, 0.5f);
            currentHealth -= damage;
            animator.SetTrigger("Hurt");
            if (currentHealth <= 0)
            {
                Die();
            }
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(enemyHurt, 0.5f);
            currentHealth -= damage;

            if (animator.GetBool("isAttacking") != true && animator.GetBool("isDead") != true && animator.GetBool("isFireAttacking") != true)
            {
                animator.SetTrigger("Hurt");
            }

            if (currentHealth <= 0)
            {
                Die();
                GetComponent<AudioSource>().PlayOneShot(bossDead, 0.5f);
                GameObject.Find("Boss_Music").SetActive(false);
                endGameScreen.SetActive(true);
            }
        }

             

    }

    void Die()
    {
        animator.SetBool("isAttacking", false);
        animator.SetBool("isFireAttacking", false);
        animator.SetBool("isDead",true);
        hitBoxEnemy.SetActive(false);
        Destroy(gameObject, 3f);
        this.enabled = false;      
    }

    void RaycastDebugger()
    {
        if(distance > attackDistance)
        {
            Debug.DrawRay(rayCast.position, transform.right * rayCastLength, Color.red);
        }
        else if(attackDistance > distance)
        {
            Debug.DrawRay(rayCast.position, transform.right * rayCastLength, Color.green);
        }
    }

    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackDistance)
        {           
            StopAttack();
        }
        else if (attackDistance >= distance && cooling == false)
        {      
            EnemyAttack();
        }
        if (cooling)
        {
            animator.SetBool("isAttacking", false);
        }

    }

    void Move()
    {
        animator.SetBool("canWalk", true);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimation.ToString()))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed*Time.deltaTime);
        }
    }

    public void EnemyAttack()
    {      
        timer = intTimer;
        attackMode = true;
        hitBoxEnemy.SetActive(true);
        animator.SetBool("canWalk", false);
        animator.SetBool("isAttacking", true);

    }

    void StopAttack()
    {
        cooling = false;
        attackMode = false;
        hitBoxEnemy.SetActive(false);
        animator.SetBool("isAttacking", false);
    }

    private bool InsideofLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    private void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

        if(distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
        }

        Flip();
    }

    private void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if(transform.position.x > target.position.x)
        {
            rotation.y = 180f;
        }
        else 
        {
            rotation.y = 0f;      
        }

        transform.eulerAngles = rotation;
    }

}
