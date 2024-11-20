using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public DataTesting dataReal1;
    public DataTesting dataCopy1;

    public DataTesting dataReal2;
    public DataTesting dataCopy2;

    public DataTesting dataReal3;
    public DataTesting dataCopy3;

    // Start is called before the first frame update
    void Start()
    {
        if (dataReal1 != null && dataCopy1 != null)
        {
            for (int i = 0; i < dataReal1.dataTesting.Count; i++)
            {
                dataCopy1.AddData(dataReal1.dataTesting[i]);
            }
        }

        if (dataReal2 != null && dataCopy2 != null)
        {
            for (int i = 0; i < dataReal2.dataTesting.Count; i++)
            {
                dataCopy2.AddData(dataReal2.dataTesting[i]);
            }
        }

        if (dataCopy3 != null && dataCopy3 != null)
        {
            for (int i = 0; i < dataReal3.dataTesting.Count; i++)
            {
                dataCopy3.AddData(dataReal3.dataTesting[i]);
            }
        }
    }
}
