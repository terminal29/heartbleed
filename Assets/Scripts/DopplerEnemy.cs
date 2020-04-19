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
public class DopplerEnemy : MonoBehaviour
{
    public Sprite[] IdleSprites;
    public Sprite[] WalkSprites;
    public Sprite[] JumpSprites;
    public Sprite[] FallSprites;

    public AudioClip[] StepSounds;
    public AudioClip[] JumpSounds;
    public AudioClip[] StompSounds;

    public AudioClip BeatSound;
    public AudioClip DeathSound;
    public AudioClip RespawnSound;

    public AudioClip bulletShootSound;
    public AudioClip reloadSound;
    public AudioClip bulletHitSound;

    public Light2D gunLight;

    public GameManager gameManager;

    public BulletController bullet;

    private System.Random r = new System.Random();
    private bool wasOnGround = true;

    public bool isAlive = false;

    private Sprite[] currentSpriteList;
    private int currentSpriteIndex = 0;

    public enum State
    {
        Idle,
        Walk,
        Jump,
        Fall
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
    private bool canJump = true;
    private bool canShoot = true;
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

        if (canJump && isOnGround)
        {
            canJump = false;
            StartCoroutine(RandomJump());
        }
        if (canMove)
        {
            canMove = false;
            StartCoroutine(RandomMove());
        }
        if (canShoot)
        {
            canShoot = false;
            StartCoroutine(RandomShoot());
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
        else
        {
            if (body.velocity.y > 0)
            {
                SetState(State.Jump, direction);
            }
            else
            {
                SetState(State.Fall, direction);
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

    public IEnumerator RandomShoot()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1, 5));
        BulletController bulletInstance = Instantiate(bullet);
        double bulletXVelocity = (bulletInitialVelocity.x + bulletRandomSpread * ((2 * r.NextDouble() - 1) / 2));
        bulletXVelocity = direction == Direction.Left ? -bulletXVelocity : bulletXVelocity;
        double bulletYVelocity = (bulletInitialVelocity.y + bulletRandomSpread * ((2 * r.NextDouble() - 1) / 2));
        bulletInstance.transform.position = transform.position + new Vector3(direction == Direction.Left ? -0.8f : 0.8f, 0.2f, 0);
        bulletInstance.GetComponent<Rigidbody2D>().velocity = new Vector2((float)bulletXVelocity, (float)bulletYVelocity) + body.velocity;
        bulletInstance.onBulletHit = () =>
        {
            bulletInstance.GetComponent<AudioSource>().PlayOneShot(bulletHitSound);
        };
        audioSource.PlayOneShot(bulletShootSound);
        yield return RunShootCooldown();
    }

    private IEnumerator RunShootCooldown()
    {
        yield return new WaitForSeconds(shootCooldownSeconds);
        audioSource.PlayOneShot(reloadSound);
        this.canShoot = true;
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

    private IEnumerator RandomJump()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1, 5));
        Vector2 velocity = body.velocity;
        velocity.y = jumpPower;
        body.velocity = velocity;
        canJump = true;
    }

    private IEnumerator RandomMove()
    {

        yield return new WaitForSeconds(UnityEngine.Random.Range(1, 5));
        Vector2 velocity = body.velocity;
        velocity.x = UnityEngine.Random.Range(-1f, 1f) * moveSpeed;
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
            case State.Jump:
                currentSpriteList = JumpSprites;
                break;
            case State.Fall:
                currentSpriteList = FallSprites;
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

        int coins = (int)(gameManager.GetLootMultiplier() * UnityEngine.Random.Range(3, 6));
        for (int i = 0; i < coins; i++)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject coin = Instantiate(gameManager.GetCoinPrefab(), pos, Quaternion.identity);
            coin.GetComponent<Rigidbody2D>().velocity = new Vector2(UnityEngine.Random.Range(-3, 3), 3);
        }
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    public void Respawn()
    {
        body.velocity = new Vector2(0, 0);
        StartCoroutine(RunRespawnAnimation());
    }

    private IEnumerator RunRespawnAnimation()
    {
        audioSource.PlayOneShot(RespawnSound);
        yield return new WaitForSeconds(2.2f);
        SetAlive(true);
        SetState(State.Idle, Direction.Left);
    }

    public void Damage(int amount)
    {
        Die();
    }
}
