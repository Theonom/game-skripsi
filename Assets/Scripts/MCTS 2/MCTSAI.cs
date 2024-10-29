using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCTSAI : MonoBehaviour
{
    public AIMcts aiMCTS;

    [Header("Parameters")]
    public string hpComparision;
    public string hpNPC;
    public string spNPC;
    public bool playerAttack;
    public string positionY;

    [Header("Action")]
    public string action;

    [Header("Dataset")]
    public List<Dataset> listDataset;

    // Update is called once per frame
    void Update()
    {
        ParameterInString();
        Action();
    }

    public void ParameterInString()
    {
        //HP comparision
        if (Player.playerHealthPoint >= AIMcts.aiHealthPoint)
        {
            hpComparision = "Lebih";
        }
        if (Player.playerHealthPoint < AIMcts.aiHealthPoint)
        {
            hpComparision = "Kurang";
        }

        //HP NPC
        if (AIMcts.aiHealthPoint > 267 && AIMcts.aiHealthPoint <= 400)
        {
            hpNPC = "Besar";
        }
        if (AIMcts.aiHealthPoint > 134 && AIMcts.aiHealthPoint <= 267)
        {
            hpNPC = "Sedang";
        }
        if (AIMcts.aiHealthPoint > 1 && AIMcts.aiHealthPoint <= 134)
        {
            hpNPC = "Kecil";
        }

        //SP NPC
        if (AIMcts.aiHealthPoint > 50)
        {
            spNPC = "Besar";
        }
        else
        {
            spNPC = "Kecil";
        }

        //Player Attack
        playerAttack = Player.playerAttack;

        //PositionY
        if (aiMCTS.playerDistanceY > aiMCTS.attackDistanceY)
        {
            positionY = "Tinggi";
        }
        if (aiMCTS.playerDistanceY <= aiMCTS.attackDistanceY)
        {
            positionY = "Sedang";
        }
    }

    public void Action()
    {
        for (int i = 0; i < listDataset.Count; i++)
        {
            for (int j = 0; j < listDataset[i].dataCondition.Count; j++)
            {
                if ((hpComparision == listDataset[i].dataCondition[j].hpComparisionData) &&
                    (hpNPC == listDataset[i].dataCondition[j].hpNPCData) &&
                    (spNPC == listDataset[i].dataCondition[j].spNpcData) &&
                    (playerAttack == listDataset[i].dataCondition[j].playerAttackData) &&
                    (positionY == listDataset[i].dataCondition[j].positionYData))
                {
                    action = listDataset[i].action.ToString();
                }
            }
        }
    }
}
