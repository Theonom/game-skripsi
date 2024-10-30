using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAction : MonoBehaviour
{
    public AI ai;
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
        AI.aiSkillsPoint -= ai.lightSkillsPoint;
    }

    public void AiHeavySkillsPoint()
    {
        AI.aiSkillsPoint -= ai.heavySkilsPoint;
    }

    public void AiBlockSkillsPoint()
    {
        AI.aiSkillsPoint -= ai.blockSkillsPoint;
    }

    public void PlaySfx(string nameSfx)
    {
        AudioManager.instance.PlaySfx(nameSfx);
    }
}
