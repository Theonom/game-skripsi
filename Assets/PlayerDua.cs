using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDua : MonoBehaviour
{
    [Header("Stats Character")]
    public int lightSkillsPoint;
    public int heavySkilsPoint;
    public int blockSkillsPoint;
    private bool isJumping = false;
    public static float playerHealthPoint;
    public static float playerSkillPoint;
    public static bool playerAttack = false;
    public static bool playerBlockAttack = false;
    public static bool playerDown = false;

    [Header("Stats Movement")]
    public float walkSpeed;
    public float jumpWalkSpeed;
    public float jumpForce;
    public float lightAttackForce;
    public float heavyAttackForce;

    [Header("Stats Distance")]
    public int marginMovement;
    public static bool canWalkLeft = true;
    public static bool canWalkRight = true;
    public static bool facingLeft = false;
    public static bool facingRight = true;
    public static bool walkLeftPlayer = true;
    public static bool walkRightPlayer = true;

    [Header("Components")]
    public LayerMask ground;
    public GameObject restrict;
    public GameObject opponent;
    private Animator anim;
    private AnimatorStateInfo player1Layer0;
    private Rigidbody2D rig;
    private Collider2D coll;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rig = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    void Update()
    {
        player1Layer0 = anim.GetCurrentAnimatorStateInfo(0);

        if (player1Layer0.IsTag("Motion"))
        {
            Walking();
            anim.SetBool("Landing", false);
        }

        //Cannot exit screen
        Vector3 ScreenBounds = Camera.main.WorldToScreenPoint(this.transform.position);

        if (ScreenBounds.x > Screen.width - marginMovement)
        {
            canWalkRight = false;
        }
        if (ScreenBounds.x <  marginMovement)
        {
            canWalkLeft = false;
        }
        else if (ScreenBounds.x > marginMovement && ScreenBounds.x < Screen.width - marginMovement)
        {
            canWalkRight = true;
            canWalkLeft = true;
        }

        JumpingAndCrouching();
        Landing();
        Attack();
        Blocking();
        Rise();
        FaceRightOrLeft();

        //Resets the restrict
        if (restrict.activeInHierarchy == false)
        {
            walkLeftPlayer = true;
            walkRightPlayer = true;
        }

        if (canWalkRight == false || canWalkLeft == false)
        {
            anim.SetBool("Forward", false);
            anim.SetBool("Backward", false);
        }

        //Skill point > 100
        if (playerSkillPoint > 100)
        {
            playerSkillPoint = 100;
        }
    }

    //Walking left and right
    public void Walking()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            if (canWalkRight == true)
            {
                if (walkRightPlayer == true)
                {
                    transform.Translate(walkSpeed, 0, 0);
                    if (facingRight == true)
                    {
                        anim.SetBool("Forward", true);
                        anim.SetBool("Backward", false);
                    }
                    if (facingLeft == true)
                    {
                        anim.SetBool("Forward", false);
                        anim.SetBool("Backward", true);
                    }
                }
            }
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            if (canWalkLeft == true)
            {
                if (walkLeftPlayer == true)
                {
                    transform.Translate(-walkSpeed, 0, 0);
                    if (facingRight == true)
                    {
                        anim.SetBool("Forward", false);
                        anim.SetBool("Backward", true);
                    }
                    if (facingLeft == true)
                    {
                        anim.SetBool("Forward", true);
                        anim.SetBool("Backward", false);
                    }
                }
            }
        }
        if (Input.GetAxis("Horizontal") == 0)
        {
            anim.SetBool("Forward", false);
            anim.SetBool("Backward", false);
        }
    }

    public void JumpingAndCrouching()
    {
        if (player1Layer0.IsTag("Motion"))
        {
            if (Input.GetAxis("Vertical") > 0 && coll.IsTouchingLayers(ground))
            {
                anim.SetBool("Forward", false);
                anim.SetBool("Backward", false);

                if (isJumping == false)
                {
                    isJumping = true;
                    anim.SetTrigger("Jump");
                    rig.velocity = new Vector2(rig.velocity.x, jumpForce);
                    StartCoroutine(JumpPause());
                }
            }
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            anim.SetBool("Crouch", true);
        }
        if (Input.GetAxis("Vertical") == 0)
        {
            anim.SetBool("Crouch", false);
        }
    }

    public void Landing()
    {
        if (player1Layer0.IsTag("Jumping"))
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                if (canWalkRight == true)
                {
                    if (walkRightPlayer == true)
                    {
                        transform.Translate(jumpWalkSpeed, 0, 0);
                    }
                }
            }
            if (Input.GetAxis("Horizontal") < 0)
            {
                if (canWalkLeft == true)
                {
                    if (walkLeftPlayer == true)
                    {
                        transform.Translate(-jumpWalkSpeed, 0, 0);
                    }
                }
            }

            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("Landing", true);
            }
        }
    }

    public void Attack()
    {
        //Standing Attacks
        if (player1Layer0.IsTag("Motion"))
        {
            if (Input.GetButtonDown("Light Punch"))
            {
                anim.SetTrigger("LightKickA");
                playerAttack = true;
                playerSkillPoint -= lightSkillsPoint;
            }
            if (Input.GetButtonDown("Light Kick"))
            {
                anim.SetTrigger("LightKickB");
                playerAttack = true;
                playerSkillPoint -= lightSkillsPoint;
            }
            if (Input.GetButtonDown("Heavy Punch"))
            {
                anim.SetTrigger("HeavyKickA");
                playerAttack = true;
                playerSkillPoint -= heavySkilsPoint;
            }
            if (Input.GetButtonDown("Heavy Kick"))
            {
                anim.SetTrigger("HeavyKickB");
                playerAttack = true;
                playerSkillPoint -= heavySkilsPoint;
            }
        }

        //Crouching attacks
        if (player1Layer0.IsTag("Crouching"))
        {
            if (Input.GetButtonDown("Light Punch"))
            {
                anim.SetTrigger("LightKickA");
                playerAttack = true;
                playerSkillPoint -= lightSkillsPoint;
            }
            if (Input.GetButtonDown("Light Kick"))
            {
                anim.SetTrigger("LightKickB");
                playerAttack = true;
                playerSkillPoint -= lightSkillsPoint;
            }
            if (Input.GetButtonDown("Heavy Punch"))
            {
                anim.SetTrigger("HeavyKickA");
                playerAttack = true;
                playerSkillPoint -= heavySkilsPoint;
            }
            if (Input.GetButtonDown("Heavy Kick"))
            {
                anim.SetTrigger("HeavyKickB");
                playerAttack = true;
                playerSkillPoint -= heavySkilsPoint;
            }
        }

        //Aerial attacks
        if (player1Layer0.IsTag("Jumping"))
        {
            if (Input.GetButtonDown("Light Punch"))
            {
                anim.SetTrigger("LightKickA");
                playerAttack = true;
                playerSkillPoint -= lightSkillsPoint;
            }
            if (Input.GetButtonDown("Light Kick"))
            {
                anim.SetTrigger("LightKickB");
                playerAttack = true;
                playerSkillPoint -= lightSkillsPoint;
            }
            if (Input.GetButtonDown("Heavy Punch"))
            {
                anim.SetTrigger("HeavyKickA");
                playerAttack = true;
                playerSkillPoint -= heavySkilsPoint;
            }
            if (Input.GetButtonDown("Heavy Kick"))
            {
                anim.SetTrigger("HeavyKickB");
                playerAttack = true;
                playerSkillPoint -= heavySkilsPoint;
            }
        }
    }

    public void Blocking()
    {
        if (player1Layer0.IsTag("Motion") || player1Layer0.IsTag("Crouching"))
        {
            if (playerSkillPoint >= blockSkillsPoint)
            {
                if (Input.GetButtonDown("Block"))
                {
                    anim.SetTrigger("BlockOn");
                    playerSkillPoint -= blockSkillsPoint;
                    playerBlockAttack = true;
                    StartCoroutine(EndBlock());
                }
            }
        }
        if (player1Layer0.IsTag("Block"))
        {
            if (Input.GetButtonUp("Block"))
            {
                anim.SetTrigger("BlockOff");
                playerBlockAttack = false;
            }
        }
    }

    public void Rise()
    {
        if (player1Layer0.IsTag("Down"))
        {
            if (Input.GetButtonDown("Rise"))
            {
                anim.SetBool("Down", false);
                playerBlockAttack = false;
                playerDown = false;
            }
        }
    }

    public void FaceRightOrLeft()
    {
        if (opponent.transform.position.x > transform.position.x)
        {
            facingRight = true;
            facingLeft = false;
            transform.localScale = new Vector2(1.0f, 1.0f);
        }
        if (opponent.transform.position.x < transform.position.x)
        {
            facingRight = false;
            facingLeft = true;
            transform.localScale = new Vector2(-1.0f, 1.0f);
        }
    }

    IEnumerator JumpPause()
    {
        yield return new WaitForSeconds(1.0f);
        isJumping = false;
    }

    IEnumerator EndBlock()
    {
        yield return new WaitForSeconds(2.0f);
        playerBlockAttack = false;
        anim.SetTrigger("BlockOff");
    }
}
