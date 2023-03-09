using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class FireballSpell : MonoBehaviour
{
    float timer = 0;
    List<string> alreadyDamaged = new List<string>();

    private void Start()
    {
        Player.instance.ScreenShake();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1.0f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (alreadyDamaged.Contains(collision.name))
            return;
        if (collision.tag == "Player")
        {
            if (!Player.instance.protectOn)
            {
                Player.instance.Hurt(10);
            }
            else
            {
                Player.instance.ParrySound();
            }
                
            alreadyDamaged.Add(collision.gameObject.name);
        }
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().Hurt(25);
            alreadyDamaged.Add(collision.gameObject.name);
        }
        if (collision.tag == "Enemy2")
        {
            collision.GetComponent<Enemy2>().Hurt(25);
            alreadyDamaged.Add(collision.gameObject.name);
        }
    }

}
