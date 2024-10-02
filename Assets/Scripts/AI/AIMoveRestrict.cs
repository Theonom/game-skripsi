using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveRestrict : MonoBehaviour
{
    public GameObject player;
    public GameObject ai;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerRight"))
        {
            if (ai.transform.position.x < player.transform.position.x)
            {
                AI.walkRightAI = false;
            }
            if (ai.transform.position.x > player.transform.position.x)
            {
                AI.walkLeftAI = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerRight"))
        {
            if (ai.transform.position.x < player.transform.position.x)
            {
                AI.walkRightAI = true;
            }
            if (ai.transform.position.x > player.transform.position.x)
            {
                AI.walkLeftAI = true;
            }
        }
    }
}
