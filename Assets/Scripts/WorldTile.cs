using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WorldTile : IWorldTile
{
    public Sprite Center, OuterCorner, InnerCorner, Edge;
    public enum SpriteVariant
    {
        Center,
        OuterCorner,
        InnerCorner,
        Edge
    }

    SpriteRenderer spriteRenderer;
    SpriteVariant spriteType = SpriteVariant.Center;
    float spriteRotation = 0;

    protected virtual void UpdateRenderer()
    {
        if (spriteRenderer)
            switch (spriteType)
            {
                case SpriteVariant.Center:
                    spriteRenderer.sprite = Center;
                    break;
                case SpriteVariant.Edge:
                    spriteRenderer.sprite = Edge;
                    break;
                case SpriteVariant.InnerCorner:
                    spriteRenderer.sprite = InnerCorner;
                    break;
                case SpriteVariant.OuterCorner:
                    spriteRenderer.sprite = OuterCorner;
                    break;
                default:
                    spriteRenderer.sprite = null;
                    break;
            }

        Vector3 euler = transform.localRotation.eulerAngles;
        euler.z = spriteRotation;
        transform.localRotation = Quaternion.Euler(euler);
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateRenderer();
    }

    public override bool DoesCollide()
    {
        return true;
    }

    protected IWorldTile DoesCollideOrNull(IWorldTile tile)
    {
        return tile ? (tile.DoesCollide() ? tile : null) : null;
    }

    public override void ResolveSpriteVariant(Vector2Int p, WorldGenerator generator)
    {
        IWorldTile left = DoesCollideOrNull(generator.GetTileAt(new Vector2Int(p.x - 1, p.y)));
        IWorldTile right = DoesCollideOrNull(generator.GetTileAt(new Vector2Int(p.x + 1, p.y)));
        IWorldTile up = DoesCollideOrNull(generator.GetTileAt(new Vector2Int(p.x, p.y + 1)));
        IWorldTile down = DoesCollideOrNull(generator.GetTileAt(new Vector2Int(p.x, p.y - 1)));
        IWorldTile up_left = DoesCollideOrNull(generator.GetTileAt(new Vector2Int(p.x - 1, p.y + 1)));
        IWorldTile up_right = DoesCollideOrNull(generator.GetTileAt(new Vector2Int(p.x + 1, p.y + 1)));
        IWorldTile down_right = DoesCollideOrNull(generator.GetTileAt(new Vector2Int(p.x + 1, p.y - 1)));
        IWorldTile down_left = DoesCollideOrNull(generator.GetTileAt(new Vector2Int(p.x - 1, p.y - 1)));

        SetSprite(SpriteVariant.Center, 0);

        if (left && right)
        {
            if (up && !down)
            {
                SetSprite(SpriteVariant.Edge, 180);
                return;
            }
            else if (!up && down)
            {
                SetSprite(SpriteVariant.Edge, 0);
                return;
            }
        }
        else if (up && down)
        {
            if (left && !right)
            {
                SetSprite(SpriteVariant.Edge, 270);
                return;
            }
            else if (!left && right)
            {
                SetSprite(SpriteVariant.Edge, 90);
                return;
            }
        }
        else if (up)
        {
            if (left)
            {
                SetSprite(SpriteVariant.OuterCorner, 180);
                return;
            }
            else if (right)
            {
                SetSprite(SpriteVariant.OuterCorner, 90);
                return;
            }
        }
        else if (down)
        {
            if (left)
            {
                SetSprite(SpriteVariant.OuterCorner, 270);
                return;
            }
            else if (right)
            {
                SetSprite(SpriteVariant.OuterCorner, 0);
                return;
            }
        }

        if (!up_left)
        {
            SetSprite(SpriteVariant.InnerCorner, 0);
            return;
        }
        else if (!up_right)
        {
            SetSprite(SpriteVariant.InnerCorner, 270);
            return;
        }
        else if (!down_right)
        {
            SetSprite(SpriteVariant.InnerCorner, 180);
            return;
        }
        else if (!down_left)
        {
            SetSprite(SpriteVariant.InnerCorner, 90);
            return;
        }


    }

    public void SetSprite(SpriteVariant type, float rotation)
    {
        spriteType = type;
        spriteRotation = rotation;
        UpdateRenderer();
    }
}
