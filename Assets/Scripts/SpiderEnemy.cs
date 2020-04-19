using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(DamageableEntity))]
public class SpiderEnemy : MonoBehaviour
{
    public Sprite[] IdleSprites;
    public Sprite[] WalkSprites;

    public AudioClip[] StepSounds;
    public AudioClip[] JumpSounds;
    public AudioClip[] StompSounds;

    public AudioClip BeatSound;
    public AudioClip DeathSound;

    public GameManager gameManager;

    private int health = 5;

    private System.Random r = new System.Random();
    private bool wasOnGround = true;

    public bool isAlive = false;

    private Sprite[] currentSpriteList;
    private int currentSpriteIndex = 0;

    public enum State
    {
        Idle,
        Walk,
    }

    public enum Direction
    {
        Left,
        Right
    }

    private State state;
    private Direction direction;

    public float moveSpeed = 1.0f;
    public float jumpPower = 1.0f;
    public LayerMask groundCollisionMask;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D body;
    private CapsuleCollider2D capCollider;
    private AudioSource audioSource;
    Coroutine animator;

    private bool canMove = true;
    private float shootCooldownSeconds = 1f;
    private Vector2 bulletInitialVelocity = new Vector2(10f, 3f);
    private float bulletRandomSpread = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        capCollider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
        GetComponent<DamageableEntity>().onDamage = (amount) =>
        {
            if (isAlive)
                Die();
        };
        SetState(State.Idle, Direction.Left);
    }

    public void setGameManager(GameManager manager)
    {
        gameManager = manager;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
            return;
        bool isOnGround = IsOnGround();
        if (isOnGround && !wasOnGround)
        {
            audioSource.PlayOneShot(StompSounds[r.Next(0, StompSounds.Length)]);
        }
        if (!isOnGround && wasOnGround)
        {
            audioSource.PlayOneShot(JumpSounds[r.Next(0, JumpSounds.Length)]);
        }

        if (canMove)
        {
            canMove = false;
            StartCoroutine(RandomMove());
        }

        if (isOnGround)
        {
            if (body.velocity.magnitude < 0.1f)
            {
                SetState(State.Idle, direction);
            }
            else
            {
                SetState(State.Walk, direction);
            }
        }

        if (Mathf.Abs(body.velocity.x) > 0.1)
        {
            if (body.velocity.x < 0)
            {
                SetState(state, Direction.Left);
            }
            else
            {
                SetState(state, Direction.Right);
            }
        }
        wasOnGround = isOnGround;
    }

    public void Teleport(Vector2 pos)
    {
        transform.position = pos;
    }

    public void SetAlive(bool alive)
    {
        isAlive = alive;
    }

    private bool IsOnGround()
    {
        RaycastHit2D ray = Physics2D.BoxCast(capCollider.bounds.center, capCollider.bounds.size - new Vector3(0.1f, 0, 0), 0f, Vector2.down, 0.1f, groundCollisionMask);
        Color r = Color.red;
        if (ray.collider != null)
            r = Color.blue;

        Debug.DrawRay(capCollider.bounds.center + new Vector3(capCollider.bounds.extents.x - 0.1f, 0), Vector2.down * (capCollider.bounds.extents.y + 0.1f), r);
        Debug.DrawRay(capCollider.bounds.center - new Vector3(capCollider.bounds.extents.x - 0.1f, 0), Vector2.down * (capCollider.bounds.extents.y + 0.1f), r);
        return ray.collider != null;
    }

    private IEnumerator RandomMove()
    {

        yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 3f));
        Vector2 velocity = body.velocity;
        velocity.x = UnityEngine.Random.Range(-3f, 3f) * moveSpeed;
        body.velocity = velocity;
        canMove = true;
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
                currentSpriteList = IdleSprites;
                break;
            case State.Walk:
                currentSpriteList = WalkSprites;
                break;
        }
        animator = StartCoroutine(RunFrameAnimation());
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

    private void PlaySoundForAnimation()
    {
        if (state == State.Walk && currentSpriteIndex % 2 == 0)
        {
            audioSource.PlayOneShot(StepSounds[r.Next(0, StepSounds.Length)]);
        }
    }

    private IEnumerator RunFrameAnimation()
    {
        while (true)
        {
            if (isAlive)
            {
                NextAnimatorIndex();
                PlaySoundForAnimation();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Die()
    {
        SetAlive(false);
        StartCoroutine(RunDeathAnimation());
    }

    private IEnumerator RunDeathAnimation()
    {
        Vector3 pos = transform.position;
        this.body.velocity = new Vector2(0, 0);
        audioSource.PlayOneShot(DeathSound);
        spriteRenderer.enabled = false;
        capCollider.enabled = false;

        int coins = (int)(gameManager.GetLootMultiplier() * UnityEngine.Random.Range(10, 15));
        for (int i = 0; i < coins; i++)
        {
            yield return new WaitForSeconds(0.01f);
            GameObject coin = Instantiate(gameManager.GetCoinPrefab(), pos, Quaternion.identity);
            coin.GetComponent<Rigidbody2D>().velocity = new Vector2(UnityEngine.Random.Range(-3, 3), 3);
        }
        yield return new WaitForSeconds(2);
        gameManager.OnSpiderKilled();
        Destroy(gameObject);
    }

    public void Damage(int amount)
    {
        health -= amount;
        if (health <= 0)
            Die();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<PlayerController>())
            collision.collider.gameObject.GetComponent<DamageableEntity>()?.Damage(1);
    }
}
