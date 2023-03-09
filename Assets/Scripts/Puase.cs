using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Puase : MonoBehaviour
{
    public Image curtains;
    bool nextScene = false;

    public void Menu()
    {
        Time.timeScale = 1.0f;
        curtains.gameObject.SetActive(true);
        nextScene = true;
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }

    public void Exit()
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
                SceneManager.LoadScene(0);
            }
        }
    }
}
