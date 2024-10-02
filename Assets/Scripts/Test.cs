using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Stats stats;

    public float playerHP;
    public float aiHP;
    public float playerSP;
    public float aiSP;
    public bool playerAttack;

    // Start is called before the first frame update
    void Start()
    {
        stats.playerHealtBar.maxValue = playerHP;
        stats.aiHealthBar.maxValue = aiHP;
        stats.playerSkillBar.maxValue = playerSP;
        stats.aiSkillBar.maxValue = aiSP;
        Player.playerAttack = playerAttack;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
