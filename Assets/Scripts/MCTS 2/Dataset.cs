using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewData", menuName = "Data")]
public class Dataset : ScriptableObject
{
    public List<Condition> dataCondition;
    public Action action;

    public enum Action
    {
        StandAttack,
        JumpAttack,
        CrouchAttack,
        StandBlock,
        CrouchBlock,
        BackStep,
    }
}

[System.Serializable]
public class Condition
{
    public string hpComparisionData;
    public string hpNPCData;
    public string spNpcData;
    public bool playerAttackData;
    public string positionYData;
}
