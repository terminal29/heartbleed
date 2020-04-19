using UnityEngine;
using System.Collections.Generic;

public class HoleGroundGenerator : IGeneratorSpec
{
    IWorldTile groundTile;
    List<Rect> holes;
    Vector2Int spawnPoint;

    public delegate IWorldTile CustomGenerator(int seed, Vector2Int position);
    List<CustomGenerator> customGenerators;

    public HoleGroundGenerator(IWorldTile groundTile, List<Rect> holes, Vector2Int spawnPoint, List<CustomGenerator> customGenerators)
    {
        this.groundTile = groundTile;
        this.holes = holes;
        this.spawnPoint = spawnPoint;
        this.customGenerators = customGenerators;
    }

    public IWorldTile GenerateTileAt(int seed, Vector2Int position)
    {
        foreach (CustomGenerator generator in customGenerators)
        {
            IWorldTile possibleTile = generator(seed, position);
            if (possibleTile != null)
                return possibleTile;
        }

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
