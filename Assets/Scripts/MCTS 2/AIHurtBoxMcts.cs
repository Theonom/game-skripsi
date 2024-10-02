using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHurtBoxMcts : MonoBehaviour
{
    public Collider2D coll;
    public AIMcts aiMcts;
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
        if (AIMcts.aiBlockAttack == false)
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
            anim.SetTrigger("HitReact");
            AIMcts.aiSkillsPoint += 5;
            Player.playerSkillPoint += 10;

            if (AIMcts.canWalkLeft == true && AIMcts.canWalkRight == true)
            {
                if (aiTransform.position.x > playerTransform.position.x)
                {
                    rig.velocity = new Vector2(aiMcts.lightAttackForce, rig.velocity.y);
                }
                else
                {
                    rig.velocity = new Vector2(-aiMcts.lightAttackForce, rig.velocity.y);
                }
            }
        }
        if (collision.gameObject.CompareTag("HeavyAttacks"))
        {
            anim.SetTrigger("BigHitReact");
            AIMcts.aiBlockAttack = true;
            AIMcts.aiSkillsPoint += 5;
            Player.playerSkillPoint += 10;

            if (AIMcts.canWalkLeft == true && AIMcts.canWalkRight == true)
            {
                if (aiTransform.position.x > playerTransform.position.x)
                {
                    rig.velocity = new Vector2(aiMcts.heavyAttackForce, rig.velocity.y);
                }
                else
                {
                    rig.velocity = new Vector2(-aiMcts.heavyAttackForce, rig.velocity.y);
                }
            }
        }
    }
}
