using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float seconds;
    public GameObject effect;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(seconds);
        if (effect != null)
            Instantiate(effect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
