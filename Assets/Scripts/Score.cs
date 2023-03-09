using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public static Score instance;

    public int score;
    TMP_Text scoreText;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        scoreText = GetComponent<TMP_Text>();
        score = PlayerPrefs.GetInt("currentScore");
        scoreText.text = "Score: " + score;
    }

    public void AddScore(int _toAdd)
    {
        score += _toAdd;
        scoreText.text = "Score: " + score;

        if (PlayerPrefs.GetInt("highscore") < score)
        {
            PlayerPrefs.SetInt("highscore", score);
        }
        PlayerPrefs.SetInt("currentScore", score);
    }
}
