using UnityEngine;
using System.Collections;

public class LavaTile : IWorldTile
{
    public Sprite
        TopEdgeLeft,
        TopEdgeRight,
        EdgeLeft,
        EdgeRight,
        BottomEdgeLeft,
        BottomEdgeRight,
        Bottom,
        Center,
        Top;

    public enum SpriteVariant
    {
        TopEdgeLeft,
        TopEdgeRight,
        EdgeLeft,
        EdgeRight,
        BottomEdgeLeft,
        BottomEdgeRight,
        Bottom,
        Center,
        Top
    }

    SpriteRenderer spriteRenderer;
    SpriteVariant spriteType = SpriteVariant.Center;

    public override bool DoesConnect()
    {
        return true;
    }

    protected void UpdateRenderer()
    {
        if (spriteRenderer)
            switch (spriteType)
            {
                case SpriteVariant.TopEdgeLeft:
                    spriteRenderer.sprite = TopEdgeLeft;
                    break;
                case SpriteVariant.TopEdgeRight:
                    spriteRenderer.sprite = TopEdgeRight;
                    break;
                case SpriteVariant.EdgeLeft:
                    spriteRenderer.sprite = EdgeLeft;
                    break;
                case SpriteVariant.EdgeRight:
                    spriteRenderer.sprite = EdgeRight;
                    break;
                case SpriteVariant.BottomEdgeLeft:
                    spriteRenderer.sprite = BottomEdgeLeft;
                    break;
                case SpriteVariant.BottomEdgeRight:
                    spriteRenderer.sprite = BottomEdgeRight;
                    break;
                case SpriteVariant.Center:
                    spriteRenderer.sprite = Center;
                    break;
                case SpriteVariant.Top:
                    spriteRenderer.sprite = Top;
                    break;
                case SpriteVariant.Bottom:
                    spriteRenderer.sprite = Bottom;
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

        SetSprite(SpriteVariant.Center);

        if (left is WorldTile && right is LavaTile)
        {
            if (!up)
            {
                SetSprite(SpriteVariant.TopEdgeLeft);
                return;
            }
            if (down is LavaTile)
            {
                SetSprite(SpriteVariant.EdgeLeft);
                return;
            }
            if (down is WorldTile)
            {
                SetSprite(SpriteVariant.BottomEdgeLeft);
                return;
            }
        }

        if (right is WorldTile && left is LavaTile)
        {
            if (!up)
            {
                SetSprite(SpriteVariant.TopEdgeRight);
                return;
            }
            if (down is LavaTile)
            {
                SetSprite(SpriteVariant.EdgeRight);
                return;
            }
            if (down is WorldTile)
            {
                SetSprite(SpriteVariant.BottomEdgeRight);
                return;
            }
        }

        if (up is LavaTile && down is WorldTile)
        {
            SetSprite(SpriteVariant.Bottom);
            return;
        }

        if (down is LavaTile && !up)
        {
            SetSprite(SpriteVariant.Top);
            return;
        }

    }

    public void SetSprite(SpriteVariant type)
    {
        spriteType = type;
        UpdateRenderer();
    }
}
