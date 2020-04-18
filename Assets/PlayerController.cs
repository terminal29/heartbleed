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

    private SpriteRenderer spriteRenderer;
    Coroutine animator;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetState(State.Idle, Direction.Left);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetState(State state, Direction direction)
    {
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
