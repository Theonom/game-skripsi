using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAction : MonoBehaviour
{
    public AI ai;
    public DataAI dataAI;
    public SaveLoadManager saveLoadManager;

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

    public void NPCAttackFalse()
    {
        AI.aiAttack = false;
    }

    public void AddDataBackStepToDataTesting()
    {
        if (!AI.isBackStep)
        {
            AI.isBackStep = true;
            dataAI.iteration += 1;
            GameState initialState = new GameState(dataAI.hpComparision, dataAI.hpNPC, dataAI.spNPC, dataAI.playerAttack, dataAI.positionY);
            dataAI.nodeAI = new Node(initialState);

            MCTS.RunMCTS(dataAI.nodeAI, dataAI.iteration, initialState);
            Action actionSelected = MCTS.Simulation(dataAI.nodeAI);
            string action = "BackStep," + actionSelected.name;

            Data newData = new Data
            {
                actionData = action,
                positionYData = dataAI.positionY,
                hpComparisionData = dataAI.hpComparision,
                hpNPCData = dataAI.hpNPC,
                spNPCData = dataAI.spNPC,
                playerAttackData = dataAI.playerAttack
            };

            saveLoadManager.dataList.Add(newData);

            /*            if (GameManager.Instance.round == 1)
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
            */
        }
    }

    public void AddDataToDataTesting(string action)
    {
        dataAI.iteration += 1;
        GameState initialState = new GameState(dataAI.hpComparision, dataAI.hpNPC, dataAI.spNPC, dataAI.playerAttack, dataAI.positionY);
        dataAI.nodeAI = new Node(initialState);

        MCTS.RunMCTS(dataAI.nodeAI, dataAI.iteration, initialState);
        Action actionSelected = MCTS.Simulation(dataAI.nodeAI);
        action = action + "," + actionSelected.name;

        Data newData = new Data
        {
            actionData = action,
            positionYData = dataAI.positionY,
            hpComparisionData = dataAI.hpComparision,
            hpNPCData = dataAI.hpNPC,
            spNPCData = dataAI.spNPC,
            playerAttackData = dataAI.playerAttack
        };

        saveLoadManager.dataList.Add(newData);
        /*        if (GameManager.Instance.round == 1)
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
        */
    }
}
