using UnityEngine;

public class LightManager : MonoBehaviour
{
    void Start()
    {
        // Disable all other lights in the scene
        Light[] lights = FindObjectsOfType<Light>();
        foreach (Light light in lights)
        {
            if (light.gameObject.name != "YourLanternPrefabName")
            {
                light.enabled = false;
            }
        }
    }
}