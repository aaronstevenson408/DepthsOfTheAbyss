// PlayerInventory.cs
using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    private Dictionary<string, int> ores = new Dictionary<string, int>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddOre(string oreType, int value)
    {
        if (ores.ContainsKey(oreType))
        {
            ores[oreType] += value;
        }
        else
        {
            ores.Add(oreType, value);
        }
        Debug.Log($"Added {value} of {oreType}. Total: {ores[oreType]}");
    }

    public int GetOreCount(string oreType)
    {
        return ores.ContainsKey(oreType) ? ores[oreType] : 0;
    }
}
