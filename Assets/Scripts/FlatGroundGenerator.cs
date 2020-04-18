using UnityEngine;
using System.Collections;

public class FlatGroundGenerator : IGeneratorSpec
{
    public WorldTile dirtTile;

    public FlatGroundGenerator(WorldTile dirt)
    {
        dirtTile = dirt;
    }

    public WorldTile GenerateTileAt(int seed, Vector2Int position)
    {
        if (!(position.y > 5 && position.y < 10 && position.x > 5 && position.x < 10))
        {
            return dirtTile;
        }
        else
        {
            return null;
        }
    }
}
