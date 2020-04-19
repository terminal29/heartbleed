using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGenerator : MonoBehaviour, WorldGenerator
{

    public float tileSize = 1f;
    const int width = 64;
    const int height = 256;

    bool hasGenerated = false;
    private Dictionary<Vector2Int, IWorldTile> tiles = new Dictionary<Vector2Int, IWorldTile>();

    void Start()
    {
    }

    private void OnDestroy()
    {
        Clear();
    }

    public Vector2 GetRealPositionFor(Vector2Int pos)
    {
        return new Vector3(pos.x * tileSize, pos.y * tileSize, 0);
    }

    public void Clear()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                DestroyTileAt(new Vector2Int(x, y));
            }
        }
        // Clear the dict
        tiles.Clear();
    }

    public void Generate(int seed, IGeneratorSpec generatorSpec)
    {
        if (hasGenerated)
        {
            // Clear old gameobjects (if they exist)
            Clear();
        }

        // Generate new ones
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GenerateTileAt(new Vector2Int(x, y), generatorSpec);
            }
        }
        hasGenerated = true;

        // Fix up variants
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GetTileAt(new Vector2Int(x, y))?.ResolveSpriteVariant(new Vector2Int(x, y), this);
            }
        }
    }

    public IWorldTile GetTileAt(Vector2Int pos)
    {
        if (tiles.ContainsKey(pos))
            return tiles[pos];
        return null;
    }

    public Dictionary<Vector2Int, IWorldTile> GetWorldTiles()
    {
        return tiles;
    }

    public void SetTileAt(IWorldTile tile, Vector2Int position)
    {
        if (tiles.ContainsKey(position))
            DestroyTileAt(position);
        if (tile)
            tiles[position] = Instantiate(tile, GetRealPositionFor(position), Quaternion.identity, transform);
    }

    public Vector2Int GetWorldSize()
    {
        return new Vector2Int(width, height);
    }

    public void GenerateTileAt(Vector2Int position, IGeneratorSpec generatorSpec)
    {
        if (GetTileAt(position))
        {
            DestroyTileAt(position);
        }
        IWorldTile prefab = generatorSpec.GenerateTileAt(0, position);
        if (prefab)
            tiles[position] = Instantiate(prefab, GetRealPositionFor(position), Quaternion.identity, transform);
    }

    public void DestroyTileAt(Vector2Int position)
    {
        if (tiles.ContainsKey(position))
        {
            Destroy(tiles[position]);
            tiles.Remove(position);
        }
    }
}
