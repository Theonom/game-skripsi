using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveRestrictMcts : MonoBehaviour
{
    public GameObject player;
    public GameObject aiMcts;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerRight"))
        {
            if (aiMcts.transform.position.x < player.transform.position.x)
            {
                AIMcts.walkRightAI = false;
            }
            if (aiMcts.transform.position.x > player.transform.position.x)
            {
                AIMcts.walkLeftAI = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerRight"))
        {
            if (aiMcts.transform.position.x < player.transform.position.x)
            {
                AIMcts.walkRightAI = true;
            }
            if (aiMcts.transform.position.x > player.transform.position.x)
            {
                AIMcts.walkLeftAI = true;
            }
        }
    }
}
