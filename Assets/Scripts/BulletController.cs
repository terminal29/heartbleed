using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class BulletController : MonoBehaviour
{
    public LayerMask ignoreLayer;
    public BulletParticle bulletParticle;
    public Action onBulletHit;
    private CircleCollider2D circleCollider;
    private Rigidbody2D body;
    // Start is called before the first frame update
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        body = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<DamageableEntity>() != null)
        {
            collision.gameObject.GetComponent<DamageableEntity>().Damage(1);
        }
        onBulletHit?.Invoke();
        circleCollider.enabled = false;
        StartCoroutine(PopAnimation());
    }

    private IEnumerator PopAnimation()
    {
        int nParticles = UnityEngine.Random.Range(2, 6);
        for (int i = 0; i < nParticles; i++)
        {
            BulletParticle particle = Instantiate(bulletParticle, transform.position, Quaternion.identity);
            particle.GetComponent<Rigidbody2D>().velocity = new Vector2(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(0, 2));
        }
        Destroy(gameObject);
        yield return null;
    }
}
