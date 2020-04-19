using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SpriteRenderer))]
public class BulletController : MonoBehaviour
{
    public LayerMask ignoreLayer;
    public BulletParticle bulletParticle;
    public Action onBulletHit;
    private CircleCollider2D circleCollider;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D body;
    public int bulletDamage = 1;
    // Start is called before the first frame update
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<DamageableEntity>()?.Damage(bulletDamage);

        onBulletHit?.Invoke();
        circleCollider.enabled = false;
        spriteRenderer.enabled = false;
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
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
        yield return null;
    }
}
