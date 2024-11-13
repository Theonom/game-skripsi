using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public int playerHealth = 100;
    public int opponentHealth = 100;
    public int iteration;
    [HideInInspector] public NodeExam node;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            iteration += 1;

            GameStateExam initialState = new GameStateExam(playerHealth, opponentHealth, true);
            node = new NodeExam(initialState);

            MCTSExam.RunMCTS(node, iteration);
            Action actionSelected = MCTSExam.Simulation(node);
            Debug.Log("Action: " + actionSelected.name);
        }

        //uji coba mencari cabang
        if (Input.GetKeyDown(KeyCode.C))
        {
            Action actionSelected = MCTSExam.Simulation(node);
            Debug.Log("Action Selected: " + actionSelected.name);
            //selectNode.wins += 1;
        }
    }

    private void ApplyAction(Action action)
    {
        if (action.name == "Attack")
        {

        }
        else if (action.name == "Defend")
        {

        }
        else if (action.name == "Dodge")
        {

        }
    }
}
