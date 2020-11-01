using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] ParticleSystem deathAnimation;
    public Transform leftEdge;
    public Transform rightEdge;
    private Rigidbody2D m_Rigidbody2D;
    float patrolSpeed = 2f;
    float defaultPatrolSpeed = 2f;
    bool facingRight = true;
    bool isAttacking = false;
    bool canMove = true;
    public Transform PlayerTransform;
    public Transform attackIndicator;
    public Transform weapon;

    public Image HealthBar;
    int hp = 100;

    public int damage = 10;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {

    }



    private void FixedUpdate()
    {
        if (isAttacking)
        {
            if(Math.Abs(PlayerTransform.position.x - transform.position.x) < 2f && !weapon.gameObject.activeSelf)
            {
                if (!weapon.gameObject.activeSelf)
                {
                    animator.SetBool("Attack", true);
                    weapon.gameObject.SetActive(true);
                    Invoke("SheateWeapon", 1f);
                }
            }
            bool isPlayerToMyLeft = PlayerTransform.position.x < transform.position.x;
            facingRight = isPlayerToMyLeft ? false : true;
            patrolSpeed = facingRight ? defaultPatrolSpeed : -defaultPatrolSpeed;
            if (facingRight && closeEnough(rightEdge.localPosition.x, transform.localPosition.x))
            {
                patrolSpeed = 0;
            }
            else if (!facingRight && closeEnough(leftEdge.localPosition.x, transform.localPosition.x))
            {
                patrolSpeed = 0;
            }
            
        }
        else
        {
            if (facingRight && closeEnough(rightEdge.localPosition.x, transform.localPosition.x))
            {
                facingRight = !facingRight;
                patrolSpeed = -defaultPatrolSpeed;
            }
            else if (!facingRight && closeEnough(leftEdge.localPosition.x, transform.localPosition.x))
            {
                facingRight = !facingRight;
                patrolSpeed = defaultPatrolSpeed;
            }
        }
        if (canMove)
        {
            m_Rigidbody2D.velocity = new Vector2(patrolSpeed, 0);
            animator.SetBool("Move", true);
        }
        else
            animator.SetBool("Move", false);
        if (facingRight)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    internal void startAttack()
    {
        isAttacking = true;
        attackIndicator.gameObject.SetActive(true);
        FindObjectOfType<AudioManager>().Play("Alert");
    }

    internal void stopAttack()
    {
        isAttacking = false;
        attackIndicator.gameObject.SetActive(false);
    }

    private bool closeEnough(float x1, float x2)
    {
        return Math.Abs(x1 - x2) < 0.05;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        MeleeWeapon melee = collider.gameObject.GetComponent<MeleeWeapon>();
        if (melee != null)
        {
            Damage(melee.getPower(), 100, melee.HittingRight());

        }
        RangedWeapon ranged = collider.gameObject.GetComponent<RangedWeapon>();
        if (ranged != null)
        {
            Damage(ranged.getPower(), 20, ranged.HittingRight());

        }

        Bullet bullet = collider.gameObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            Damage(bullet.getPower(), 50, bullet.HittingRight());

        }
    }
    public void Damage(int baseDamage, int knockback, bool hittingRight)
    {
        if (hp <= 0) return;
        if (hittingRight)
            m_Rigidbody2D.AddForce(new Vector2(knockback, 0));
        else
            m_Rigidbody2D.AddForce(new Vector2(-knockback, 0));
        if (knockback > 0)
        {
            if (canMove)
            {
                canMove = false;
                Invoke("EnableMove", 1f);
            }
        }
        hp -= baseDamage;
        HealthBar.fillAmount = hp / 100f;
        Debug.Log("Enemy took " + baseDamage + " damage and has " + hp + " hp left");
        if (hp <= 0)
            Die();
    }
    private void SheateWeapon()
    {
        animator.SetBool("Attack", false);
        weapon.gameObject.SetActive(false);
    }
    private void EnableMove()
    {
        canMove = true;
    }

    private void Die()
    {
        damage = 0;
        animator.SetTrigger("Death");
        FindObjectOfType<AudioManager>().Play("EnemyDeath");
        //deathAnimation.gameObject.SetActive(true);
        Destroy(gameObject.transform.parent.gameObject, 1f);
    }
}
