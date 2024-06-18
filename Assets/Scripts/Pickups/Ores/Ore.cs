using UnityEngine;

public class Ore : MonoBehaviour, IPickup
{
    public string oreType;
    public int value;

    public virtual void OnPickup(PlayerController player)
    {
        player.PickUpObject(gameObject);
    }
}
