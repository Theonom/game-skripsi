using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewData", menuName = "DataTesting")]
public class DataTesting : ScriptableObject
{
    public List<Data> dataTesting;

    public void AddData(Data newData)
    {
        dataTesting.Add(newData);
    }

    public void RemoveAllData()
    {
        dataTesting.Clear();
    }
}

[System.Serializable]
public class Data{
    public string actionData;
    public string hpComparisionData;
    public string hpNPCData;
    public string spNPCData;
    public string playerAttackData;
    public string positionYData;
}
