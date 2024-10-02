using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public Collider2D coll;
    public float damageAmt;

    private void Update()
    {
       if (Player.playerAttack == true)
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
        if (collision.gameObject.CompareTag("AIHurtBox"))
        {
            Player.playerAttack = false;
            AI.aiHealthPoint -= damageAmt;
        }
    }
}
