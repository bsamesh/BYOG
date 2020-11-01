using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RangedWeapon : MonoBehaviour
{
    int damage = 5;
    public Transform bullet;

    private bool shooting = false;
    private bool hittingRight = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public int getPower()
    {
        return damage;
    }
    public void Shoot(bool right)
    {
        hittingRight = right;
        if (!shooting) 
        {
            shooting = true;
            Transform temp = Instantiate(bullet, transform.position, transform.rotation);
            temp.gameObject.GetComponent<Bullet>().Shoot(right);
            Destroy(temp.gameObject, 0.7f);
            Invoke("StopShooting", 0.5f);
        }

    }
    public bool HittingRight()
    {
        return hittingRight;
    }
    private void StopShooting()
    {
        shooting = false;
    }
}
