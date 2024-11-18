using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private bool isPause;
    public static bool playGame;

    public GameObject gameOverPopUp;
    public GameObject pausePopUp;
    public Text winnerText;

    public Player playerScript;
    public AI aiScrpt;

    [Header("Spawn Point")]
    public Transform spawn1;
    public Transform spawn2;

    [Header("Character")]
    public GameObject player;
    public GameObject npc;

    [Header("WinnerInRound")]
    public GameObject panelNextRound;

    public Text winnerName;
    public Text round;

    private void Start()
    {
        playGame = false;
        isPause = false;
        AudioManager.instance.PlayMusic("BGM");
        AudioManager.instance.SetMusicVolume(1.0f);
        PositionCharacter();
        panelNextRound.SetActive(false);
        Pause(1);
    }

    // Update is called once per frame
    void Update()
    {
        GameOver();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPause)
            {
                pausePopUp.SetActive(true);
                Pause(0);
                isPause = true;
            }
            else
            {
                pausePopUp.SetActive(false);
                Pause(1);
                isPause = false;
            }
        }
    }

    public void GameOver()
    {
        if (Stats.timer <= 0 || Player.playerHealthPoint <= 0 || AI.aiHealthPoint <= 0)
        {
            if (GameManager.Instance.GetPlayerWinAmount() < 2 && GameManager.Instance.GetAiWinAmount() < 2)
            {
                panelNextRound.SetActive(true);
                round.text = "Round" + GameManager.Instance.GetRound();

                if (Player.playerHealthPoint > AI.aiHealthPoint)
                {
                    winnerName.text = "Player Win";
                }
                if (Player.playerHealthPoint < AI.aiHealthPoint)
                {
                    winnerName.text = "Computer Win";
                }
                if (Player.playerHealthPoint == AI.aiHealthPoint)
                {
                    winnerName.text = "Draw";
                }

                Pause(0);
            }

            if (GameManager.Instance.GetPlayerWinAmount() == 1 && Player.playerHealthPoint > AI.aiHealthPoint)
            {
                winnerText.text = "Player Win";
                gameOverPopUp.SetActive(true);
                Pause(0);
            }
            if (GameManager.Instance.GetAiWinAmount() == 1 && Player.playerHealthPoint < AI.aiHealthPoint)
            {
                winnerText.text = "Computer Win";
                gameOverPopUp.SetActive(true);
                Pause(0);
            }
            if (GameManager.Instance.GetAiWinAmount() == 1 && GameManager.Instance.GetPlayerWinAmount() == 1 && Player.playerHealthPoint == AI.aiHealthPoint)
            {
                winnerText.text = "Draw";
                gameOverPopUp.SetActive(true);
                Pause(0);
            }
        }
    }

    public void WinnerInRound()
    {
        if (Player.playerHealthPoint > AI.aiHealthPoint)
        {
            GameManager.Instance.AddPlayerWinAmount(1);
        }
        if (Player.playerHealthPoint < AI.aiHealthPoint)
        {
            GameManager.Instance.AddAIWinAmount(1);
        }
        if (Player.playerHealthPoint == AI.aiHealthPoint)
        {
            Debug.Log("DrawInRound");
        }
    }

    public void PositionCharacter()
    {
        if (GameManager.Instance.GetRound() == 1)
        {
            player.transform.position = new Vector2(spawn1.position.x, spawn1.position.y);
            npc.transform.position = new Vector2(spawn2.position.x, spawn2.position.y);
        }
        if (GameManager.Instance.GetRound() == 2)
        {
            player.transform.position = new Vector2(spawn2.position.x, spawn2.position.y);
            npc.transform.position = new Vector2(spawn1.position.x, spawn1.position.y);
        }
        if (GameManager.Instance.GetRound() == 3)
        {
            player.transform.position = new Vector2(spawn1.position.x, spawn1.position.y);
            npc.transform.position = new Vector2(spawn2.position.x, spawn2.position.y);
        }
    }

    public void NextRound()
    {
        WinnerInRound();
        GameManager.Instance.AddRound(1);
        SceneManager.LoadScene("Play");
    }

    public void Pause(int value)
    {
        Time.timeScale = value;
    }
}
