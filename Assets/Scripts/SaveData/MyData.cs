using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MyData
{
    public string positionYData;
    public string hpComparisionData;
    public string hpNPCData;
    public string spNPCData;
    public string playerAttackData;
    public string actionData;

    public override string ToString()
    {
        return positionYData + "," + hpComparisionData + "," + hpNPCData + "," + spNPCData + "," + playerAttackData + "," + actionData;
    }
}
