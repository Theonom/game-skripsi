using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    [Header("Stats Character")]
    public int lightSkillsPoint;
    public int heavySkilsPoint;
    public int blockSkillsPoint;
    private bool isJumping = false;
    private bool jumpAttack = false;
    private bool walkToPlayer = false;
    public static float aiHealthPoint;
    public static float aiSkillsPoint;
    public static bool aiAttack = false;
    public static bool aiBlockAttack = false;

    [Header("Stats Movement")]
    public float walkSpeed;
    public float jumpForce;
    public float lightAttackForce;
    public float heavyAttackForce;

    [Header("Stats Distance")]
    public float attackDistanceX;
    public float attackDistanceY;
    public float rangeAttackY;
    public int marginMovement;
    [HideInInspector] public float playerDistanceX;
    [HideInInspector] public float playerDistanceY;
    public static bool canWalkLeft = true;
    public static bool canWalkRight = true;
    public static bool facingLeftAI = true;
    public static bool facingRightAI = false;
    public static bool walkLeftAI = true;
    public static bool walkRightAI = true;

    [Header("Components")]
    public LayerMask ground;
    public GameObject restrict;
    public GameObject player;
    private Animator anim;
    private AnimatorStateInfo ai1Layer0;
    private Rigidbody2D rig;
    private Collider2D coll;

    [Header("Etc")]
    public float randomAttackNumberRate;
    public int state = 0;
    private float timerRandomAttackNumber;
    private float timerWalkToPlayer;
    private int attackNumber;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rig = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        state = 0;
        timerWalkToPlayer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ai1Layer0 = anim.GetCurrentAnimatorStateInfo(0);

        playerDistanceX = Mathf.Abs(player.transform.position.x - transform.position.x);
        playerDistanceY = Mathf.Abs(player.transform.position.y - transform.position.y);

        //Cannot exit screen
        Vector3 ScreenBounds = Camera.main.WorldToScreenPoint(this.transform.position);
        if (ScreenBounds.x > Screen.width - marginMovement)
        {
            canWalkRight = false;
        }
        if (ScreenBounds.x < marginMovement)
        {
            canWalkLeft = false;
        }
        else if (ScreenBounds.x > marginMovement && ScreenBounds.x < Screen.width - marginMovement)
        {
            canWalkRight = true;
            canWalkLeft = true;
        }

        //Resets the restrict
        if (restrict.activeInHierarchy == false)
        {
            walkLeftAI = true;
            walkRightAI = true;
        }

        if (ai1Layer0.IsTag("Motion"))
        {
            Walking();
            anim.SetBool("Landing", false);
        }

        JumpingAndCrouching();
        Landing();
        Attack();
        Blocking();
        Rise();
        FaceRightOrLeft();
        WalkToPlayer();

        if (GameController.playGame == true)
        {
            if (playerDistanceX > attackDistanceX && state == 0 && Player.playerDown == false)
            {
                walkToPlayer = true;
            }
        }

        if (playerDistanceX <= attackDistanceX)
        {
            walkToPlayer = false;
            timerWalkToPlayer = 0;

            anim.SetBool("Forward", false);
            anim.SetBool("Backward", false);

            if (Player.playerDown == false)
            {
                StandAttackParameters();
                JumpAttackParameters();
                CrouchAttackParameters();
                StandBlockParameters();
                CrouchBlockParameters();
                BackStepParameters();
            }
        }
        else
        {
            if (state != 1 && state != 7 && state != 0)
            {
                state = 1;
            }
        }

        if (canWalkRight == false || canWalkLeft == false)
        {
            timerWalkToPlayer = 0;
            anim.SetBool("Forward", false);
            anim.SetBool("Backward", false);

            if (state == 7)
            {
                walkToPlayer = true;
            }
        }

        //Random number attack
        if (state == 2 || state == 3 || state == 4)
        {
            timerRandomAttackNumber += Time.deltaTime;

            if (timerRandomAttackNumber > randomAttackNumberRate)
            {
                timerRandomAttackNumber = 0;
                attackNumber = Random.Range(1, 5);
            }
        }

        //State if player down
        if (Player.playerDown == true)
        {
            state = 0;
        }

        //Crouch off
        if (state != 4 && state != 6)
        {
            anim.SetBool("Crouch", false);
        }

        //Skill point > 100
        if (aiSkillsPoint > 100)
        {
            aiSkillsPoint = 100;
        }
    }

    //Walking left and right
    public void Walking()
    {
        if (GameController.playGame == true)
        {
            //Walking forward
            if (state == 1)
            {
                if (playerDistanceX > attackDistanceX)
                {
                    if (player.transform.position.x > transform.position.x)
                    {
                        if (canWalkRight == true)
                        {
                            if (walkRightAI == true)
                            {
                                transform.Translate(walkSpeed, 0, 0);
                                if (facingRightAI == true)
                                {
                                    anim.SetBool("Forward", true);
                                    anim.SetBool("Backward", false);
                                }
                            }
                        }
                    }
                    if (player.transform.position.x < transform.position.x)
                    {
                        if (canWalkLeft == true)
                        {
                            if (walkLeftAI == true)
                            {
                                transform.Translate(-walkSpeed, 0, 0);
                                if (facingLeftAI == true)
                                {
                                    anim.SetBool("Forward", true);
                                    anim.SetBool("Backward", false);
                                }
                            }
                        }
                    }
                }
            }

            //Walking backstep
            if (state == 7)
            {
                if (canWalkRight == true)
                {
                    if (facingLeftAI == true)
                    {
                        transform.Translate(walkSpeed, 0, 0);
                        anim.SetBool("Forward", false);
                        anim.SetBool("Backward", true);
                    }
                }
                if (canWalkLeft == true)
                {
                    if (facingRightAI == true)
                    {
                        transform.Translate(-walkSpeed, 0, 0);
                        anim.SetBool("Forward", false);
                        anim.SetBool("Backward", true);
                    }
                }
            }
        }
    }

    public void JumpingAndCrouching()
    {
        if (GameController.playGame == true)
        {
            //Activated state jump
            if (ai1Layer0.IsTag("Motion"))
            {
                if (state == 3 && coll.IsTouchingLayers(ground))
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
            //Activated state crouch
            if (state == 4 || state == 6)
            {
                anim.SetBool("Crouch", true);
            }
        }
    }

    public void Landing()
    {
        if (ai1Layer0.IsTag("Jumping"))
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("Landing", true);
                state = 0;
            }
        }
    }

    public void Attack()
    {
        if (GameController.playGame == true)
        {
            //Standig attacks
            if (ai1Layer0.IsTag("Motion"))
            {
                if (state == 2)
                {
                    if (aiSkillsPoint >= lightSkillsPoint)
                    {
                        if (attackNumber == 1)
                        {
                            anim.SetTrigger("LightKickA");
                            aiAttack = true;
                        }
                        if (attackNumber == 2)
                        {
                            anim.SetTrigger("LightKickB");
                            aiAttack = true;
                        }
                    }
                    if (aiSkillsPoint >= heavySkilsPoint)
                    {
                        if (attackNumber == 3)
                        {
                            anim.SetTrigger("HeavyKickA");
                            aiAttack = true;
                        }
                        if (attackNumber == 4)
                        {
                            anim.SetTrigger("HeavyKickB");
                            aiAttack = true;
                        }
                    }
                }
            }

            //Crouching attacks
            if (ai1Layer0.IsTag("Crouching"))
            {
                if (state == 4)
                {
                    if (aiSkillsPoint >= lightSkillsPoint)
                    {
                        if (attackNumber == 1)
                        {
                            anim.SetTrigger("LightKickA");
                            aiAttack = true;
                        }
                        if (attackNumber == 2)
                        {
                            anim.SetTrigger("LightKickB");
                            aiAttack = true;
                        }
                    }
                    if (aiSkillsPoint >= heavySkilsPoint)
                    {
                        if (attackNumber == 3)
                        {
                            anim.SetTrigger("HeavyKickA");
                            aiAttack = true;
                        }
                        if (attackNumber == 4)
                        {
                            anim.SetTrigger("HeavyKickB");
                            aiAttack = true;
                        }
                    }
                }
            }

            //Aerial attacks
            if (ai1Layer0.IsTag("Jumping"))
            {
                if (state == 3)
                {
                    if (jumpAttack == true)
                    {
                        if (aiSkillsPoint >= lightSkillsPoint)
                        {
                            if (attackNumber == 1)
                            {
                                anim.SetTrigger("LightKickA");
                                aiAttack = true;
                            }
                            if (attackNumber == 2)
                            {
                                anim.SetTrigger("LightKickB");
                                aiAttack = true;
                            }
                        }
                        if (aiSkillsPoint >= heavySkilsPoint)
                        {
                            if (attackNumber == 3)
                            {
                                anim.SetTrigger("HeavyKickA");
                                aiAttack = true;
                            }
                            if (attackNumber == 4)
                            {
                                anim.SetTrigger("HeavyKickB");
                                aiAttack = true;
                            }
                        }
                    }
                }
            }
        }
    }

    public void Blocking()
    {
        if (GameController.playGame == true)
        {
            //Standing block
            if (ai1Layer0.IsTag("Motion"))
            {
                if (aiSkillsPoint >= blockSkillsPoint)
                {
                    if (state == 5 && aiBlockAttack == false)
                    {
                        anim.SetTrigger("BlockOn");
                        aiBlockAttack = true;
                        StartCoroutine(EndBlock());
                    }
                }
            }

            // Crouching block
            if (ai1Layer0.IsTag("Crouching"))
            {
                if (aiSkillsPoint >= blockSkillsPoint)
                {
                    if (state == 6 && aiBlockAttack == false)
                    {
                        anim.SetTrigger("BlockOn");
                        aiBlockAttack = true;
                        StartCoroutine(EndBlock());
                    }
                }
            }
        }
    }

    public void Rise()
    {
        if (ai1Layer0.IsTag("Down"))
        {
            if (aiHealthPoint > 0)
            {
                StartCoroutine(Down());
            }
        }
    }

    public void FaceRightOrLeft()
    {
        if (player.transform.position.x > transform.position.x)
        {
            facingRightAI = true;
            facingLeftAI = false;
            transform.localScale = new Vector2(1.0f, 1.0f);
        }
        if (player.transform.position.x < transform.position.x)
        {
            facingRightAI = false;
            facingLeftAI = true;
            transform.localScale = new Vector2(-1.0f, 1.0f);
        }
    }

    public void StandAttackParameters()
    {
        if (Player.playerHealthPoint >= aiHealthPoint)
        {
            if (aiHealthPoint > 267 && aiHealthPoint <= 400)
            {
                if (aiSkillsPoint > 50)
                {
                    if (Player.playerAttack == false)
                    {
                        if (playerDistanceY <= attackDistanceY)
                        {
                            state = 2;
                        }
                    }
                }
                if (aiSkillsPoint <= 50)
                {
                    if (Player.playerAttack == false)
                    {
                        if (playerDistanceY <= attackDistanceY)
                        {
                            state = 2;
                        }
                    }
                }
            }
            if (aiHealthPoint > 134 && aiHealthPoint <= 267)
            {
                if (aiSkillsPoint > 50)
                {
                    if (Player.playerAttack == false)
                    {
                        if (playerDistanceY <= attackDistanceY)
                        {
                            state = 2;
                        }
                    }
                }
            }
        }
        if (Player.playerHealthPoint < aiHealthPoint)
        {
            if (aiHealthPoint > 267 && aiHealthPoint <= 400)
            {
                if (aiSkillsPoint > 50)
                {
                    if (Player.playerAttack == true)
                    {
                        if (playerDistanceY <= attackDistanceY)
                        {
                            state = 2;
                        }
                    }
                    if (Player.playerAttack == false)
                    {
                        if (playerDistanceY <= attackDistanceY)
                        {
                            state = 2;
                        }
                    }
                }
                if (aiSkillsPoint <= 50)
                {
                    if (Player.playerAttack == false)
                    {
                        if (playerDistanceY > attackDistanceY)
                        {
                            state = 2;
                        }
                    }
                }
            }
            if (aiHealthPoint > 134 && aiHealthPoint <= 267)
            {
                if (aiSkillsPoint > 50)
                {
                    if (Player.playerAttack == false)
                    {
                        if (playerDistanceY > attackDistanceY)
                        {
                            state = 2;
                        }
                    }
                }
            }
            if (aiHealthPoint > 1 && aiHealthPoint <= 134)
            {
                if (aiSkillsPoint > 50)
                {
                    if (Player.playerAttack == false)
                    {
                        if (playerDistanceY <= attackDistanceY)
                        {
                            state = 2;
                        }
                    }
                }
                if (aiSkillsPoint <= 50)
                {
                    if (Player.playerAttack == false)
                    {
                        if (playerDistanceY <= attackDistanceY)
                        {
                            state = 2;
                        }
                    }
                }
            }
        }
    }

    public void JumpAttackParameters()
    {
        if (Player.playerHealthPoint >= aiHealthPoint)
        {
            if (aiHealthPoint > 267 && aiHealthPoint <= 400)
            {
                if (aiSkillsPoint > 50)
                {
                    if (Player.playerAttack == false)
                    {
                        if (playerDistanceY > attackDistanceY)
                        {
                            state = 3;
                            if (transform.position.y > rangeAttackY)
                            {
                                jumpAttack = true;
                            }
                            else
                            {
                                jumpAttack = false;
                            }
                        }
                    }
                }
                if (aiSkillsPoint <= 50)
                {
                    if (Player.playerAttack == false)
                    {
                        if (playerDistanceY > attackDistanceY)
                        {
                            state = 3;
                            if (transform.position.y > rangeAttackY)
                            {
                                jumpAttack = true;
                            }
                            else
                            {
                                jumpAttack = false;
                            }
                        }
                    }
                }
            }
            if (aiHealthPoint > 134 && aiHealthPoint <= 267)
            {
                if (aiSkillsPoint > 50)
                {
                    if (Player.playerAttack == false)
                    {
                        if (playerDistanceY > attackDistanceY)
                        {
                            state = 3;
                            if (transform.position.y > rangeAttackY)
                            {
                                jumpAttack = true;
                            }
                            else
                            {
                                jumpAttack = false;
                            }
                        }
                    }
                }
                if (aiSkillsPoint <= 50)
                {
                    if (Player.playerAttack == false)
                    {
                        if (playerDistanceY > attackDistanceY)
                        {
                            state = 3;
                            if (transform.position.y > rangeAttackY)
                            {
                                jumpAttack = true;
                            }
                            else
                            {
                                jumpAttack = false;
                            }
                        }
                    }
                }
            }
            if (aiHealthPoint >= 1 && aiHealthPoint <= 134)
            {
                if (aiSkillsPoint > 50)
                {
                    if (Player.playerAttack == false)
                    {
                        if (playerDistanceY > attackDistanceY)
                        {
                            state = 3;
                            if (transform.position.y > rangeAttackY)
                            {
                                jumpAttack = true;
                            }
                            else
                            {
                                jumpAttack = false;
                            }
                        }
                    }
                }
                if (aiSkillsPoint <= 50)
                {
                    if (Player.playerAttack == false)
                    {
                        if (playerDistanceY > attackDistanceY)
                        {
                            state = 3;
                            if (transform.position.y > rangeAttackY)
                            {
                                jumpAttack = true;
                            }
                            else
                            {
                                jumpAttack = false;
                            }
                        }
                    }
                }
            }
        }
        if (Player.playerHealthPoint < aiHealthPoint)
        {
            if (aiHealthPoint > 267 && aiHealthPoint <= 400)
            {
                if (aiSkillsPoint > 50)
                {
                    if (Player.playerAttack == true)
                    {
                        if (playerDistanceY > attackDistanceY)
                        {
                            state = 3;
                            if (transform.position.y > rangeAttackY)
                            {
                                jumpAttack = true;
                            }
                            else
                            {
                                jumpAttack = false;
                            }
                        }
                    }
                }
                if (aiSkillsPoint > 50)
                {
                    if (Player.playerAttack == false)
                    {
                        if (playerDistanceY > attackDistanceY)
                        {
                            state = 3;
                            if (transform.position.y > rangeAttackY)
                            {
                                jumpAttack = true;
                            }
                            else
                            {
                                jumpAttack = false;
                            }
                        }
                    }
                }
            }
            if (aiHealthPoint > 134 && aiHealthPoint <= 267)
            {
                if (aiSkillsPoint > 50)
                {
                    if (Player.playerAttack == true)
                    {
                        if (playerDistanceY > attackDistanceY)
                        {
                            state = 3;
                            if (transform.position.y > rangeAttackY)
                            {
                                jumpAttack = true;
                            }
                            else
                            {
                                jumpAttack = false;
                            }
                        }
                    }
                }
                if (aiSkillsPoint <= 50)
                {
                    if (Player.playerAttack == false)
                    {
                        if (playerDistanceY > attackDistanceY)
                        {
                            state = 3;
                            if (transform.position.y > rangeAttackY)
                            {
                                jumpAttack = true;
                            }
                            else
                            {
                                jumpAttack = false;
                            }
                        }
                    }
                }
            }
            if (aiHealthPoint >= 1 && aiHealthPoint <= 134)
            {
                if (aiSkillsPoint > 50)
                {
                    if (Player.playerAttack == false)
                    {
                        if (playerDistanceY > attackDistanceY)
                        {
                            state = 3;
                            if (transform.position.y > rangeAttackY)
                            {
                                jumpAttack = true;
                            }
                            else
                            {
                                jumpAttack = false;
                            }
                        }
                    }
                }
                if (aiSkillsPoint <= 50)
                {
                    if (Player.playerAttack == false)
                    {
                        if (playerDistanceY > attackDistanceY)
                        {
                            state = 3;
                            if (transform.position.y > rangeAttackY)
                            {
                                jumpAttack = true;
                            }
                            else
                            {
                                jumpAttack = false;
                            }
                        }
                    }
                }
            }
        }
    }

    public void CrouchAttackParameters()
    {
        if (Player.playerHealthPoint >= aiHealthPoint)
        {
            if (aiHealthPoint > 134 && aiHealthPoint <= 267)
            {
                if (aiSkillsPoint <= 50)
                {
                    if (Player.playerAttack == false)
                    {
                        if (playerDistanceY <= attackDistanceY)
                        {
                            state = 4;
                        }
                    }
                }
            }
            if (aiHealthPoint >= 1 && aiHealthPoint <= 134)
            {
                if (aiSkillsPoint > 50)
                {
                    if (Player.playerAttack == false)
                    {
                        if (playerDistanceY <= attackDistanceY)
                        {
                            state = 4;
                        }
                    }
                }
                if (aiSkillsPoint <= 50)
                {
                    if (Player.playerAttack == false)
                    {
                        if (playerDistanceY <= attackDistanceY)
                        {
                            state = 4;
                        }
                    }
                }
            }
        }
        if (Player.playerHealthPoint < aiHealthPoint)
        {
            if (aiHealthPoint > 267 && aiHealthPoint <= 400)
            {
                if (aiSkillsPoint <= 50)
                {
                    if (Player.playerAttack == false)
                    {
                        if (playerDistanceY <= attackDistanceY)
                        {
                            state = 4;
                        }
                    }
                }
            }
            if (aiHealthPoint > 134 && aiHealthPoint <= 267)
            {
                if (aiSkillsPoint > 50)
                {
                    if (Player.playerAttack == false)
                    {
                        if (playerDistanceY <= attackDistanceY)
                        {
                            state = 4;
                        }
                    }
                    if (Player.playerAttack == true)
                    {
                        if (playerDistanceY <= attackDistanceY)
                        {
                            state = 4;
                        }
                    }
                }
                if (aiSkillsPoint <= 50)
                {
                    if (Player.playerAttack == false)
                    {
                        if (playerDistanceY <= attackDistanceY)
                        {
                            state = 4;
                        }
                    }
                }
            }
        }
    }

    public void StandBlockParameters()
    {
        if (Player.playerHealthPoint >= aiHealthPoint)
        {
            if (aiHealthPoint > 267 && aiHealthPoint <= 400)
            {
                if (aiSkillsPoint > 50)
                {
                    if (Player.playerAttack == true)
                    {
                        if (playerDistanceY > attackDistanceY)
                        {
                            state = 5;
                        }
                        if (playerDistanceY <= attackDistanceY)
                        {
                            state = 5;
                        }
                    }
                }
            }
            if (aiHealthPoint > 134 && aiHealthPoint <= 267)
            {
                if (aiSkillsPoint > 50)
                {
                    if (Player.playerAttack == true)
                    {
                        if (playerDistanceY > attackDistanceY)
                        {
                            state = 5;
                        }
                        if (playerDistanceY <= attackDistanceY)
                        {
                            state = 5;
                        }
                    }
                }
            }
            if (aiHealthPoint >= 1 && aiHealthPoint <= 134)
            {
                if (aiSkillsPoint > 50)
                {
                    if (Player.playerAttack == true)
                    {
                        if (playerDistanceY > attackDistanceY)
                        {
                            state = 5;
                        }
                    }
                }
                if (aiSkillsPoint <= 50)
                {
                    if (Player.playerAttack == true)
                    {
                        if (playerDistanceY > attackDistanceY)
                        {
                            state = 5;
                        }
                    }
                }
            }
        }
        if (Player.playerHealthPoint < aiHealthPoint)
        {
            if (aiHealthPoint > 267 && aiHealthPoint <= 400)
            {
                if (aiSkillsPoint <= 50)
                {
                    if (Player.playerAttack == true)
                    {
                        if (playerDistanceY > attackDistanceY)
                        {
                            state = 5;
                        }
                    }
                }
            }
            if (aiHealthPoint >= 1 && aiHealthPoint <= 134)
            {
                if (aiSkillsPoint > 50)
                {
                    if (Player.playerAttack == true)
                    {
                        if (playerDistanceY > attackDistanceY)
                        {
                            state = 5;
                        }
                    }
                }
                if (aiSkillsPoint <= 50)
                {
                    if (Player.playerAttack == true)
                    {
                        if (playerDistanceY > attackDistanceY)
                        {
                            state = 5;
                        }
                    }
                }
            }
        }
    }

    public void CrouchBlockParameters()
    {
        if (Player.playerHealthPoint >= aiHealthPoint)
        {
            if (aiHealthPoint > 267 && aiHealthPoint <= 400)
            {
                if (aiSkillsPoint <= 50)
                {
                    if (Player.playerAttack == true)
                    {
                        if (playerDistanceY > attackDistanceY)
                        {
                            state = 6;
                        }
                    }
                }
            }
            if (aiHealthPoint > 134 && aiHealthPoint <= 267)
            {
                if (aiSkillsPoint <= 50)
                {
                    if (Player.playerAttack == true)
                    {
                        if (playerDistanceY > attackDistanceY)
                        {
                            state = 6;
                        }
                    }
                }
            }
            if (aiHealthPoint >= 1 && aiHealthPoint <= 134)
            {
                if (aiSkillsPoint > 50)
                {
                    if (Player.playerAttack == true)
                    {
                        if (playerDistanceY <= attackDistanceY)
                        {
                            state = 6;
                        }
                    }
                }
            }
        }
        if (Player.playerHealthPoint < aiHealthPoint)
        {
            if (aiHealthPoint > 134 && aiHealthPoint <= 267)
            {
                if (aiSkillsPoint <= 50)
                {
                    if (Player.playerAttack == true)
                    {
                        if (playerDistanceY <= attackDistanceY)
                        {
                            state = 6;
                        }
                    }
                }
            }
            if (aiHealthPoint >= 1 && aiHealthPoint <= 134)
            {
                if (aiSkillsPoint > 50)
                {
                    if (Player.playerAttack == true)
                    {
                        if (playerDistanceY <= attackDistanceY)
                        {
                            state = 6;
                        }
                    }
                }
            }
        }
    }

    public void BackStepParameters()
    {
        if (Player.playerHealthPoint >= aiHealthPoint)
        {
            if (aiHealthPoint > 267 && aiHealthPoint <= 400)
            {
                if (aiSkillsPoint <= 50)
                {
                    if (Player.playerAttack == true)
                    {
                        if (playerDistanceY <= attackDistanceY)
                        {
                            state = 7;
                        }
                    }
                }
            }
            if (aiHealthPoint > 134 && aiHealthPoint <= 267)
            {
                if (aiSkillsPoint <= 50)
                {
                    if (Player.playerAttack == true)
                    {
                        if (playerDistanceY <= attackDistanceY)
                        {
                            state = 7;
                        }
                    }
                }
            }
            if (aiHealthPoint >= 1 && aiHealthPoint <= 134)
            {
                if (aiSkillsPoint <= 50)
                {
                    if (Player.playerAttack == true)
                    {
                        if (playerDistanceY <= attackDistanceY)
                        {
                            state = 7;
                        }
                    }
                }
            }
        }
        if (Player.playerHealthPoint < aiHealthPoint)
        {
            if (aiHealthPoint > 267 && aiHealthPoint <= 400)
            {
                if (aiSkillsPoint <= 50)
                {
                    if (Player.playerAttack == true)
                    {
                        if (playerDistanceY <= attackDistanceY)
                        {
                            state = 7;
                        }
                    }
                }
            }
            if (aiHealthPoint > 134 && aiHealthPoint <= 267)
            {
                if (aiSkillsPoint <= 50)
                {
                    if (Player.playerAttack == true)
                    {
                        if (playerDistanceY > attackDistanceY)
                        {
                            state = 7;
                        }
                    }
                }
            }
            if (aiHealthPoint >= 1 && aiHealthPoint <= 134)
            {
                if (aiSkillsPoint <= 50)
                {
                    if (Player.playerAttack == true)
                    {
                        if (playerDistanceY <= attackDistanceY)
                        {
                            state = 7;
                        }
                    }
                }
            }
        }
    }

    public void WalkToPlayer()
    {
        if (walkToPlayer == true)
        {
            timerWalkToPlayer += Time.deltaTime;

            if (timerWalkToPlayer >= 2)
            {
                timerWalkToPlayer = 0;
                state = 1;
            }
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
        aiBlockAttack = false;
        anim.SetTrigger("BlockOff");
        state = 0;
    }

    IEnumerator Down()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Down", false);
        aiBlockAttack = false;
    }
}
