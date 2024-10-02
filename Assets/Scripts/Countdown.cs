using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    [HideInInspector] public float timeStart;

    public Text timeStartText;
    public GameObject timeStartPanel;

    // Start is called before the first frame update
    void Start()
    {
        timeStart = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeStart += Time.deltaTime;

        TimeCountdown();
    }

    private void TimeCountdown()
    {
        if (timeStart < 1)
        {
            timeStartText.text = "Round " + GameManager.Instance.GetRound();
        }
        if (timeStart >= 1)
        {
            timeStartText.text = "Ready";
        }
        if (timeStart >= 2)
        {
            timeStartText.text = "Fight";
        }  
        if (timeStart >= 3)
        {
            timeStartPanel.SetActive(false);
            GameController.playGame = true;
        }
    }

}
