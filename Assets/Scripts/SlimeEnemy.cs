using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(DamageableEntity))]
public class SlimeEnemy : MonoBehaviour
{
    public Sprite[] idleSprites;
    public Sprite jumpSprite;
    public LayerMask groundCollisionMask;

    private CircleCollider2D circleCollider;
    private Rigidbody2D body;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    public GameManager manager;

    public AudioClip slimeJump;
    public AudioClip slimeLand;
    public AudioClip slimeDie;

    public Light2D slimeLight;

    bool isAlive = true;
    bool wasOnGround = true;
    bool waitingJump = false;

    private Sprite[] currentSpriteList;
    private int currentSpriteIndex = 0;

    public float jumpCooldownMultiplier = 1f;

    private State state = State.Jump;
    private Direction direction = Direction.Right;

    public enum State
    {
        Idle,
        Jump,
    }

    public enum Direction
    {
        Left,
        Right
    }

    Coroutine animator;

    public void SetLightColor(Color newColor)
    {
        slimeLight.color = newColor;
    }

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        circleCollider = GetComponent<CircleCollider2D>();
        body = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GetComponent<DamageableEntity>().onDamage = (amount) =>
        {
            Die();
        };
        SetState(State.Idle, Direction.Left);
    }

    // Update is called once per frame
    void Update()
    {
        bool isOnGround = IsOnGround();
        if (wasOnGround && !isOnGround)
        {
            audioSource.PlayOneShot(slimeJump);
        }
        if (isOnGround && !wasOnGround)
        {
            body.velocity = new Vector2(0, body.velocity.y);
            audioSource.PlayOneShot(slimeLand);
            if (!waitingJump)
                StartCoroutine(JumpLater());
        }
        if (isOnGround && !waitingJump)
        {
            StartCoroutine(JumpLater());
        }
        wasOnGround = isOnGround;
    }

    private void Die()
    {
        circleCollider.enabled = false;
        spriteRenderer.enabled = false;
        isAlive = false;
        StartCoroutine(Cleanup(transform.position));
    }

    private IEnumerator Cleanup(Vector3 pos)
    {
        audioSource.PlayOneShot(slimeDie);
        int coins = (int)(manager.GetLootMultiplier() * Random.Range(1, 3));
        for (int i = 0; i < coins; i++)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject coin = Instantiate(manager.GetCoinPrefab(), pos, Quaternion.identity);
            coin.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-3, 3), 3);
        }
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    private void SetState(State state, Direction direction)
    {
        if (this.state == state && this.direction == direction)
            return;
        if (animator != null)
            StopCoroutine(animator);
        this.state = state;
        this.direction = direction;
        switch (direction)
        {
            case Direction.Left:
                {
                    Vector3 eu = this.transform.localRotation.eulerAngles;
                    eu.y = 180;
                    this.transform.localRotation = Quaternion.Euler(eu);
                }
                break;
            case Direction.Right:
                {
                    Vector3 eu = this.transform.localRotation.eulerAngles;
                    eu.y = 0;
                    this.transform.localRotation = Quaternion.Euler(eu);
                }
                break;
        }
        switch (state)
        {
            case State.Idle:
                currentSpriteList = idleSprites;
                break;
            case State.Jump:
                currentSpriteList = new Sprite[] { jumpSprite };
                break;
        }
        animator = StartCoroutine(RunFrameAnimation());
    }

    private bool IsOnGround()
    {
        RaycastHit2D ray = Physics2D.BoxCast(circleCollider.bounds.center, circleCollider.bounds.size - new Vector3(0.1f, 0, 0), 0f, Vector2.down, 0.1f, groundCollisionMask);
        Color r = Color.red;
        if (ray.collider != null)
            r = Color.blue;

        Debug.DrawRay(circleCollider.bounds.center + new Vector3(circleCollider.bounds.extents.x - 0.1f, 0), Vector2.down * (circleCollider.bounds.extents.y + 0.1f), r);
        Debug.DrawRay(circleCollider.bounds.center - new Vector3(circleCollider.bounds.extents.x - 0.1f, 0), Vector2.down * (circleCollider.bounds.extents.y + 0.1f), r);
        return ray.collider != null;
    }

    private void NextAnimatorIndex()
    {
        currentSpriteIndex++;
        if (currentSpriteIndex >= currentSpriteList.Length)
        {
            currentSpriteIndex = 0;
        }
        spriteRenderer.sprite = currentSpriteList[currentSpriteIndex];
    }

    private IEnumerator RunFrameAnimation()
    {
        while (true)
        {
            if (isAlive)
            {
                NextAnimatorIndex();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator JumpLater()
    {
        if (waitingJump)
            yield return null;
        waitingJump = true;
        yield return new WaitForSeconds(Random.Range(1, 4) * jumpCooldownMultiplier);
        if (isAlive)
            Jump();
        waitingJump = false;
    }

    private void Jump()
    {
        Vector2 jumpVector = new Vector2(Random.Range(-10, 10), 10);
        body.velocity = jumpVector;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<PlayerController>())
            collision.collider.gameObject.GetComponent<DamageableEntity>()?.Damage(1);
    }
}
