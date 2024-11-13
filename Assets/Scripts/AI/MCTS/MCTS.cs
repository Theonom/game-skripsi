using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCTS
{
    public static Node RunMCTS(Node root, int iterations, GameState state)
    {
        for (int i = 0; i < iterations; i++)
        {
            Node selectedNode = Selection(root);
            Node expandedNode = Expansion(selectedNode, state);
            Backpropagation(expandedNode);
        }
        return root.GetBestChild();
    }

    public static Action Simulation(Node node)
    {
        Node selectedNode = SelectNode(node);
        Action action = new Action("");

        while (selectedNode != null)
        {
            action = action.Combine(selectedNode.action);
            selectedNode = selectedNode.parent;
        }
        return action;
    }


    private static Node SelectNode(Node node)
    {
        if (node.IsFullyExpanded())
        {
            while (node.IsFullyExpanded())
            {
                Node bestChild = node.children[0];
                double bestVisit = node.children[0].visits;

                foreach (Node child in node.children)
                {
                    double visitChild = child.visits;
                    if (visitChild >= bestVisit)
                    {
                        bestChild = child;
                        bestVisit = visitChild;
                    }
                }

                if (bestChild.children.Count > 0 && bestChild.children.Count < 3)
                {
                    foreach (Node child in bestChild.children)
                    {
                        double visitChild = child.visits;
                        if (visitChild <= bestVisit)
                        {
                            bestChild = child;
                            bestVisit = visitChild;
                        }
                    }
                    node = bestChild;
                }
                else
                {
                    node = bestChild;
                }
            }
        }
        if (!node.IsFullyExpanded() && node.children.Count > 0)
        {
            Node bestChild = node.children[0];
            double bestVisit = node.children[0].visits;

            foreach (Node child in node.children)
            {
                double visitChild = child.visits;
                if (visitChild >= bestVisit)
                {
                    bestChild = child;
                    bestVisit = visitChild;
                }
            }
            node = bestChild;
        }
        return node;
    }

    private static Node Selection(Node node)
    {
        while (node.IsFullyExpanded())
        {
            node = UCTSelectChild(node);
        }
        return node;
    }

    private static Node Expansion(Node node, GameState state)
    {
        List<Action> availableActions = node.GetAvailableActions();

        foreach (var action in availableActions)
        {
            if (!node.children.Exists(n => n.action == action))
            {
                Node newNode = new Node(state, node, action);
                node.children.Add(newNode);
                return newNode;
            }
        }
        return node;
    }

    private static void Backpropagation(Node node)
    {
        while (node != null)
        {
            node.visits ++;
            node.wins ++;
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
