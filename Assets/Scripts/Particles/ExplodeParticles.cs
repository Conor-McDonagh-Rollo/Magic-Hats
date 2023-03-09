using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeParticles : MonoBehaviour
{
    public GameObject particle;
    public Color color;

    float timer = 0;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.5f)
            Destroy(gameObject);

        
        Instantiate(particle, transform.position, Quaternion.identity).GetComponent<SpriteRenderer>().color = color;
    }
}
