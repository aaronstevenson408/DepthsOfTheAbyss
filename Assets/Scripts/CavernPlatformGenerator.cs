using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlatformGenerator : MonoBehaviour
{
    public Tilemap tilemap; // Reference to the Tilemap
    public RuleTile platformTile; // Reference to your Rule Tile
    public int platformLength = 5; // Length of each platform
    public int platformHeight = 2; // Height of each platform
    public int numberOfPlatforms = 10; // Number of platforms to generate
    public Vector3Int startPosition = new Vector3Int(0, 0, 0); // Starting position for the first platform

    void Start()
    {
        GeneratePlatforms();
    }

    void GeneratePlatforms()
    {
        Vector3Int currentPos = startPosition;

        for (int i = 0; i < numberOfPlatforms; i++)
        {
            GeneratePlatform(currentPos);
            // Move downward to the next platform's starting position
            currentPos.y -= platformHeight + Random.Range(2, 5); // Random gap between platforms
            currentPos.x += Random.Range(-platformLength, platformLength); // Optional: add some horizontal variation
        }
    }

    void GeneratePlatform(Vector3Int startPos)
    {
        for (int x = 0; x < platformLength; x++)
        {
            tilemap.SetTile(startPos + new Vector3Int(x, 0, 0), platformTile);
        }
    }
}
