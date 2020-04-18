using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    public Sprite[] IdleSprites;
    public Sprite[] WalkSprites;
    public Sprite[] JumpSprites;
    public Sprite[] FallSprites;

    private int currentSpriteIndex = 0;

    private Sprite[] currentSpriteList;
    public enum State
    {
        Idle,
        Walk,
        Jump,
        Fall
    }
    public enum Direction
    {
        Left, Right
    }

    private State state;
    private Direction direction;

    public float moveSpeed = 1.0f;
    public float jumpPower = 1.0f;
    public LayerMask groundCollisionMask;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D body;
    private CapsuleCollider2D capCollider;
    Coroutine animator;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        capCollider = GetComponent<CapsuleCollider2D>();
        SetState(State.Idle, Direction.Left);
    }

    // Update is called once per frame
    void Update()
    {
        bool isOnGround = IsOnGround();

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            Jump();
        }

        Move();

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
    }

    public void Spawn(Vector2 pos)
    {
        transform.position = pos;
    }

    private bool IsOnGround()
    {
        RaycastHit2D ray = Physics2D.BoxCast(capCollider.bounds.center, capCollider.bounds.size, 0f, Vector2.down, 0.1f, groundCollisionMask);
        return ray.collider != null;
    }

    private void Jump()
    {
        Vector2 velocity = body.velocity;
        velocity.y = jumpPower;
        body.velocity = velocity;
    }

    private void Move()
    {
        Vector2 velocity = body.velocity;
        velocity.x = Input.GetAxis("Horizontal") * moveSpeed;
        body.velocity = velocity;
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

    private IEnumerator RunFrameAnimation()
    {
        while (true)
        {
            NextAnimatorIndex();
            yield return new WaitForSeconds(0.3f);
        }
    }
}
