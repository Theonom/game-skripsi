using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDataset : MonoBehaviour
{
    public List<Dataset> listDataset;

    public string hpComparision;
    public string hpNPC;
    public string spNPC;
    public bool playerAttack;
    public string positionY;

    public string action;

    // Update is called once per frame
    void Update()
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
