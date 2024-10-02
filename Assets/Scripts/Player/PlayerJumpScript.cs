using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpScript : MonoBehaviour
{
    public GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("AiSpaceDetector"))
        {
            if (Player.facingRight == true)
            {
                player.transform.Translate(-1.5f, 0, 0);
            }
            if (Player.facingLeft == true)
            {
                player.transform.Translate(1.5f, 0, 0);
            }
        }
    }
}
