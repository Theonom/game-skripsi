using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int round;
    [SerializeField] private int playerWinAmount;
    [SerializeField] private int aiWinAmount;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        round = 1;
        playerWinAmount = 0;
        aiWinAmount = 0;
    }

    public void AddRound(int value)
    {
        round += value;
    }

    public int GetRound()
    {
        return round;
    }

    public void SetRound(int value)
    {
        round = value;
    }

    public void AddPlayerWinAmount(int value)
    {
        playerWinAmount += value;
    }

    public int GetPlayerWinAmount()
    {
        return playerWinAmount;
    }

    public void SetPlayerWinAmount(int value)
    {
        playerWinAmount = value;
    }

    public void AddAIWinAmount(int value)
    {
        aiWinAmount += value;
    }

    public int GetAiWinAmount()
    {
        return aiWinAmount;
    }

    public void SetAiWinAmount(int value)
    {
        aiWinAmount = value;
    }
}
