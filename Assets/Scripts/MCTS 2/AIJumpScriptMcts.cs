using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIJumpScriptMcts : MonoBehaviour
{
    public GameObject aiMcts;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerSpaceDetector"))
        {
            if (AIMcts.facingRightAI == true)
            {
                aiMcts.transform.Translate(-1.5f, 0, 0);
            }
            if (AIMcts.facingLeftAI == true)
            {
                aiMcts.transform.Translate(1.5f, 0, 0);
            }
        }
    }
}
