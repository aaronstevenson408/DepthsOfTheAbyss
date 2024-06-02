using UnityEngine;
using System.Collections.Generic;

public class CavernGenerator : MonoBehaviour
{
    public GameObject groundTilePrefab;
    public GameObject wallTilePrefab;
    public GameObject ceilingTilePrefab;
    public int numberOfHallways = 5;
    public int hallwayLength = 10;
    public float tileSize = 1; // Assuming one Unity unit represents a 32x32 pixel tile

    void Start()
    {
        GenerateCavern();
    }

    void GenerateCavern()
    {
        for (int i = 0; i < numberOfHallways; i++)
        {
            List<Tile> tiles = GenerateHallwayTiles();
            CreateHallway(tiles);
        }
    }

    List<Tile> GenerateHallwayTiles()
    {
        List<Tile> tiles = new List<Tile>();

        for (int i = 0; i < hallwayLength; i++)
        {
            tiles.Add(new Tile(TileType.Ground, new Vector2Int(i, 0)));
            tiles.Add(new Tile(TileType.Wall, new Vector2Int(i, 1)));
            tiles.Add(new Tile(TileType.Wall, new Vector2Int(i, 2)));
            tiles.Add(new Tile(TileType.Ceiling, new Vector2Int(i, 3)));
        }

        return tiles;
    }

    Vector3 hallwayStartPosition = new Vector3(0, 0, 0); // Starting position for the first hallway

    void CreateHallway(List<Tile> tiles)
    {
        foreach (Tile tile in tiles)
        {
            // Calculate the position based on the tile's position in the hallway
            Vector3 position = new Vector3(tile.position.x * tileSize, tile.position.y * tileSize, 0);

            // Instantiate the prefab at the calculated position
            GameObject prefab = GetPrefabFromTileType(tile.type);
            Instantiate(prefab, position, Quaternion.identity);
        }
    }

    GameObject GetPrefabFromTileType(TileType type)
    {
        switch (type)
        {
            case TileType.Ground:
                return groundTilePrefab;
            case TileType.Wall:
                return wallTilePrefab;
            case TileType.Ceiling:
                return ceilingTilePrefab;
            default:
                return null;
        }
    }
}
