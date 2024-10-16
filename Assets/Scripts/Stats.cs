using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public GameObject player;
    public GameObject npc;

    [Header("Bar")]
    public Slider playerHealtBar;
    public Slider playerSkillBar;
    public Slider aiHealthBar;
    public Slider aiSkillBar;

    [Header("Testing Player Text")]
    public Text playerHPText;
    public Text playerSPText;
    public Text playerPositionX;
    public Text playerPositionY;
    public Text playerAttack;

    [Header("Testing NPC Text")]
    public Text npcHPText;
    public Text npcSPText;
    public Text npcPositionX;
    public Text npcPositionY;
    public Text npcAttack;

    public Text timeText;

    public static float timer;

    // Start is called before the first frame update
    void Start()
    {
        Player.playerHealthPoint = playerHealtBar.maxValue;
        Player.playerSkillPoint = playerSkillBar.maxValue;
        AIMcts.aiHealthPoint = aiHealthBar.maxValue;
        AIMcts.aiSkillsPoint = aiSkillBar.maxValue;
        timer = 60;
    }

    // Update is called once per frame
    void Update()
    {
        playerHealtBar.value = Player.playerHealthPoint;
        playerSkillBar.value = Player.playerSkillPoint;
        aiHealthBar.value = AIMcts.aiHealthPoint;
        aiSkillBar.value = AIMcts.aiSkillsPoint;

        playerHPText.text = playerHealtBar.value.ToString();
        playerSPText.text = playerSkillBar.value.ToString();
        playerPositionX.text = player.transform.position.x.ToString();
        playerPositionY.text = player.transform.position.y.ToString();
        playerAttack.text = Player.playerAttack.ToString();

        npcHPText.text = aiHealthBar.value.ToString();
        npcSPText.text = aiSkillBar.value.ToString();
        npcPositionX.text = npc.transform.position.x.ToString();
        npcPositionY.text = npc.transform.position.y.ToString();
        npcAttack.text = AIMcts.aiAttack.ToString();

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
