using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static int maxHp = 100;
    public static int hp = maxHp;
    static bool isShielded = false;
    static float shieldDuration = 0;
    static float dmgDuration = 0;
    public Image ShieldBar;
    public static Action PlayerTookDamage;
    public static Animator animator;
    public static Action PlayerDied;
    public static bool lostControl = true;
    static SpriteRenderer m_SpriteRenderer;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.color = Color.white;
    }

    public static void Shield(float time)
    {
        isShielded = true;
        shieldDuration = time;
        //Invoke("Unshield", time);
    }
    public void Unshield()
    {
        isShielded = false;
    }
    public static void DMG(float time)
    {
        dmgDuration = time;
    }

    public static bool Damage(int baseDamage)
    {
        if (hp <= 0) return false;
        if (isShielded) return false;
        hp -= baseDamage;

        if (PlayerTookDamage != null)
        {
            PlayerTookDamage.Invoke();
            FindObjectOfType<AudioManager>().Play("Damaged");
        }

        Debug.Log("player took " + baseDamage + " damage and has " + hp + "hp left");
        if (hp <= 0)
        {
            Die();
            return false;
        }
        else
        {
            m_SpriteRenderer.color = Color.red;
            DMG(0.5f);
        }
        Shield(2f);

        return true;
    }

    private static void Die()
    {
        animator.SetTrigger("DeathTrigger");
        if (PlayerDied != null)
        {
            PlayerDied.Invoke();
            FindObjectOfType<AudioManager>().Play("PlayerDeath");
        }
    }
    public static void Crashed()
    {
        animator.SetTrigger("Crash");
    }
    // Update is called once per frame
    void Update()
    {
        if (shieldDuration > 0)
        {
            shieldDuration -= Time.deltaTime;
            ShieldBar.fillAmount = shieldDuration;
            if (shieldDuration <= 0)
                Unshield();
        }

        if (dmgDuration > 0)
        {
            dmgDuration -= Time.deltaTime;
            if (dmgDuration <= 0)
                m_SpriteRenderer.color = Color.white;
        }
    }
}
