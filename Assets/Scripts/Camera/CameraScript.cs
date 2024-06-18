using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset;

    void Start()
    {
        // You can find the player GameObject by its tag or by directly referencing it
        // Example 1: Find by tag
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Example 2: Direct reference if you already have a reference to the player GameObject
        // playerTransform = myPlayerGameObject.transform;

        // Example 3: If the player is always the same object, you can assign it directly in the Unity Editor
        // Assign the player GameObject to the "Player Transform" field in the Inspector
    }

    void Update()
    {
        // Check if the playerTransform is assigned
        if (playerTransform != null)
        {
            // Set the camera's position to the player's position with an offset
            transform.position = playerTransform.position + offset;
        }
    }
}
