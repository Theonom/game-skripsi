using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHurtBox : MonoBehaviour
{
    public Collider2D coll;
    public AI ai;
    public Rigidbody2D rig;
    public Transform playerTransform;
    public Transform aiTransform;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (AI.aiBlockAttack == false)
        {
            coll.enabled = true;
        }
        else
        {
            coll.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("LightAttacks"))
        {
            AudioManager.instance.PlaySfx("hitA");
            anim.SetTrigger("HitReact");
            AI.aiSkillsPoint += 5;
            Player.playerSkillPoint += 10;

            if (AI.canWalkLeft == true && AI.canWalkRight == true)
            {
                if (aiTransform.position.x > playerTransform.position.x)
                {
                    rig.velocity = new Vector2(ai.lightAttackForce, rig.velocity.y);
                }
                else
                {
                    rig.velocity = new Vector2(-ai.lightAttackForce, rig.velocity.y);
                }
            }
        }
        if (collision.gameObject.CompareTag("HeavyAttacks"))
        {
            AudioManager.instance.PlaySfx("hitB");
            anim.SetTrigger("BigHitReact");
            AI.aiBlockAttack = true;
            AI.aiSkillsPoint += 5;
            Player.playerSkillPoint += 10;

            if (AI.canWalkLeft == true && AI.canWalkRight == true)
            {
                if (aiTransform.position.x > playerTransform.position.x)
                {
                    rig.velocity = new Vector2(ai.heavyAttackForce, rig.velocity.y);
                }
                else
                {
                    rig.velocity = new Vector2(-ai.heavyAttackForce, rig.velocity.y);
                }
            }
        }
    }
}
