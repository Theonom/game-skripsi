using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Action
{
    public string name;

    public Action(string name)
    {
        this.name = name;
    }

    public Action Combine(Action actionParent)
    {
        string combineName = "";
        if (actionParent != null)
        {
            combineName = actionParent.name + "." + this.name;
            return new Action(combineName);
        }
        else
        {
            combineName = this.name;
            return new Action(combineName);
        }
    }
}
