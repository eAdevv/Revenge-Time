using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{

    [Header("Attack Settings")]
    public GameObject dagger;
    public Transform daggerPos;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public AudioClip playerAttack;

    public float attackRange = 0.5f;
    public int attackDamage = 40;
    public float attackRate = 2f;

    [Header("Dagger Settings")]
    public Text daggerCoolDownText;
    [SerializeField]private float daggerCoolDown = 3f;
    [SerializeField]private bool daggerAttack = false;
    [SerializeField]private float nextAttackTime = 0f;
    PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        daggerCoolDownText.text = "Shuriken  Cooldown : " + (int)daggerCoolDown;
    }

    
    void Update()
    {
        
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && Time.timeScale > 0)
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse1) && playerMovement.animator.GetBool("isJumping") == false && daggerCoolDown == 3f)
        {
            DaggerAttack();
            daggerAttack = true;
        }

        if (daggerCoolDown <= 0)
        {
            daggerCoolDown = 3f;
            daggerCoolDownText.text = "Shuriken  Cooldown : " + (int)daggerCoolDown;
            daggerAttack = false;
        }
        else if(daggerAttack == true && daggerCoolDown <= 3f)
        {
            daggerCoolDown -= Time.deltaTime;
            daggerCoolDownText.text = "Shuriken  Cooldown : " +(int)daggerCoolDown;
        }
    }

    void Attack()
    {
        playerMovement.animator.SetTrigger("Attack");
        GetComponent<AudioSource>().PlayOneShot(playerAttack,0.5f);
        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemyLayers);

        foreach(Collider2D enemy in hitEnemy)
        {
            enemy.GetComponent<EnemySystem>().TakeDamage(20);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void DaggerAttack()
    {     
        StartCoroutine(DaggerCreate());
    }
    IEnumerator DaggerCreate()
    {
        playerMovement.animator.SetTrigger("Dagger");
        GetComponent<AudioSource>().PlayOneShot(playerAttack,0.5f);
        yield return new WaitForSeconds(0.4f); // Catch the animation
        GameObject dagger_ = Instantiate(dagger, daggerPos.transform.position, daggerPos.transform.rotation) as GameObject;
        dagger_.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * 10, 0);
        Destroy(dagger_.gameObject, 2f);
    }


}
