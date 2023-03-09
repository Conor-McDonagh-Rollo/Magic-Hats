using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextScene : MonoBehaviour
{
    public static NextScene instance;

    public Image curtains;
    bool nextScene = false;
    bool openScene = true;

    int amountOfDecals = 0;
    public GameObject decal;
    public List<GameObject> decalsArr = new List<GameObject>();

    private void Start()
    {
        instance = this; 

        curtains.gameObject.SetActive(true);

        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        amountOfDecals = PlayerPrefs.GetInt("amountOfDecals" + buildIndex, 0);

        for (int i = 0; i < amountOfDecals; i++)
        {
            string s = PlayerPrefs.GetString(buildIndex + "decal" + i);
            string[] ssplit = s.Split(",");
            GameObject go = Instantiate(decal, new Vector2(float.Parse(ssplit[0]), float.Parse(ssplit[1])), Quaternion.identity);
            decalsArr.Add(go);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            nextScene = true;
        }
    }

    private void Update()
    {
        if (nextScene)
        {
            curtains.color += new Color(0,0,0, Time.deltaTime * 5);
            
            if (curtains.color.a >= 1)
            {
                curtains.color = Color.black;
                nextScene = false;

                //decals perminance
                int buildIndex = SceneManager.GetActiveScene().buildIndex;
                for (int i = 0; i < decalsArr.Count; i++)
                {
                    PlayerPrefs.SetString(buildIndex + "decal" + i, decalsArr[i].transform.position.x.ToString() + "," + decalsArr[i].transform.position.y.ToString());
                }
                PlayerPrefs.SetInt("amountOfDecals" + buildIndex, decalsArr.Count);

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
