using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameState
{
    public string hpComparision;
    public string hpNPC;
    public string spNPC;
    public string playerAttack;
    public string positionY;

    public GameState(string comparisionHp, string npcHp, string npcSp, string attackPlayer, string yPosition)
    {
        hpComparision = comparisionHp;
        hpNPC = npcHp;
        spNPC = npcSp;
        playerAttack = attackPlayer;
        positionY = yPosition;
    }
}
