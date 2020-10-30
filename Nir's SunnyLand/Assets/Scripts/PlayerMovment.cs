using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    public CharacterController2D controller;
    public GameObject deathMenuUI;
    public Animator animator;

    public float runSpeed = 20f;
    

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    bool paused = false;
    bool isDead = false;


    void Update()
    {
        if (paused)
            return;
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if(Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }


        if(Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        } else if(Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }

    }

    //player death
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.collider.GetComponent<Enemy>();
        if(!isDead && enemy != null)
        {
            Debug.Log("Player is dead!");
            paused = true;
            horizontalMove = 0f;
            isDead = true;
            animator.SetBool("IsDead", true);
            FindObjectOfType<AudioManager>().Play("PlayerDeath");
            deathMenuUI.SetActive(true);
            return;
        }
    }

    public void SetPaused(bool IsPaused)
    {
        paused = IsPaused;
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    } 

    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
    }

    private void FixedUpdate()
    {
        //move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime,crouch,jump);
        jump = false;

    }
}
