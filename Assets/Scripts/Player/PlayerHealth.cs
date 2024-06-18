using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Singleton instance
    public static PlayerHealth Instance { get; private set; }

    // Health properties
    public int currentHealth;
    public int maxHealth = 100;

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Duplicate instance of PlayerHealth found. Destroying this instance.");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Logic for when the player dies
        Debug.Log("Player has died.");
    }

    public void Heal(int healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
    }
}
