using UnityEngine;

public class LanternManager : MonoBehaviour, IPickup, IInteractable
{
    public float maxFuel = 100f;
    public float fuelConsumptionRate = 1f;
    public float maxLightRadius = 5f;
    public UnityEngine.Rendering.Universal.Light2D lanternLight;
    [SerializeField] private float fuel;

    void Start()
    {
        fuel = maxFuel;
        lanternLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        if (lanternLight == null)
        {
            Debug.LogError("Light2D component is missing from the Lantern GameObject.");
        }
    }

    void Update()
    {
        if (fuel > 0)
        {
            fuel -= fuelConsumptionRate * Time.deltaTime;
            float radius = Mathf.Lerp(0, maxLightRadius, fuel / maxFuel);
            lanternLight.pointLightOuterRadius = radius;

            if (fuel < maxFuel * 0.2f)
            {
                lanternLight.intensity = Mathf.Lerp(0.5f, 1f, Mathf.PingPong(Time.time * 3, 1));
            }
        }
        else
        {
            lanternLight.enabled = false;
        }
    }

    public void ReplenishFuel(float amount)
    {
        fuel = Mathf.Clamp(fuel + amount, 0, maxFuel);
        if (fuel > 0 && !lanternLight.enabled)
        {
            lanternLight.enabled = true;
        }
    }

    public void OnPickup(PlayerController player)
    {
        player.PickUpObject(gameObject);
    }

    public void Interact(GameObject item)
    {
        GasCan gasCan = item.GetComponent<GasCan>();
        if (gasCan != null)
        {
            ReplenishFuel(gasCan.fuelAmount);
            Destroy(gasCan.gameObject);
        }
    }
}
