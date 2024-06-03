using UnityEngine;

public class FuelSource : MonoBehaviour
{
    public float fuelAmount = 20f;
    // public AudioClip pickupSound; // Public variable for the pickup sound
    // private AudioSource audioSource; // AudioSource component

    void Start()
    {
        // audioSource = GetComponent<AudioSource>();

        // if (audioSource == null)
        // {
        //     Debug.LogError("AudioSource component is missing from the FuelSource GameObject.");
        // }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        LanternManager lantern = other.GetComponent<LanternManager>();
        if (lantern != null)
        {
            lantern.ReplenishFuel(fuelAmount);

            // // Play the pickup sound
            // if (pickupSound != null && audioSource != null)
            // {
            //     audioSource.PlayOneShot(pickupSound);
            // }

            Destroy(gameObject); // Destroy the fuel pickup after use
        }
    }
}
