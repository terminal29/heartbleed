using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class Coin : MonoBehaviour
{
    public AudioClip spawnSound;
    public AudioClip pickupSound;

    private Rigidbody2D body;
    private CircleCollider2D circleCollider;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource.PlayOneShot(spawnSound);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 eu = transform.rotation.eulerAngles;
        eu.y += Time.deltaTime * 100;
        transform.rotation = Quaternion.Euler(eu);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            this.circleCollider.enabled = false;
            this.spriteRenderer.enabled = false;
            GameObject.Find("GameManager").GetComponent<GameManager>().AddCoins(1);
            StartCoroutine(Cleanup());
        }
    }

    private IEnumerator Cleanup()
    {
        audioSource.PlayOneShot(pickupSound);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
