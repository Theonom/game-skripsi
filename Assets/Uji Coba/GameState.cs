using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public int playerHealth;
    public int opponentHealth;
    public bool isPlayerTurn;

    public GameState(int playerHP, int OpponentHP, bool playerTurn)
    {
        playerHealth = playerHP;
        opponentHealth = OpponentHP;
        isPlayerTurn = playerTurn;
    }

    public List<Action> GetAvailableActions()
    {
        return new List<Action> { new Action("Attack"), new Action("Defend"), new Action("Dodge") };
    }

    public GameState ApplyAction(Action action)
    {
        GameState newState = new GameState(playerHealth, opponentHealth, !isPlayerTurn);

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

