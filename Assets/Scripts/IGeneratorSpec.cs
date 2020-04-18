using UnityEngine;
using System.Collections;

public interface IGeneratorSpec
{
    IWorldTile GenerateTileAt(int seed, Vector2Int position);

    Vector2Int GetSpawn();
}
