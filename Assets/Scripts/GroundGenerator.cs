using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGenerator : MonoBehaviour, WorldGenerator
{
    public WorldTile dirt;
    public WorldTile stone;
    public PlayerController player;

    private IGeneratorSpec generatorSpec;

    public float tileSize = 1f;
    const int width = 64;
    const int height = 256;

    bool hasGenerated = false;
    private Dictionary<Vector2Int, WorldTile> tiles = new Dictionary<Vector2Int, WorldTile>();

    void Start()
    {
        generatorSpec = new HoleGroundGenerator(stone, new List<Rect>{
           new Rect(16, height-32, 32, 32),
           new Rect(15, height-31, 34, 32),
           new Rect(14, height-30, 36, 32)
        }, new Vector2Int(18, height - 29));
        Generate(0);
        player.Spawn(generatorSpec.GetSpawn());
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

    public void Generate(int seed)
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
                GenerateTileAt(new Vector2Int(x, y));
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

    public WorldTile GetTileAt(Vector2Int pos)
    {
        if (tiles.ContainsKey(pos))
            return tiles[pos];
        return null;
    }

    public Dictionary<Vector2Int, WorldTile> GetWorldTiles()
    {
        return tiles;
    }

    public void SetTileAt(WorldTile tile, Vector2Int position)
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

    public void GenerateTileAt(Vector2Int position)
    {
        if (GetTileAt(position))
        {
            DestroyTileAt(position);
        }
        WorldTile prefab = generatorSpec.GenerateTileAt(0, position);
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
