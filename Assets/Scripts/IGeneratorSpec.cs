using UnityEngine;
using System.Collections;

public interface IGeneratorSpec
{
    WorldTile GenerateTileAt(int seed, Vector2Int position);
}
