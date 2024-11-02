using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAction : MonoBehaviour
{
    public AI ai;
    public DataAI dataAI;

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

    public void AddDataToDataTesting(string action)
    {
        Data newData = new Data
        {
            actionData = action,
            hpComparisionData = dataAI.hpComparision,
            hpNPCData = dataAI.hpNPC,
            spNPCData = dataAI.spNPC,
            playerAttackData = dataAI.playerAttack,
            positionYData = dataAI.positionY
        };

        if (GameManager.Instance.round == 1)
        {
            dataAI.dataRound[0].AddData(newData);
        }
        if (GameManager.Instance.round == 2)
        {
            dataAI.dataRound[1].AddData(newData);
        }
        if (GameManager.Instance.round == 3)
        {
            dataAI.dataRound[2].AddData(newData);
        }
    }
}
