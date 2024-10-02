using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITriggerMcts : MonoBehaviour
{
    public Collider2D coll;
    public float damageAmt;

    private void Update()
    {
        if (AIMcts.aiAttack == true)
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
            AIMcts.aiAttack = false;
            Player.playerHealthPoint -= damageAmt;
        }
    }
}
