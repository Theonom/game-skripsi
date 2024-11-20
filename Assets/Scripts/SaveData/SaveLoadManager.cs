using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public List<Data> dataList = new List<Data>();
    private string filePath;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.GetRound() == 1)
        {
            filePath = Application.persistentDataPath + "/round1.txt";
        }
        if (GameManager.Instance.GetRound() == 2)
        {
            filePath = Application.persistentDataPath + "/round2.txt";
        }
        if (GameManager.Instance.GetRound() == 3)
        {
            filePath = Application.persistentDataPath + "/round3.txt";
        }
    }

    public void SaveData()
    {
        List<string> lines = new List<string>();

        foreach (var data in dataList)
        {
            lines.Add(data.ToString());
        }

        File.WriteAllLines(filePath, lines);
    }
}
