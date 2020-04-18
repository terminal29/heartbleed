using System.Collections.Generic;
using UnityEngine;
public interface WorldGenerator
{
    /// <summary>
    /// Generates the world and stores it in an internal array
    /// </summary>
    /// <param name="seed">World seed</param>
    void Generate(int seed);

    /// <summary>
    /// Gets the size of the world
    /// </summary>
    /// <returns></returns>
    Vector2Int GetWorldSize();

    /// <summary>
    /// Gets all of the world tiles
    /// </summary>
    /// <returns></returns>
    Dictionary<Vector2Int, WorldTile> GetWorldTiles();

    /// <summary>
    /// Generates a tile at that position using the internal generator, removing any previous tile that was there
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    void GenerateTileAt(Vector2Int position);

    /// <summary>
    /// Destroys & cleans up a tile at that position
    /// </summary>
    /// <param name="position"></param>
    void DestroyTileAt(Vector2Int position);

    /// <summary>
    /// Gets a tile at a specific index
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    WorldTile GetTileAt(Vector2Int position);

    /// <summary>
    /// Sets the tile at a specific index
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void SetTileAt(WorldTile tile, Vector2Int position);
}