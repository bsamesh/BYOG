using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    int damage = 25;
    private bool hittingRight = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HitDirection(bool right)
    {
        hittingRight = right;
    }

    public bool HittingRight()
    {
        return hittingRight;
    }

    public int getPower()
    {
        return damage;
    }
}
