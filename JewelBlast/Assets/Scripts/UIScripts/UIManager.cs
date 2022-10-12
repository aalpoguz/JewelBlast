using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    TextMeshProUGUI timeText;
    [SerializeField]
    TextMeshProUGUI scoreText;
    float timeLeft = 50;
    [HideInInspector]
    public int currentScore;
    public GameObject finalScreen;

    private void Awake()
    {
        instance = this;

    }
    private void Update()
    {
        timeLeft -= Time.deltaTime;
        timeText.text = Mathf.Floor(timeLeft).ToString();

        if (timeLeft == 0)
        {
            SoundManager.instance.finishVoiceEff();
            GameOver();
            timeText.text = "0";

        }
    }

    void GameOver()
    {
        finalScreen.SetActive(true);
    }

    public void increaseScore(int externalScore)
    {
        currentScore += externalScore;
        scoreText.text = currentScore.ToString();
    }
}
