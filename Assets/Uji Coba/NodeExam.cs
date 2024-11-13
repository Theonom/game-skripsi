using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeExam 
{
    public GameStateExam state;
    public NodeExam parent;
    public List<NodeExam> children;
    public int visits;
    public double wins;
    public Action action;

    public NodeExam(GameStateExam state, NodeExam parent = null, Action action = null)
    {
        this.state = state;
        this.parent = parent;
        this.children = new List<NodeExam>();
        this.visits = 0;
        this.wins = 0;
        this.action = action;
    }

    public bool IsFullyExpanded()
    {
        return children.Count == state.GetAvailableActions().Count;
    }

    public NodeExam GetBestChild()
    {
        NodeExam bestChild = children[0];
        double bestWinRate = children[0].wins / children[0].visits;

        foreach (NodeExam child in children)
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
