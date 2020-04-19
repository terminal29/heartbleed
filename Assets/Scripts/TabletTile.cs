using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TabletTile : IWorldTile
{
    public Sprite TopLeft, TopRight, BottomLeft, BottomRight;
    public enum SpriteVariant
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    SpriteRenderer spriteRenderer;
    SpriteVariant spriteType = SpriteVariant.TopLeft;

    public override bool DoesConnect()
    {
        return false;
    }

    protected void UpdateRenderer()
    {
        if (spriteRenderer)
            switch (spriteType)
            {
                case SpriteVariant.TopLeft:
                    spriteRenderer.sprite = TopLeft;
                    break;
                case SpriteVariant.TopRight:
                    spriteRenderer.sprite = TopRight;
                    break;
                case SpriteVariant.BottomLeft:
                    spriteRenderer.sprite = BottomLeft;
                    break;
                case SpriteVariant.BottomRight:
                    spriteRenderer.sprite = BottomRight;
                    break;
                default:
                    spriteRenderer.sprite = null;
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
        IWorldTile left = generator.GetTileAt(new Vector2Int(p.x - 1, p.y));
        IWorldTile right = generator.GetTileAt(new Vector2Int(p.x + 1, p.y));
        IWorldTile up = generator.GetTileAt(new Vector2Int(p.x, p.y + 1));
        IWorldTile down = generator.GetTileAt(new Vector2Int(p.x, p.y - 1));
        IWorldTile up_left = generator.GetTileAt(new Vector2Int(p.x - 1, p.y + 1));
        IWorldTile up_right = generator.GetTileAt(new Vector2Int(p.x + 1, p.y + 1));
        IWorldTile down_right = generator.GetTileAt(new Vector2Int(p.x + 1, p.y - 1));
        IWorldTile down_left = generator.GetTileAt(new Vector2Int(p.x - 1, p.y - 1));

        SetSprite(SpriteVariant.TopLeft);

        if (!up && down)
        {
            if (!left && right)
            {
                SetSprite(SpriteVariant.TopLeft);
                return;
            }
            if (!right && left)
            {
                SetSprite(SpriteVariant.TopRight);
                return;
            }
        }
        if (up && down)
        {
            if (!left && right)
            {
                SetSprite(SpriteVariant.BottomLeft);
                return;
            }
            if (!right && left)
            {
                SetSprite(SpriteVariant.BottomRight);
                return;
            }
        }
    }

    public void SetSprite(SpriteVariant type, float rotation = 0)
    {
        spriteType = type;
        UpdateRenderer();
    }
}
