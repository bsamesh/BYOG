using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    int damage = 30;
    float velocity = 800f;

    private bool hittingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    //left indicates if bullet should be fired to the left
    public void Shoot(bool right)
    {
        hittingRight = right;
        if(right)
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(velocity, 0f));
        else
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-velocity, 0f));
        //transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + 200, transform.position.y,transform.position.z), velocity);
    }
    // Update is called once per frame
    void Update()
    {
        
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
