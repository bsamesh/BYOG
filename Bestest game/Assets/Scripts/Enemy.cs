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
    bool facingRight = true;
    bool isAttacking = false;
    bool canMove = true;
    public Transform PlayerTransform;

    public Image HealthBar;
    int hp = 100;

    // Start is called before the first frame update
    void Start()
    {

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
            bool isPlayerToMyLeft = PlayerTransform.position.x < transform.position.x;
            facingRight = isPlayerToMyLeft ? false : true;
            patrolSpeed = facingRight ? Math.Abs(patrolSpeed) : -Math.Abs(patrolSpeed);
        }
        else
        {
            if (facingRight && closeEnough(rightEdge.localPosition.x, transform.localPosition.x))
            {
                facingRight = !facingRight;
                patrolSpeed = -patrolSpeed;
            }
            else if (!facingRight && closeEnough(leftEdge.localPosition.x, transform.localPosition.x))
            {
                facingRight = !facingRight;
                patrolSpeed = -patrolSpeed;
            }
        }
        if(canMove)
            m_Rigidbody2D.velocity = new Vector2(patrolSpeed, 0);
    }

    internal void startAttack()
    {
        isAttacking = true;
    }

    internal void stopAttack()
    {
        isAttacking = false;
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

    private void EnableMove()
    {
        canMove = true;
    }

    private void Die()
    {
        deathAnimation.gameObject.SetActive(true);
        Destroy(gameObject, 7f);
    }
}
