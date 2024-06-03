using UnityEngine;
using System.Collections.Generic;

public class CavernGenerator : MonoBehaviour
{
    public GameObject groundTilePrefab;
    public GameObject wallTilePrefab;
    public GameObject ceilingTilePrefab;
    public GameObject fuelSourcePrefab; // Add reference to the FuelSource prefab
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
            PlaceFuelSources(tiles); // Place fuel sources in the generated hallway
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

    void PlaceFuelSources(List<Tile> tiles)
    {
        int fuelSourceCount = Random.Range(1, 4); // Randomly decide how many fuel sources to place
        Debug.Log($"Placing {fuelSourceCount} fuel sources.");

        for (int i = 0; i < fuelSourceCount; i++)
        {
            // Find a random ground tile
            Tile groundTile = null;
            while (groundTile == null || groundTile.type != TileType.Ground)
            {
                groundTile = tiles[Random.Range(0, tiles.Count)];
            }

            // Adjust the position to be one tile higher
            Vector3 position = new Vector3(groundTile.position.x * tileSize, (groundTile.position.y + 1) * tileSize, 0);
            Debug.Log($"Placing fuel source at position: {position}");
            Instantiate(fuelSourcePrefab, position, Quaternion.identity);
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