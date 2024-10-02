using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIJumpScript : MonoBehaviour
{
    public GameObject ai;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerSpaceDetector"))
        {
            if (AI.facingRightAI == true)
            {
                ai.transform.Translate(-1.5f, 0, 0);
            }
            if (AI.facingLeftAI == true)
            {
                ai.transform.Translate(1.5f, 0, 0);
            }
        }
    }
}
