using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public int playerHealth = 100;
    public int opponentHealth = 100;

    private void Start()
    {
        GameState initialState = new GameState(playerHealth, opponentHealth, true);
        Node root = new Node(initialState);

        Node bestMoveNode = MCTS.RunMCTS(root, 1000);

        Debug.Log("Best move: " + bestMoveNode.action.name);
        ApplyAction(bestMoveNode.action);
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
