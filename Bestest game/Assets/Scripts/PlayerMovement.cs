using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400;							// Amount of force added when the player jumps.
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField]int runSpeed = 50;

    public Transform meleeWeapon;

    private Vector3 m_Velocity = Vector3.zero;
    private bool m_Grounded;            // Whether or not the player is grounded.
    private Rigidbody2D m_Rigidbody2D;
    int jumpsAvailable = 1;
    bool jump = false;
    List<Collider2D> currentColiisions;
    float horizontalMove = 0;


    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        currentColiisions = new List<Collider2D>();
    }

    void Start()
    {

    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;


        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Melee"))
        {
            Debug.Log("here");
            MeleeAttack();
        }

    }

    private void FixedUpdate()
    {
        m_Grounded = false;

        foreach(Collider2D collision in currentColiisions)
        {
            if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("ground")))
            {
                m_Grounded = true;
                jumpsAvailable = 1;
            }
        }
        
        Move(horizontalMove * Time.fixedDeltaTime);
        jump = false;

    }

    public void Move(float move)
    {
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
        // And then smoothing it out and applying it to the character
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        // If the player should jump...

        if (m_Grounded && jump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            Debug.Log("added jump force");
            jumpsAvailable--;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentColiisions.Add(collision.collider);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        currentColiisions.Remove(collision.collider);
    }

    public void MeleeAttack()
    {
        Debug.Log("melee");
        meleeWeapon.gameObject.SetActive(true);
        Invoke("disableMelee", 1f);
    }
    private void disableMelee()
    {
        meleeWeapon.gameObject.SetActive(false);

    }

}
