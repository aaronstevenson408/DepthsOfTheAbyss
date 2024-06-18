using UnityEngine;

public class GasCan : MonoBehaviour, IPickup, IInteractable
{
    public int fuelAmount = 50;

    public void OnPickup(PlayerController player)
    {
        player.PickUpObject(gameObject);
    }

    public void Interact(GameObject item)
    {
        LanternManager lantern = item.GetComponent<LanternManager>();
        if (lantern != null)
        {
            lantern.ReplenishFuel(fuelAmount);
            Destroy(gameObject);
        }
    }
}
