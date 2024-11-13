using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCTSExam 
{
    private static System.Random random = new System.Random();

    public static NodeExam RunMCTS(NodeExam root, int iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            NodeExam selectedNode = Selection(root);
            NodeExam expandedNode = Expansion(selectedNode);
            //double result = Simulation(expandedNode);
            Backpropagation(expandedNode);
        }
        return root.GetBestChild();
    }

    public static Action Simulation(NodeExam node)
    {
        NodeExam selectedNode = SelectNode(node);
        Action action = new Action("");

        while (selectedNode != null)
        {
            action = action.Combine(selectedNode.action);
            selectedNode = selectedNode.parent;
        }
        return action;
    }

    public static NodeExam SelectNode(NodeExam node)
    {
        if (node.IsFullyExpanded())
        {
            while (node.IsFullyExpanded())
            {
                NodeExam bestChild = node.children[0];
                double bestVisit = node.children[0].visits;

                foreach (NodeExam child in node.children)
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
                    foreach (NodeExam child in bestChild.children)
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
            NodeExam bestChild = node.children[0];
            double bestVisit = node.children[0].visits;

            foreach (NodeExam child in node.children)
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

    public static NodeExam Selection(NodeExam node)
    {
        while (!node.state.IsTerminal() && node.IsFullyExpanded())
        {
            node = UCTSelectChild(node);
        }
        return node;
    }

    private static NodeExam Expansion(NodeExam node)
    {
        List<Action> availableActions = node.state.GetAvailableActions();
        foreach (var action in availableActions)
        {
            if (!node.children.Exists(n => n.action == action))
            {
                GameStateExam newState = node.state.ApplyAction(action);
                NodeExam newNode = new NodeExam(newState, node, action);
                node.children.Add(newNode);
                return newNode;

                /*                if (node.parent != null)
                                {
                                    Node newNode = new Node(newState, node, action.Combine(node.action));
                                    node.children.Add(newNode);
                                    return newNode;
                                }
                                else
                                {
                                    Node newNode = new Node(newState, node, action);
                                    node.children.Add(newNode);
                                    return newNode;
                                }
                */
            }
        }
        return node;
    }

/*    private static double Simulation(Node node)
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
*/
    private static void Backpropagation(NodeExam node)
    {
        while (node != null)
        {
            node.visits++;
            node.wins ++;
            node = node.parent;
        }
    }

    private static NodeExam UCTSelectChild(NodeExam node)
    {
        NodeExam bestChild = null;
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
