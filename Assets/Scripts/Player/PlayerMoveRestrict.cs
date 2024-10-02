using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveRestrict : MonoBehaviour
{
    public GameObject player;
    public GameObject ai;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("AIRight"))
        {
            if (player.transform.position.x < ai.transform.position.x)
            {
                Player.walkRightPlayer = false;
            }
            if (player.transform.position.x > ai.transform.position.x)
            {
                Player.walkLeftPlayer = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("AIRight"))
        {
            if (player.transform.position.x < ai.transform.position.x)
            {
                Player.walkRightPlayer = true;
            }
            if (player.transform.position.x > ai.transform.position.x)
            {
                Player.walkLeftPlayer = true;
            }
        }
    }
}
