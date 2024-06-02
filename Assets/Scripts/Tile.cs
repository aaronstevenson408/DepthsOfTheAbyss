// Tile.cs

using UnityEngine;

[System.Serializable]
public class Tile
{
    public TileType type;
    public Vector2Int position;

    public Tile(TileType type, Vector2Int position)
    {
        this.type = type;
        this.position = position;
    }
}
