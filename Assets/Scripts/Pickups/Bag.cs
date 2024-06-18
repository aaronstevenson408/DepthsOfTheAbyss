using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour, IPickup, IInteractable
{
    public int capacity = 10;
    private List<string> items = new List<string>();

    public void OnPickup(PlayerController player)
    {
        player.PickUpObject(gameObject);
    }

    public void Interact(GameObject item)
    {
        Ore ore = item.GetComponent<Ore>();
        if (ore != null)
        {
            AddItem(ore.oreType);
            Destroy(item);
        }
    }

    private void AddItem(string item)
    {
        if (items.Count < capacity)
        {
            items.Add(item);
        }
        else
        {
            Debug.Log("Bag is full");
        }
    }
}
