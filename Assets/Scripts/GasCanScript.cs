using UnityEngine;

public class GasCanScript : MonoBehaviour
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
        // Find the player GameObject
        GameObject playerObj = other.gameObject.GetComponentInParent<PlayerController>().gameObject;

        if (playerObj != null)
        {
            // Find the LanternManager component on the player's child GameObject
            LanternManager lantern = playerObj.GetComponentInChildren<LanternManager>();

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
            else
            {
                Debug.LogWarning("Player GameObject doesn't have a LanternManager component on any of its children.");
            }
        }
        else
        {
            Debug.LogWarning("Collided with " + other.gameObject.name + " but it doesn't have a PlayerController component.");
        }
    }
}

