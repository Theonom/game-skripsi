using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDataset : MonoBehaviour
{
    public List<Action> listDataAction;

    public List<Action> listAction;

    private void Start()
    {
        listDataAction = GetAvailableActions();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.B))
        {
            DeleteWithName();
            //SearchLastDataInList();
           // ExecuteActions();
           // Debug.Log(UCT());
        }*/

        if (listDataAction.Count <= 0)
        {
            listDataAction = GetAvailableActions();
        }
    }

    public List<Action> GetAvailableActions()
    {
        return new List<Action> { new Action("1"), new Action("2"), new Action("3"), new Action("4"), new Action("5"), new Action("6") };
    }

    public void DeleteWithName()
    {
        for (int i = 0; i < listAction.Count; i++)
        {
            string targetName = listAction[i].name;
            listDataAction.RemoveAll(Action => Action.name == targetName);
            Debug.Log(targetName);
        }
    }

    public void SearchLastDataInList()
    {
        for (int i = 0; i < listAction.Count; i++)
        {
            if (i == (listAction.Count - 1))
            {
                Debug.Log(listAction[i].name);
            }
        }
    }

    public void ExecuteActions()
    {
        if (listDataAction.Count > 0)
        {
            var random = new System.Random();

            int index = random.Next(listDataAction.Count);
            Action action = listDataAction[index];
            //listAction.Add(action);
            listDataAction.RemoveAt(index);
        }
    }

/*    private string UCT()
    {
        Action bestAction = null;
        double bestValue = double.MinValue;

        foreach (var action in listAction)
        {
            double uctValue = (action.wins / (action.visit + 1)) + Mathf.Sqrt(2 * Mathf.Log(action.visit + 1) / (action.visit + 1));
            if (uctValue > bestValue)
            {
                bestValue = uctValue;
                bestAction = action;
            }
        }
        bestAction.visit += 1;
        return bestAction.name;
    }
*/
    private static Action UCTSelectChild(Action action)
    {
        Action bestAction = null;
        
        return bestAction;
    }
}