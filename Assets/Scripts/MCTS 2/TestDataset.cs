using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDataset : MonoBehaviour
{
    public Dataset dataset;

    public string hpComparision;
    public string hpNPC;
    public string spNPC;
    public bool playerAttack;
    public string positionY;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < dataset.dataCondition.Count; i++)
        {
            if ((hpComparision == dataset.dataCondition[i].hpComparisionData) &&
                (hpNPC == dataset.dataCondition[i].hpNPCData) &&
                (spNPC == dataset.dataCondition[i].spNpcData) &&
                (playerAttack == dataset.dataCondition[i].playerAttackData) &&
                (positionY == dataset.dataCondition[i].positionYData))
            {
                Debug.Log(dataset.action);
            }
        }
    }
}
