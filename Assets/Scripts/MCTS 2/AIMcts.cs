using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMcts : MonoBehaviour
{
    [Header("Stats Character")]
    public int lightSkillsPoint;
    public int heavySkilsPoint;
    public int blockSkillsPoint;
    private bool isJumping = false;
    private bool jumpAttack = false;
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
    public int marginMovement;
    private float playerDistanceX;
    private float playerDistanceY;
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

    [Header("Dataset")]
    public List<Dataset> listDataset;

    [Header("String Parameters")]
    public string hpComparisioValue;
    public string hpNpcValue;
    public string spNpcValue;
    public bool playerAttackValue;
    public string positionYValue;
    public string actionValue;

    [Header("Etc")]
    public int state = 0;
    private int attackNumber;
    private float timerWalkToPlayer;

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

        ConvertParameterToString();
        MCTS();
        DefinitionActionValue();

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

    public void MCTS()
    {
        if (GameController.playGame)
        {
            if (playerDistanceX > attackDistanceX && state == 0 && !Player.playerDown)
            {
                if (timerWalkToPlayer < 2)
                {
                    timerWalkToPlayer += Time.deltaTime;
                }
                if (timerWalkToPlayer >= 2)
                {
                    state = 1;
                    timerWalkToPlayer = 0;
                }
            }
            if (playerDistanceX <= attackDistanceX)
            {
                state = 0;
                anim.SetBool("Forward", false);
                anim.SetBool("Backward", false);

                //Selection
                //Expansion
                //Simulation
                //Bakcpropagation
            }
        }
    }

    public void ConvertParameterToString()
    {
        //HP comparision
        if (Player.playerHealthPoint >= aiHealthPoint)
        {
            hpComparisioValue = "Lebih";
        }
        if (Player.playerHealthPoint < aiHealthPoint)
        {
            hpComparisioValue = "Kurang";
        }

        //HPNPC
        if (aiHealthPoint > 267 && aiHealthPoint <= 400)
        {
            hpNpcValue = "Besar";
        }
        if (aiHealthPoint > 134 && aiHealthPoint <= 267)
        {
            hpNpcValue = "Sedang";
        }
        if (aiHealthPoint > 1 && aiHealthPoint <= 134)
        {
            hpNpcValue = "Kecil";
        }

        //SPNPC
        if (aiSkillsPoint > 50)
        {
            spNpcValue = "Besar";
        }
        else
        {
            spNpcValue = "Kecil";
        }

        //PlayerAttack
        playerAttackValue = Player.playerAttack;

        //PositionY
        if (playerDistanceY > attackDistanceY)
        {
            positionYValue = "Tinggi";
        }
        if (playerDistanceY <= attackDistanceY)
        {
            positionYValue = "Sedang";
        }
    }

    public void DefinitionActionValue()
    {
        if (actionValue == "StandAttack")
        {
            state = 2;
        }
        if (actionValue == "JumpAttack")
        {
            state = 3;
        }
        if (actionValue == "CrouchAttack")
        {
            state = 4;
        }
        if (actionValue == "StandBlock")
        {
            state = 5;
        }
        if (actionValue == "CrouchBlock")
        {
            state = 6;
        }
        if (actionValue == "BackStep")
        {
            state = 7;
        }
    }
}
