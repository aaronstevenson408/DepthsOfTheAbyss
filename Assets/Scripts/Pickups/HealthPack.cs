using UnityEngine;

public class HealthPack : MonoBehaviour, IPickup
{
    public int healAmount = 20;

    public void OnPickup(PlayerController player)
    {
        if (PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance.Heal(healAmount);
            Destroy(gameObject); // Destroy the health pack after it's picked up
        }
        else
        {
            Debug.LogError("PlayerHealth.Instance is null. Cannot heal the player.");
        }
    }
}
