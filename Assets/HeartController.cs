using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HeartController : MonoBehaviour
{
    public Sprite[] heartSprites;
    public Sprite deadHeartSprite;

    public delegate void OnHeartbeat();

    public OnHeartbeat onHeartbeat;

    private Image image;

    private int maxHealth = 4;
    private int health = 4;

    private float scaleX = 1;
    private float scaleY = 1;

    private float beatInterval = 0.2f;

    public void SetHealth(int health)
    {
        this.health = health;
        beatInterval = (((health) / (float)maxHealth)) * 0.2f;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetHealth()
    {
        return health;
    }

    private void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine(HeartAnimation());
        StartCoroutine(HeartAnimator());
    }

    private void Update()
    {
        if (health > 0)
        {
            image.sprite = heartSprites[maxHealth - health];
        }
        else
        {
            image.sprite = deadHeartSprite;
        }
    }

    private IEnumerator HeartAnimator()
    {
        while (true)
        {
            scaleX = 1f;
            scaleY = 1f;
            yield return new WaitForSeconds(beatInterval * 5);
            onHeartbeat?.Invoke();
            scaleX = 1.2f;
            scaleY = 0.1f;
            yield return new WaitForSeconds(beatInterval);
            onHeartbeat?.Invoke();
            scaleY = 1.2f;
            scaleX = 0.1f;
            yield return new WaitForSeconds(beatInterval);
        }
    }

    private IEnumerator HeartAnimation()
    {
        while (true)
        {
            Vector3 vec = transform.localScale;
            vec.x = Mathf.Lerp(vec.x, scaleX, 10 * Time.deltaTime);
            vec.y = Mathf.Lerp(vec.y, scaleY, 10 * Time.deltaTime);
            transform.localScale = vec;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
