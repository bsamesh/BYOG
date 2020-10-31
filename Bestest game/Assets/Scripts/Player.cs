using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    static int hp = 100;
    static bool isShielded = false;
    static float shieldDuration = 0;
    public Image ShieldBar;

    // Start is called before the first frame update
    void Start()
    {
        
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

    public static bool Damage(int baseDamage)
    {
        if(hp <= 0) return false;
        Debug.Log("player took " + baseDamage + " damage and has " + hp + "hp left");
        if (isShielded) return false;
        hp -= baseDamage;
        if (hp <= 0)
        {
            Die();
            return false;
        }
        Shield(2f);
        return true;
    }

    private static void Die()
    {
        //death
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
    }
}
