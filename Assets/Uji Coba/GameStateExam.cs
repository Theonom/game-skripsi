using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameStateExam
{
    public int playerHealth;
    public int opponentHealth;
    public bool isPlayerTurn;

    private static List<Action> availableActions;

    public GameStateExam(int playerHP, int OpponentHP, bool playerTurn)
    {
        playerHealth = playerHP;
        opponentHealth = OpponentHP;
        isPlayerTurn = playerTurn;
    }

    public List<Action> GetAvailableActions()
    {
        if (availableActions == null)
        {
            availableActions = new List<Action>
            {
                new Action("Attack"),
                new Action("Defend"),
                new Action("Dodge")
            };
        }
        return availableActions;
    }

    public GameStateExam ApplyAction(Action action)
    {
        GameStateExam newState = new GameStateExam(playerHealth, opponentHealth, !isPlayerTurn);

        if (action.name == "Attack")
        {
            if (isPlayerTurn) newState.opponentHealth -= 10;
            else newState.playerHealth -= 10;
        }
        else if (action.name == "Defend")
        {

        }
        else if (action.name == "Dodge")
        {

        }

        return newState;
    }

    public bool IsTerminal()
    {
        return playerHealth <= 0 || opponentHealth <= 0;
    }

    public double GetResult()
    {
        if (playerHealth <= 0) return 0.0;
        else if (opponentHealth <= 0) return 1.0;
        return 0.5;
    }
}

