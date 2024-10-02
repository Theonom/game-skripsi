using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIActionMcts : MonoBehaviour
{
    public AIMcts ai;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Down()
    {
        anim.SetBool("Down", true);
    }

    public void AiLightSkillsPoint()
    {
        AIMcts.aiSkillsPoint -= ai.lightSkillsPoint;
    }

    public void AiHeavySkillsPoint()
    {
        AIMcts.aiSkillsPoint -= ai.heavySkilsPoint;
    }

    public void AiBlockSkillsPoint()
    {
        AIMcts.aiSkillsPoint -= ai.blockSkillsPoint;
    }
}
