using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        IPickup pickup = other.GetComponent<IPickup>();
        if (pickup != null)
        {
            // Assuming you have a reference to PlayerController somewhere
            PlayerController player = GetComponent<PlayerController>();
            if (player != null)
            {
                pickup.OnPickup(player);
            }
            else
            {
                Debug.LogError("PlayerController component not found on player.");
            }
        }
    }
}