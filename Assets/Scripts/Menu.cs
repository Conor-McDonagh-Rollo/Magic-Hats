using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    public Image curtains;
    bool nextScene = false;
    bool openScene = true;

    public Text highScoreText;

    private void Start()
    {
        curtains.gameObject.SetActive(true);
        if (PlayerPrefs.HasKey("highscore"))
        {
            highScoreText.text = "Highscore: " + PlayerPrefs.GetInt("highscore");
            highScoreText.transform.GetChild(0).GetComponent<Text>().text = "Highscore: " + PlayerPrefs.GetInt("highscore");
        }
        PlayerPrefs.SetInt("currentScore", 0);
    }

    public void PlayGame()
    {
        nextScene = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (nextScene)
        {
            curtains.color += new Color(0, 0, 0, Time.deltaTime * 5);
            if (curtains.color.a >= 1)
            {
                curtains.color = Color.black;
                nextScene = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        if (openScene)
        {
            curtains.color -= new Color(0, 0, 0, Time.deltaTime * 5);
            if (curtains.color.a <= 0)
            {
                curtains.color = Color.clear;
                openScene = false;
            }
        }
    }
}
