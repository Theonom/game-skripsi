using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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
    public float timeBlocking;

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
        timeBlocking = 0;
        StatsReset();

        isJumping = false;
        playerAttack = false;
        playerBlockAttack = false;
        playerDown = false;
    }

    public void StatsReset()
    {
        playerAttack = false;
        playerBlockAttack = false;
        playerDown = false;

        canWalkLeft = true;
        canWalkRight = true;
        walkLeftPlayer = true;
        walkRightPlayer = true;
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
        EndBlock();

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
        if (GameController.playGame == true)
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
    }

    public void JumpingAndCrouching()
    {
        if (GameController.playGame == true)
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
                        AudioManager.instance.PlaySfx("jump");
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
        if (GameController.playGame == true)
        {
            //Standing Attacks
            if (player1Layer0.IsTag("Motion"))
            {
                if (playerSkillPoint >= lightSkillsPoint)
                {
                    if (Input.GetButtonDown("Light Punch"))
                    {
                        AudioManager.instance.PlaySfx("standA");
                        anim.SetTrigger("LightPunch");
                        playerAttack = true;
                        playerSkillPoint -= lightSkillsPoint;
                    }
                    if (Input.GetButtonDown("Light Kick"))
                    {
                        AudioManager.instance.PlaySfx("standB");
                        anim.SetTrigger("LightKick");
                        playerAttack = true;
                        playerSkillPoint -= lightSkillsPoint;
                    }
                }
                if (playerSkillPoint >= heavySkilsPoint)
                {
                    if (Input.GetButtonDown("Heavy Punch"))
                    {
                        AudioManager.instance.PlaySfx("standFA");
                        anim.SetTrigger("HeavyPunch");
                        playerAttack = true;
                        playerSkillPoint -= heavySkilsPoint;
                    }
                    if (Input.GetButtonDown("Heavy Kick"))
                    {
                        AudioManager.instance.PlaySfx("standFB");
                        anim.SetTrigger("HeavyKick");
                        playerAttack = true;
                        playerSkillPoint -= heavySkilsPoint;
                    }
                }
            }

            //Crouching attacks
            if (player1Layer0.IsTag("Crouching"))
            {
                if (playerSkillPoint >= lightSkillsPoint)
                {
                    if (Input.GetButtonDown("Light Punch"))
                    {
                        AudioManager.instance.PlaySfx("crouchA");
                        anim.SetTrigger("LightPunch");
                        playerAttack = true;
                        playerSkillPoint -= lightSkillsPoint;
                    }
                    if (Input.GetButtonDown("Light Kick"))
                    {
                        AudioManager.instance.PlaySfx("crouchB");
                        anim.SetTrigger("LightKick");
                        playerAttack = true;
                        playerSkillPoint -= lightSkillsPoint;
                    }
                }
                if (playerSkillPoint >= heavySkilsPoint)
                {
                    if (Input.GetButtonDown("Heavy Punch"))
                    {
                        AudioManager.instance.PlaySfx("crouchFA");
                        anim.SetTrigger("HeavyPunch");
                        playerAttack = true;
                        playerSkillPoint -= heavySkilsPoint;
                    }
                    if (Input.GetButtonDown("Heavy Kick"))
                    {
                        AudioManager.instance.PlaySfx("crouchFB");
                        anim.SetTrigger("HeavyKick");
                        playerAttack = true;
                        playerSkillPoint -= heavySkilsPoint;
                    }
                }
            }

            //Aerial attacks
            if (player1Layer0.IsTag("Jumping"))
            {
                if (playerSkillPoint >= lightSkillsPoint)
                {
                    if (Input.GetButtonDown("Light Punch"))
                    {
                        AudioManager.instance.PlaySfx("airA");
                        anim.SetTrigger("LightPunch");
                        playerAttack = true;
                        playerSkillPoint -= lightSkillsPoint;
                    }
                    if (Input.GetButtonDown("Light Kick"))
                    {
                        AudioManager.instance.PlaySfx("airB");
                        anim.SetTrigger("LightKick");
                        playerAttack = true;
                        playerSkillPoint -= lightSkillsPoint;
                    }
                }
                if (playerSkillPoint >= heavySkilsPoint)
                {
                    if (Input.GetButtonDown("Heavy Punch"))
                    {
                        AudioManager.instance.PlaySfx("airFA");
                        anim.SetTrigger("HeavyPunch");
                        playerAttack = true;
                        playerSkillPoint -= heavySkilsPoint;
                    }
                    if (Input.GetButtonDown("Heavy Kick"))
                    {
                        AudioManager.instance.PlaySfx("airFB");
                        anim.SetTrigger("HeavyKick");
                        playerAttack = true;
                        playerSkillPoint -= heavySkilsPoint;
                    }
                }
            }
        }
    }

    public void Blocking()
    {
        if (GameController.playGame == true)
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
                        timeBlocking = 0;
                        //StartCoroutine(EndBlock());
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
    }

    public void Rise()
    {
        if (player1Layer0.IsTag("Down"))
        {
            if (Input.GetButtonDown("Rise"))
            {
                AudioManager.instance.PlaySfx("rise");
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

    private void EndBlock()
    {
        if (player1Layer0.IsTag("Block"))
        {
            if (playerBlockAttack == true)
            {
                timeBlocking += Time.deltaTime;

                if (timeBlocking >= 2)
                {
                    anim.SetTrigger("BlockOff");
                    playerBlockAttack = false;
                }
        } }
    }

    IEnumerator JumpPause()
    {
        yield return new WaitForSeconds(1.0f);
        isJumping = false;
    }
}
