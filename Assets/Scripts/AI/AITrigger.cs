using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITrigger : MonoBehaviour
{
    public Collider2D coll;
    public float damageAmt;

    private void Update()
    {
        if (AI.aiAttack == true)
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
        if (collision.gameObject.CompareTag("PlayerHurtBox"))
        {
            AI.aiAttack = false;
            Player.playerHealthPoint -= damageAmt;
        }
    }
}
