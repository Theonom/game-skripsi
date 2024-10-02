using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ResetRoundAndWinner()
    {
        GameManager.Instance.SetRound(1);
        GameManager.Instance.SetPlayerWinAmount(0);
        GameManager.Instance.SetAiWinAmount(0);
    }
}
