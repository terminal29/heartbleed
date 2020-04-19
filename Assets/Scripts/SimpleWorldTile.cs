using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWorldTile : IWorldTile
{
    public Sprite
        Default;

    public enum SpriteVariant
    {
        Default
    }

    SpriteRenderer spriteRenderer;
    SpriteVariant spriteType = SpriteVariant.Default;

    public override bool DoesConnect()
    {
        return false;
    }

    protected void UpdateRenderer()
    {
        if (spriteRenderer)
            switch (spriteType)
            {
                default:
                    spriteRenderer.sprite = Default;
                    break;
            }

    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateRenderer();
    }

    public override void ResolveSpriteVariant(Vector2Int p, WorldGenerator generator)
    {
        SetSprite(SpriteVariant.Default);
    }

    public void SetSprite(SpriteVariant type)
    {
        spriteType = type;
        UpdateRenderer();
    }
}
