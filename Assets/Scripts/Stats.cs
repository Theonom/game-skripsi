using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    [Header("Bar")]
    public Slider playerHealtBar;
    public Slider playerSkillBar;
    public Slider aiHealthBar;
    public Slider aiSkillBar;

    [Header("Text")]
    public Text playerHPText;
    public Text aiHPText;
    public Text playerSPText;
    public Text aiSPText;
    public Text playerAttack;
    public Text timeText;

    public static float timer;

    // Start is called before the first frame update
    void Start()
    {
        Player.playerHealthPoint = playerHealtBar.maxValue;
        Player.playerSkillPoint = playerSkillBar.maxValue;
        AI.aiHealthPoint = aiHealthBar.maxValue;
        AI.aiSkillsPoint = aiSkillBar.maxValue;
        timer = 60;
    }

    // Update is called once per frame
    void Update()
    {
        playerHealtBar.value = Player.playerHealthPoint;
        playerSkillBar.value = Player.playerSkillPoint;
        aiHealthBar.value = AI.aiHealthPoint;
        aiSkillBar.value = AI.aiSkillsPoint;

        playerHPText.text = playerHealtBar.value.ToString();
        aiHPText.text = aiHealthBar.value.ToString();
        playerSPText.text = playerSkillBar.value.ToString();
        aiSPText.text = aiSkillBar.value.ToString();
        playerAttack.text = Player.playerAttack.ToString();

        if (GameController.playGame == true)
        {
            if (timer <= 60)
            {
                timer -= Time.deltaTime;
                DisplayTime(timer);
            }
            if (timer <= 0)
            {
                timer = 0;
            }
        }
    }

    public void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
