using UnityEngine;
using System.Collections.Generic;

public class HoleGroundGenerator : IGeneratorSpec
{
    public WorldTile groundTile;

    List<Rect> holes;
    public HoleGroundGenerator(WorldTile groundTile, List<Rect> holes)
    {
        this.groundTile = groundTile;
        this.holes = holes;
    }

    public WorldTile GenerateTileAt(int seed, Vector2Int position)
    {

        foreach (Rect hole in holes)
        {
            if (hole.Contains(position))
            {
                return null;
            }
        }
        return groundTile;
    }
}
