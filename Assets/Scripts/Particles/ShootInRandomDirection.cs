using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootInRandomDirection : MonoBehaviour
{
    public float speed;
    Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        float rand = (Mathf.PI / 180) * Random.Range(0, 360);
        direction = new Vector2((float)Mathf.Cos(rand), (float)Mathf.Sin(rand)).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)direction * Time.deltaTime * speed;
    }
}
