using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Enemy2 : MonoBehaviour
{
    NavMeshAgent agent;
    Transform player;
    SpriteRenderer sprite;

    bool stop = false;

    // HEALTH
    int health = 20;
    bool flashing = false;
    bool damageFlash = false;
    GameObject effect;

    // SPELLS
    public GameObject iceCast;
    bool readyToCast = true;
    public bool frozen = false;
    float frozenCooldown = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        player = Player.instance.transform;
        sprite = GetComponent<SpriteRenderer>();
        effect = Resources.Load<GameObject>("Explode");
        effect.GetComponent<ExplodeParticles>().color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        if (frozen)
        {
            frozenCooldown -= Time.deltaTime;
            if (frozenCooldown <= 0.0f)
            {
                frozen = false;
                frozenCooldown = 5.0f;
            }
        }

        //PATHING & SPELLS
        if (!frozen) {
            if (player == null)
                return;
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance < 10) // stop to shoot
            {
                if (!stop)
                {
                    stop = true;
                    agent.isStopped = true; ;
                }

                //rotate
                Vector2 lookdirection = (player.position - transform.position).normalized;

                float angle = Mathf.Atan2(lookdirection.y, lookdirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


                //start shooting
                if (readyToCast)
                    StartCoroutine(Cast());
            }
            else if (distance > 14) // lose interest
            {
                if (!stop)
                {
                    stop = true;
                    agent.isStopped = true; ;
                }
            }
            else if (stop)
            {
                stop = false;
                agent.isStopped = false;
            }
            if (!stop)
            {
                agent.SetDestination(player.position);

                Vector2 lookdirection = (agent.desiredVelocity).normalized;

                float angle = Mathf.Atan2(lookdirection.y, lookdirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }
        else
        {
            if (!agent.isStopped)
            {
                agent.isStopped = true;
            }
        }

        // DAMAGE FLASH
        if (flashing)
        {
            if (damageFlash)
            {
                sprite.color -= new Color(0, Time.deltaTime, Time.deltaTime, 0) * 8f;
            }
            else
            {
                sprite.color += new Color(0, Time.deltaTime, Time.deltaTime, 0) * 8f;
            }
        }
        
    }

    IEnumerator Cast()
    {
        readyToCast = false;
        yield return new WaitForSeconds(1.5f);
        if (!frozen)
        {
            ShootInDirection s = Instantiate(iceCast, transform.position, transform.rotation).GetComponent<ShootInDirection>();
            s.direction = (player.position - transform.position).normalized;
            s.freezing = true;
        }
        readyToCast = true;
    }

    public void Hurt(int _amount)
    {
        health -= _amount;
        if (!flashing)
            StartCoroutine(FlashDamage());

        if (health <= 0)
        {
            if (effect != null)
                Instantiate(effect, transform.position, transform.rotation);
            Score.instance.AddScore(10);
            Destroy(gameObject);
        }
    }

    IEnumerator FlashDamage()
    {
        damageFlash = true;
        flashing = true;
        yield return new WaitForSeconds(0.15f);
        damageFlash = false;
        yield return new WaitForSeconds(0.15f);
        flashing = false;
        sprite.color = Color.white;
    }
}
