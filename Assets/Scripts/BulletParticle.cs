using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class BulletParticle : MonoBehaviour
{
    float lifetime;

    // Start is called before the first frame update
    void Start()
    {
        lifetime = Random.Range(0.5f, 1.5f);
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
