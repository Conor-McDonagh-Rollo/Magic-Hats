using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootInDirection : MonoBehaviour
{
    public float speed;
    public Vector2 direction;
    public GameObject effect;
    public bool friendly = false;
    public bool freezing = false;
    bool hit = false;

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)direction * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hit)
            return;
        if (collision.tag == "Wall")
        {
            if (effect != null)
                Instantiate(effect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        if (collision.tag == "Player")
        {
            if (!friendly)
            {
                hit = true;
                if (!Player.instance.protectOn)
                {
                    if (!freezing)
                    {
                        Player.instance.Hurt(10);
                    }
                    else
                    {
                        Player.instance.Hurt(5);
                        Player.instance.frozen = true;
                        Player.instance.body.velocity = Vector2.zero;
                    }
                }
                else
                {
                    Player.instance.ParrySound();
                }
                if (effect != null)
                    Instantiate(effect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
        if (collision.tag == "Enemy")
        {
            if (friendly)
            {
                hit = true;
                if (freezing)
                {
                    collision.GetComponent<Enemy>().frozen = true;
                }

                collision.GetComponent<Enemy>().Hurt(10);
                
                if (effect != null)
                    Instantiate(effect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
        if (collision.tag == "Enemy2")
        {
            if (friendly)
            {
                hit = true;
                if (freezing)
                {
                    collision.GetComponent<Enemy2>().frozen = true;
                }

                collision.GetComponent<Enemy2>().Hurt(10);

                if (effect != null)
                    Instantiate(effect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
