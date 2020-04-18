using UnityEngine;
using System.Collections.Generic;

public class HoleGroundGenerator : IGeneratorSpec
{
    WorldTile groundTile;
    List<Rect> holes;
    Vector2Int spawnPoint;

    public HoleGroundGenerator(WorldTile groundTile, List<Rect> holes, Vector2Int spawnPoint)
    {
        this.groundTile = groundTile;
        this.holes = holes;
        this.spawnPoint = spawnPoint;
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

    public Vector2Int GetSpawn()
    {
        return spawnPoint;
    }
}
