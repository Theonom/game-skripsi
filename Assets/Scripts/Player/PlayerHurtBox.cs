using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtBox : MonoBehaviour
{
    public Collider2D coll;
    public Player player;
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
        if (Player.playerBlockAttack == false)
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
            Player.playerSkillPoint += 4;
            AI.aiSkillsPoint += 4;

            if (Player.canWalkLeft == true && Player.canWalkRight == true)
            {
                if (playerTransform.position.x > aiTransform.position.x)
                {
                    rig.velocity = new Vector2(player.lightAttackForce, rig.velocity.y);
                }
                else
                {
                    rig.velocity = new Vector2(-player.lightAttackForce, rig.velocity.y);
                }
            }
        }
        if (collision.gameObject.CompareTag("HeavyAttacks"))
        {
            AudioManager.instance.PlaySfx("hitB");
            anim.SetTrigger("BigHitReact");
            Player.playerBlockAttack = true;
            Player.playerSkillPoint += 4;
            AI.aiSkillsPoint += 4;

            if (Player.canWalkLeft == true && Player.canWalkRight == true)
            {
                if (playerTransform.position.x > aiTransform.position.x)
                {
                    rig.velocity = new Vector2(player.heavyAttackForce, rig.velocity.y);
                }
                else
                {
                    rig.velocity = new Vector2(-player.heavyAttackForce, rig.velocity.y);
                }
            }
        }
    }
}
