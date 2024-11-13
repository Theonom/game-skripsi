using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataAI : MonoBehaviour
{
    public AI ai;

    [Header("Parameters")]
    public string hpComparision;
    public string hpNPC;
    public string spNPC;
    public string playerAttack;
    public string positionY;

    [Header("Action")]
    public string action;

    [Header("Data Testing")]
    public List<DataTesting> dataRound;

    [HideInInspector] public Node nodeAI;
    public int iteration;

    // Update is called once per frame
    void Update()
    {
        //Convert parameters value to string
        HpComparisionInString();
        HpNpcInString();
        SpNpcInString();
        PlayerAttackInString();
        PositionYInString();

        //Convert action to string
        ActionInString();

        if (Input.GetKeyDown(KeyCode.B))
        {
            iteration += 1;
            GameState initialState = new GameState(hpComparision, hpNPC, spNPC, playerAttack, positionY);
            nodeAI = new Node(initialState);

            Node bestMoveNode = MCTS.RunMCTS(nodeAI, iteration, initialState);
            Action actionSelected = MCTS.Simulation(nodeAI);
            Debug.Log("Action: " + actionSelected.name);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Action actionSelected = MCTS.Simulation(nodeAI);
            Debug.Log("Action: " + actionSelected.name);
       }
    }

    public void HpComparisionInString()
    {
        if (Player.playerHealthPoint >= AI.aiHealthPoint)
        {
            hpComparision = "Lebih";
        }
        if (Player.playerHealthPoint < AI.aiHealthPoint)
        {
            hpComparision = "Kurang";
        }
    }

    public void HpNpcInString()
    {
        if (AI.aiHealthPoint > 267 && AI.aiHealthPoint <= 400)
        {
            hpNPC = "Besar";
        }
        if (AI.aiHealthPoint > 134 && AI.aiHealthPoint <= 267)
        {
            hpNPC = "Sedang";
        }
        if (AI.aiHealthPoint > 1 && AI.aiHealthPoint <= 134)
        {
            hpNPC = "Kecil";
        }
    }

    public void SpNpcInString()
    {
        if (AI.aiHealthPoint > 50)
        {
            spNPC = "Besar";
        }
        else
        {
            spNPC = "Kecil";
        }
    }

    public void PlayerAttackInString()
    {
        playerAttack = Player.playerAttack.ToString();
    }

    public void PositionYInString()
    {
        if (ai.playerDistanceX > ai.attackDistanceX)
        {
            positionY = "Tinggi";
        }
        if (ai.playerDistanceX > ai.attackDistanceX)
        {
            positionY = "Sedang";
        }
    }

    public void ActionInString()
    {
        if (ai.state == 0)
        {
            action = "Idle";
        }
        if (ai.state == 1)
        {
            action = "Walk";
        }
        if (ai.state == 2)
        {
            action = "Stand Attack";
        }
        if (ai.state == 3)
        {
            action = "Jump Attack";
        }
        if (ai.state == 4)
        {
            action = "Crouch Attack";
        }
        if (ai.state == 5)
        {
            action = "Stand Block";
        }
        if (ai.state == 6)
        {
            action = "Crouch Block";
        }
        if (ai.state == 7)
        {
            action = "Back Step";
        }
    }
}
