using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCTS 
{
    private static System.Random random = new System.Random();

    public static Node RunMCTS(Node root, int iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            Node selectedNode = Selection(root);
            Node expandedNode = Expansion(selectedNode);
            double result = Simulation(expandedNode);
            Backpropagation(expandedNode, result);
        }

        return root.GetBestChild();
    }

    private static Node Selection(Node node)
    {
        while (!node.state.IsTerminal() && node.IsFullyExpanded())
        {
            node = UCTSelectChild(node);
        }
        return node;
    }

    private static Node Expansion(Node node)
    {
        List<Action> availableActions = node.state.GetAvailableActions();
        foreach (var action in availableActions)
        {
            if (!node.children.Exists(n => n.action == action))
            {
                GameState newState = node.state.ApplyAction(action);
                Node newNode = new Node(newState, node, action);
                node.children.Add(newNode);
                return newNode;
            }
        }
        return node;
    }

    private static double Simulation(Node node)
    {
        GameState simState = node.state;
        while (!simState.IsTerminal())
        {
            List<Action> actions = simState.GetAvailableActions();
            Action randomAction = actions[random.Next(actions.Count)];
            simState = simState.ApplyAction(randomAction);
        }
        return simState.GetResult();
    }

    private static void Backpropagation(Node node, double result)
    {
        while (node != null)
        {
            node.visits++;
            node.wins += result;
            node = node.parent;
        }
    }

    private static Node UCTSelectChild(Node node)
    {
        Node bestChild = null;
        double bestValue = double.MinValue;

        foreach (var child in node.children)
        {
            double uctValue = (child.wins / (child.visits + 1)) + Mathf.Sqrt(2 * Mathf.Log(node.visits + 1) / (child.visits + 1));
            if (uctValue > bestValue)
            {
                bestValue = uctValue;
                bestChild = child;
            }
        }
        return bestChild;
    }
}
