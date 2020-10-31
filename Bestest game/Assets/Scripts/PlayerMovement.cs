using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400;							// Amount of force added when the player jumps.
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] int runSpeed = 50;

    public Transform meleeWeapon;
    public Transform rangedWeapon;
    public Transform shield;
    public Transform swastika;
    public Animator animator;

    private Vector3 m_Velocity = Vector3.zero;
    private bool m_Grounded;            // Whether or not the player is grounded.
    private Rigidbody2D m_Rigidbody2D;
    private bool lookingRight = true;
    int jumpsAvailable = 2;
    bool jump = false;
    bool glide = false;
    List<Collider2D> currentColiisions;
    float horizontalMove = 0;
    public bool shieldOnCooldown = false;

    bool dash = false;
    bool hasDashAvailable = true;
    float defaultGravity;
    bool isMovingTowardsWall = false;
    bool isWallToMyLeft;
    int spikeDamage = 20;

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
        animator = gameObject.GetComponent<Animator>();
        Player.PlayerDied += delegate { enabled = false; };
        UIManager.pausePressed += delegate { enabled = !enabled; };
    }

    void Update()
    {
        if (!Player.lostControl)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;


        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            glide = true;
        }
        if (Input.GetButtonUp("Jump"))
        {
            animator.SetBool("Glide", false);
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
    }

    private void FixedUpdate()
    {
        //m_Grounded = false;

        isMovingTowardsWall = false;

        foreach (Collider2D collider in currentColiisions)
        {
            if (collider == null || collider.gameObject == null)
                continue;                    
            if (collider.gameObject.layer.Equals(LayerMask.NameToLayer("Walls")))
            {
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
        if (m_Rigidbody2D.velocity.y <= -0.3 && glide && !m_Grounded)
        {
            if (!canGlide)
            {
                if (m_Rigidbody2D.velocity.y <= -2)
                    showSwastika();
            }
            else
            {
                m_Rigidbody2D.gravityScale = 0.55f;
                animator.SetBool("Glide", true);
            }
        }
        else if (glide)
        {
            m_Rigidbody2D.gravityScale = defaultGravity;
            animator.SetBool("Glide", false);
        }
        Move(horizontalMove * Time.fixedDeltaTime);
        jump = false;
        dash = false;

    }

    public void Move(float move)
    {
        if (move > 0)
        {
            lookingRight = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (move < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            lookingRight = false;
        }
        if (!animator.GetBool("AirDash"))
        {
            if (move != 0)
                animator.SetBool("Moving", true);
            if (m_Rigidbody2D.velocity.y > 0)
            {
                animator.SetBool("Fall", false);
            }
            else if (m_Rigidbody2D.velocity.y < -0.01)
            {
                animator.SetBool("Jump", false);
                animator.SetBool("DoubleJump", false);
                animator.SetBool("Fall", true);
            }
            else
            {
                animator.SetBool("Fall", false);
                animator.SetBool("DoubleJump", false);
                animator.SetBool("Jump", false);
            }
            if (move == 0)
                animator.SetBool("Moving", false);
        }
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
                animator.SetBool("WallSlide", true);
                m_Rigidbody2D.gravityScale = 1.1f;
                targetVelocity.y = 0;
            }
        }
        else if (!glide)
        {
            m_Rigidbody2D.gravityScale = defaultGravity;
            animator.SetBool("WallSlide", false);
        }
        else
        {
            animator.SetBool("WallSlide", false);
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
                    animator.SetBool("GDash", true);
                    Invoke("DisableGDash", 0.3f);
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
                        animator.SetBool("AirDash", true);
                        Invoke("DisableAirDash", 0.3f);
                        m_Rigidbody2D.AddForce(new Vector2(horizontalForce, 0f));
                        hasDashAvailable = false;
                    }
                }
            }
        }
        if (jump)
        {
            if (isMovingTowardsWall && canWallslide)
            {
                float horizontalForce = isWallToMyLeft ? m_JumpForce : -m_JumpForce;
                horizontalForce *= 2;
                m_Rigidbody2D.AddForce(new Vector2(horizontalForce, m_JumpForce));
            }
            else if (m_Grounded)
            {
                animator.SetBool("Jump", true);
                animator.SetBool("Fall", false);
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
                    animator.SetBool("DoubleJump", true);
                    m_Grounded = false;
                    m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0f);
                    m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                    jumpsAvailable--;
                }
            }
        }

    }

    private void DisableGDash()
    {
        animator.SetBool("GDash", false);
    }

    private void DisableAirDash()
    {
        animator.SetBool("AirDash", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Spikes")))
        {
            Player.Damage(spikeDamage);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentColiisions.Add(collision.collider);
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Walls")))
        {
            isWallToMyLeft = collision.contacts[0].point.x < transform.position.x;
        }
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("ground")))
        {
            m_Grounded = true;
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0f);
            hasDashAvailable = true;
            jumpsAvailable = 2;
        }
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            if (Player.Damage(collision.gameObject.GetComponent<Enemy>().damage))
            {
                if(collision.transform.position.x < transform.position.x)
                    m_Rigidbody2D.AddForce(new Vector2(0.5f * m_JumpForce, 0.2f * m_JumpForce));
                else
                    m_Rigidbody2D.AddForce(new Vector2(-0.5f * m_JumpForce, 0.2f * m_JumpForce));
            }
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
        if (!canShield || shieldOnCooldown)
        {
            if(!canShield)
                showSwastika();
            return;
        }
        if (shield.gameObject.activeSelf == false)
        {
            Debug.Log("shield");
            shield.gameObject.SetActive(true);
            shieldOnCooldown = true;
            Player.Shield(1f);
            //gameObject.GetComponent<Player>().Shield(/*shield duration: */ 1f);
            Invoke("disableShield", 1f);
            Invoke("shieldOffCooldown", 3f);
        }
    }
    private void shieldOffCooldown()
    {
        shieldOnCooldown = false;
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
        if (meleeWeapon.gameObject.activeSelf == false && rangedWeapon.gameObject.activeSelf == false)
        {
            Debug.Log("melee");
            animator.SetBool("Attack", true);
            meleeWeapon.gameObject.SetActive(true);
            Invoke("disableMelee", 0.5f);
        }
    }
    public void RangedAttack()
    {
        if (!canRanged)
        {
            showSwastika();
            return;
        }
        if (rangedWeapon.gameObject.activeSelf == false && meleeWeapon.gameObject.activeSelf == false)
        {
            Debug.Log("ranged");
            animator.SetBool("Ranged", true);
            rangedWeapon.gameObject.SetActive(true);
            rangedWeapon.gameObject.GetComponent<RangedWeapon>().Shoot(lookingRight);
            Invoke("disableRanged", 0.5f);
        }
    }
    private void disableRanged()
    {
        rangedWeapon.gameObject.SetActive(false);
        animator.SetBool("Ranged", false);


    }
    private void disableMelee()
    {
        meleeWeapon.gameObject.SetActive(false);
        animator.SetBool("Attack", false);

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
