using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public GameState state;
    public Node parent;
    public List<Node> children;
    public int visits;
    public double wins;
    public Action action;

    public Node(GameState state, Node parent = null, Action action = null)
    {
        this.state = state;
        this.parent = parent;
        this.children = new List<Node>();
        this.visits = 0;
        this.wins = 0;
        this.action = action;
    }

    public bool IsFullyExpanded()
    {
        return children.Count == state.GetAvailableActions().Count;
    }

    public Node GetBestChild()
    {
        Node bestChild = children[0];
        double bestWinRate = children[0].wins / children[0].visits;

        foreach (Node child in children)
        {
            double winRate = child.wins / child.visits;
            if(winRate > bestWinRate)
            {
                bestChild = child;
                bestWinRate = winRate;
            }
        }
        return bestChild;
    }
}
