using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]ParticleSystem deathAnimation;
    private Rigidbody2D m_Rigidbody2D;
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
    private void OnTriggerEnter2D(Collider2D collider)
    {
        MeleeWeapon melee = collider.gameObject.GetComponent<MeleeWeapon>();
        if (melee != null)
        {
            Damage(melee.getPower(),100, melee.HittingRight());

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

    public void Damage(int baseDamage,int knockback, bool hittingRight)
    {
        if(hittingRight)
            m_Rigidbody2D.AddForce(new Vector2(knockback, 0));
        else
            m_Rigidbody2D.AddForce(new Vector2(-knockback, 0));

        hp -= baseDamage;
        Debug.Log("Enemy took " + baseDamage + " damage and has " + hp +" hp left");
        if (hp <= 0)
            Die();
    }

    private void Die()
    {
        deathAnimation.gameObject.SetActive(true);
        Destroy(gameObject, 7f);
    }
}
