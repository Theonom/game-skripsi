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

    private void Start()
    {
        playGame = false;
        isPause = false;
        AudioManager.instance.PlayMusic("BGM");
        AudioManager.instance.SetMusicVolume(1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //GameOver();

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
                WinnerInRound();
                GameManager.Instance.AddRound(1);
                SceneManager.LoadScene("Play");
            }
        }

        if (GameManager.Instance.GetPlayerWinAmount() == 2 || GameManager.Instance.GetAiWinAmount() == 2 || GameManager.Instance.GetRound() > 3)
        {
            WinnerInGame();
            gameOverPopUp.SetActive(true);
            Pause(0);
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

    public void WinnerInGame()
    {
        if (GameManager.Instance.GetPlayerWinAmount() > GameManager.Instance.GetAiWinAmount())
        {
            winnerText.text = "Player Win";
        }
        if (GameManager.Instance.GetPlayerWinAmount() < GameManager.Instance.GetAiWinAmount())
        {
            winnerText.text = "Computer Win";
        }
        if (GameManager.Instance.GetPlayerWinAmount() == GameManager.Instance.GetAiWinAmount())
        {
            winnerText.text = "Draw";
        }
    }

    public void Pause(int value)
    {
        Time.timeScale = value;
    }
}
