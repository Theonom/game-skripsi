using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public GameObject player;
    public GameObject npc;

    [Header("Bar")]
    public Slider leftHealtBar;
    public Slider leftSkillBar;
    public Slider rightHealthBar;
    public Slider rightSkillBar;

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

    [Header("Object Testing")]
    public RectTransform playerTest;
    public RectTransform npcTest;

    public Text timeText;

    public static float timer;
    private float timerAddSkillPoint;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.GetRound() == 1 || GameManager.Instance.GetRound() == 3)
        {
            Player.playerHealthPoint = leftHealtBar.maxValue;
            Player.playerSkillPoint = leftSkillBar.maxValue;
            AI.aiHealthPoint = rightHealthBar.maxValue;
            AI.aiSkillsPoint = rightSkillBar.maxValue;
        }
        if (GameManager.Instance.GetRound() == 2)
        {
            Player.playerHealthPoint = rightHealthBar.maxValue;
            Player.playerSkillPoint = rightSkillBar.maxValue;
            AI.aiHealthPoint = leftHealtBar.maxValue;
            AI.aiSkillsPoint = leftSkillBar.maxValue;
        }

        timer = 60;
        ChangePositionObjetTest();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GetRound() == 1 || GameManager.Instance.GetRound() == 3)
        {
            leftHealtBar.value = Player.playerHealthPoint;
            leftSkillBar.value = Player.playerSkillPoint;
            rightHealthBar.value = AI.aiHealthPoint;
            rightSkillBar.value = AI.aiSkillsPoint;

            playerHPText.text = leftHealtBar.value.ToString();
            playerSPText.text = leftSkillBar.value.ToString();
            playerPositionX.text = player.transform.position.x.ToString();
            playerPositionY.text = player.transform.position.y.ToString();
            playerAttack.text = Player.playerAttack.ToString();

            npcHPText.text = rightHealthBar.value.ToString();
            npcSPText.text = rightSkillBar.value.ToString();
            npcPositionX.text = npc.transform.position.x.ToString();
            npcPositionY.text = npc.transform.position.y.ToString();
            npcAttack.text = AI.aiAttack.ToString();
        }
        if (GameManager.Instance.GetRound() == 2)
        {
            rightHealthBar.value = Player.playerHealthPoint;
            rightSkillBar.value = Player.playerSkillPoint;
            leftHealtBar.value = AI.aiHealthPoint;
            leftSkillBar.value = AI.aiSkillsPoint;

            playerHPText.text = rightHealthBar.value.ToString();
            playerSPText.text = rightSkillBar.value.ToString();
            playerPositionX.text = player.transform.position.x.ToString();
            playerPositionY.text = player.transform.position.y.ToString();
            playerAttack.text = Player.playerAttack.ToString();

            npcHPText.text = leftHealtBar.value.ToString();
            npcSPText.text = leftSkillBar.value.ToString();
            npcPositionX.text = npc.transform.position.x.ToString();
            npcPositionY.text = npc.transform.position.y.ToString();
            npcAttack.text = AI.aiAttack.ToString();
        }

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

        AddSkillPoint();
    }

    public void AddSkillPoint()
    {
        if (Player.playerSkillPoint < 10)
        {
            timerAddSkillPoint += Time.deltaTime;

            if (timerAddSkillPoint >= 1)
            {
                Player.playerSkillPoint += 2;
                timerAddSkillPoint = 0;
            }
        }
        if (AI.aiSkillsPoint < 10)
        {
            timerAddSkillPoint += Time.deltaTime;

            if (timerAddSkillPoint >= 1)
            {
                AI.aiSkillsPoint += 2;
                timerAddSkillPoint = 0;
            }
        }
    }

    public void ChangePositionObjetTest()
    {
        if (GameManager.Instance.GetRound() == 1 || GameManager.Instance.GetRound() == 3)
        {
            playerTest.anchoredPosition = new Vector2(103, 85.79999f);
            npcTest.anchoredPosition = new Vector2(667, 85.79999f);
        }
        if (GameManager.Instance.GetRound() == 2)
        {
            playerTest.anchoredPosition = new Vector2(667, 85.79999f);
            npcTest.anchoredPosition = new Vector2(103, 85.79999f);
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
