using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400;							// Amount of force added when the player jumps.
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField]int runSpeed = 50;

    public Transform meleeWeapon;
    public Transform rangedWeapon;
    public Transform shield;
    public Transform swastika;

    private Vector3 m_Velocity = Vector3.zero;
    private bool m_Grounded;            // Whether or not the player is grounded.
    private Rigidbody2D m_Rigidbody2D;
    private bool lookingRight = true;
    int jumpsAvailable = 2;
    bool jump = false;
    bool glide = false;
    List<Collider2D> currentColiisions;
    float horizontalMove = 0;

    bool dash = false;
    bool hasDashAvailable = true;
    float defaultGravity;
    bool isMovingTowardsWall = false;
    bool isWallToMyLeft;

    public bool canMelee;
    public bool canRanged;
    public bool canGroundDash;
    public bool canAirDash;
    public bool canShield;
    public bool canDoubleJump;
    public bool canWallslide;
    public bool canGlide;


    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        currentColiisions = new List<Collider2D>();
        defaultGravity = m_Rigidbody2D.gravityScale;

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
            glide = true;
        }
        if (Input.GetButtonUp("Jump"))
        {
            glide = false;
        }

        if (Input.GetButtonDown("Melee"))
        {
            MeleeAttack();
        }

        if (Input.GetButtonDown("Ranged"))
        {
            RangedAttack();
        }

        if (Input.GetButtonDown("Shield"))
        {
            Shield();
        }
        if (Input.GetButtonDown("Dash"))
        {
            dash = true;
        }
    }

    private void FixedUpdate()
    {
        //m_Grounded = false;

        isMovingTowardsWall = false;

        foreach (Collider2D collider in currentColiisions)
        {
            if (collider.gameObject.layer.Equals(LayerMask.NameToLayer("wall")))
            {
                isWallToMyLeft = collider.gameObject.transform.position.x < transform.position.x;
                if ((isWallToMyLeft && horizontalMove < 0) || (!isWallToMyLeft && horizontalMove > 0))
                    isMovingTowardsWall = true;
            }
        }
        /*
        foreach (Collider2D collision in currentColiisions)
        {
            if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("ground")))
            {
                m_Grounded = true;
                jumpsAvailable = 1;
            }
        }*/
        Debug.Log("Velocity: " + m_Rigidbody2D.velocity.y + ", glide: "+glide);
        if (m_Rigidbody2D.velocity.y <= -0.3 && glide && !m_Grounded)
        {
            if (!canGlide)
            {
                if(m_Rigidbody2D.velocity.y <= -2)
                    showSwastika();
            }
            else
                m_Rigidbody2D.gravityScale = 0.55f;
        }
        else if(glide)
        {
            m_Rigidbody2D.gravityScale = defaultGravity;
        }
        Move(horizontalMove * Time.fixedDeltaTime);
        jump = false;
        dash = false;

    }

    public void Move(float move)
    {
        if (move > 0)
            lookingRight = true;
        if (move < 0)
            lookingRight = false;
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
        if (isMovingTowardsWall)
        {
            if (!canWallslide)
            {
                showSwastika();
            }
            else
            {
                m_Rigidbody2D.gravityScale = 0.5f;
                targetVelocity.y = 0;
            }
        }
        else if(!glide)
        {
            m_Rigidbody2D.gravityScale = defaultGravity;
        }
        // And then smoothing it out and applying it to the character
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        if (dash)
        {
            if (m_Grounded)
            {

                if (!canGroundDash)
                {
                    showSwastika();
                }
                else
                {
                    float horizontalForce = lookingRight ? m_JumpForce : -m_JumpForce;
                    horizontalForce *= 4;
                    m_Rigidbody2D.AddForce(new Vector2(horizontalForce, 0f));
                }
            }
            else
            {
                if (!canAirDash)
                {
                    showSwastika();
                }
                else
                {
                    float horizontalForce = lookingRight ? m_JumpForce : -m_JumpForce;
                    horizontalForce *= 4;
                    if (hasDashAvailable)
                    {
                        m_Rigidbody2D.AddForce(new Vector2(horizontalForce, 0f));
                        hasDashAvailable = false;
                    }
                }
            }
        }

        // If the player should jump...

        /*if (m_Grounded && jump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            Debug.Log("added jump force");
            jumpsAvailable--;
        }*/
        if (jump)
        {
            if (isMovingTowardsWall && canWallslide)
            {
                float horizontalForce = isWallToMyLeft ? m_JumpForce : -m_JumpForce;
                horizontalForce *= 4;
                m_Rigidbody2D.AddForce(new Vector2(horizontalForce, m_JumpForce));
            }
            else if (m_Grounded)
            {
                m_Grounded = false;
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                jumpsAvailable--;
            }
            else if (jumpsAvailable > 0)
            {
                if (!canDoubleJump)
                {
                    showSwastika();
                }
                else
                {
                    m_Grounded = false;
                    m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                    jumpsAvailable--;
                }
            }
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentColiisions.Add(collision.collider);
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("ground")))
        {
            m_Grounded = true;
            hasDashAvailable = true;
            jumpsAvailable = 2;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        currentColiisions.Remove(collision.collider);
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("ground")))
        {
            m_Grounded = false;
        }
    }

    public void Shield()
    {
        if (!canShield)
        {
            showSwastika();
            return;
        }
        if (shield.gameObject.activeSelf == false)
        {
            Debug.Log("shield");
            shield.gameObject.SetActive(true);
            gameObject.GetComponent<Player>().Shield(/*shield duration: */ 1f);
            Invoke("disableShield", 1f);
        }
    }
    private void disableShield()
    {
        shield.gameObject.SetActive(false);
    }

    public void MeleeAttack()
    {
        if (!canMelee)
        {
            showSwastika();
            return;
        }
        if (meleeWeapon.gameObject.activeSelf == false)
        {
            Debug.Log("melee");
            meleeWeapon.gameObject.SetActive(true);
            Invoke("disableMelee", 1f);
        }
    }
    public void RangedAttack()
    {
        if (!canRanged)
        {
            showSwastika();
            return;
        }
        if (rangedWeapon.gameObject.activeSelf == false)
        {
            Debug.Log("ranged");
            rangedWeapon.gameObject.SetActive(true);
            rangedWeapon.gameObject.GetComponent<RangedWeapon>().Shoot(lookingRight);
            Invoke("disableRanged", 0.5f);
        }
    }
    private void disableRanged()
    {
        rangedWeapon.gameObject.SetActive(false);

    }
    private void disableMelee()
    {
        meleeWeapon.gameObject.SetActive(false);

    }
    private void showSwastika()
    {
        if (!swastika.gameObject.activeSelf)
        {
            swastika.gameObject.SetActive(true);
            Invoke("hideSwastika", 0.5f);
        }
    }
    private void hideSwastika()
    {
        swastika.gameObject.SetActive(false);
    }

}
