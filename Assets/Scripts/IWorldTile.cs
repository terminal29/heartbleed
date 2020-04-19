using UnityEngine;
using System.Collections;

public abstract class IWorldTile : MonoBehaviour
{
    public abstract void ResolveSpriteVariant(Vector2Int p, WorldGenerator generator);

    public abstract bool DoesConnect();
}
