using UnityEngine;
using System.Collections.Generic;

// This class manages a chain of links between a player and a lantern
public class ChainManager : MonoBehaviour
{
    public GameObject chainLinkPrefab; // Prefab for the chain link
    public Transform playerAnchor; // Transform of the player's anchor point
    public Transform lanternAnchor; // Transform of the lantern's anchor point
    public int initialLinkCount = 5; // Initial number of chain links
    public LayerMask platformLayer; // Layer mask for platforms (currently unused)
    public float linkSpacing = 0.1f; // Spacing between chain links

    private List<GameObject> chainLinks = new List<GameObject>(); // List to store chain link objects

    void Start()
    {
        InitializeChain(); // Initialize the chain when the script starts
    }

    /// <summary>
    /// Initializes the chain by creating and connecting chain links between the player and the lantern.
    /// </summary>
    void InitializeChain()
    {
        Vector2 currentPosition = playerAnchor.position; // Start position at the player's anchor

        GameObject firstLink = Instantiate(chainLinkPrefab, currentPosition, Quaternion.identity); // Create the first chain link
        firstLink.GetComponent<Rigidbody2D>().isKinematic = true; // Make the first link kinematic initially
        chainLinks.Add(firstLink); // Add the first link to the list

        HingeJoint2D playerJoint = playerAnchor.GetComponent<HingeJoint2D>(); // Get the player's hinge joint
        playerJoint.connectedBody = firstLink.GetComponent<Rigidbody2D>(); // Connect the player to the first link
        SetupHingeJoint(playerJoint, firstLink.transform); // Set up the hinge joint properties

        for (int i = 1; i < initialLinkCount; i++) // Create the rest of the initial chain links
        {
            GameObject previousLink = chainLinks[i - 1]; // Get the previous link
            currentPosition = GetNextLinkPosition(previousLink); // Calculate the position for the new link

            GameObject newLink = Instantiate(chainLinkPrefab, currentPosition, Quaternion.identity); // Create a new link
            chainLinks.Add(newLink); // Add the new link to the list

            HingeJoint2D previousJoint = previousLink.GetComponent<HingeJoint2D>(); // Get the previous link's hinge joint
            previousJoint.connectedBody = newLink.GetComponent<Rigidbody2D>(); // Connect the previous link to the new link
            SetupHingeJoint(previousJoint, newLink.transform); // Set up the hinge joint properties
        }

        HingeJoint2D lanternJoint = lanternAnchor.GetComponent<HingeJoint2D>(); // Get the lantern's hinge joint
        lanternJoint.connectedBody = chainLinks[chainLinks.Count - 1].GetComponent<Rigidbody2D>(); // Connect the lantern to the last link
        SetupHingeJoint(lanternJoint, chainLinks[chainLinks.Count - 1].transform); // Set up the hinge joint properties

        // chainLinks[0].GetComponent<Rigidbody2D>().isKinematic = false; // Make the first link non-kinematic after setup
    }

    /// <summary>
    /// Calculates the position for the next chain link based on the current link's position and joint.
    /// </summary>
    /// <param name="currentLink">The current chain link GameObject.</param>
    /// <returns>The position for the next chain link.</returns>
    Vector2 GetNextLinkPosition(GameObject currentLink)
    {
        HingeJoint2D currentJoint = currentLink.GetComponent<HingeJoint2D>(); // Get the current link's hinge joint
        Vector2 currentJointWorldPosition = currentLink.transform.TransformPoint(currentJoint.anchor); // Get the world position of the joint
        return currentJointWorldPosition - new Vector2(0, linkSpacing); // Return the position below the current joint
    }

    /// <summary>
    /// Sets up the properties of a hinge joint, including limits and connected anchor.
    /// </summary>
    /// <param name="joint">The HingeJoint2D to set up.</param>
    /// <param name="connectedTransform">The Transform of the connected object.</param>
    void SetupHingeJoint(HingeJoint2D joint, Transform connectedTransform)
    {
        joint.useLimits = true; // Enable joint angle limits
        // joint.limits = new JointAngleLimits2D { min = -5, max = 5 }; // Set the joint angle limits/
        joint.connectedAnchor = connectedTransform.InverseTransformPoint(connectedTransform.TransformPoint(connectedTransform.GetComponent<HingeJoint2D>().anchor)); // Set the connected anchor in local space
    }

    /// <summary>
    /// Adds a new link to the end of the chain.
    /// </summary>
    public void AddLink()
    {
        GameObject lastLink = chainLinks[chainLinks.Count - 1]; // Get the last link in the chain
        Vector2 newPosition = GetNextLinkPosition(lastLink); // Calculate the position for the new link

        GameObject newLink = Instantiate(chainLinkPrefab, newPosition, Quaternion.identity); // Create a new link
        chainLinks.Add(newLink); // Add the new link to the list

        HingeJoint2D lastJoint = lastLink.GetComponent<HingeJoint2D>(); // Get the last link's hinge joint
        lastJoint.connectedBody = newLink.GetComponent<Rigidbody2D>(); // Connect the last link to the new link
        SetupHingeJoint(lastJoint, newLink.transform); // Set up the hinge joint properties

        HingeJoint2D lanternJoint = lanternAnchor.GetComponent<HingeJoint2D>(); // Get the lantern's hinge joint
        lanternJoint.connectedBody = newLink.GetComponent<Rigidbody2D>(); // Connect the lantern to the new link
        SetupHingeJoint(lanternJoint, newLink.transform); // Set up the hinge joint properties
    }

    /// <summary>
    /// Removes the last link from the chain.
    /// </summary>
    public void RemoveLink()
    {
        if (chainLinks.Count > 1) // Only remove if there's more than one link
        {
            GameObject lastLink = chainLinks[chainLinks.Count - 1]; // Get the last link
            chainLinks.RemoveAt(chainLinks.Count - 1); // Remove the last link from the list
            Destroy(lastLink); // Destroy the last link GameObject

            GameObject newLastLink = chainLinks[chainLinks.Count - 1]; // Get the new last link
            HingeJoint2D lanternJoint = lanternAnchor.GetComponent<HingeJoint2D>(); // Get the lantern's hinge joint
            lanternJoint.connectedBody = newLastLink.GetComponent<Rigidbody2D>(); // Connect the lantern to the new last link
            SetupHingeJoint(lanternJoint, newLastLink.transform); // Set up the hinge joint properties
        }
    }

    /// <summary>
    /// Draws gizmos in the Unity editor to visualize the chain links and anchor points.
    /// </summary>
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return; // Only draw gizmos when the application is playing

        Gizmos.color = Color.red; // Set gizmo color to red
        Gizmos.DrawSphere(playerAnchor.position, 0.1f); // Draw a sphere at the player anchor
        Gizmos.DrawSphere(lanternAnchor.position, 0.1f); // Draw a sphere at the lantern anchor

        foreach (var link in chainLinks) // Iterate through all chain links
        {
            if (link != null) // Check if the link still exists
            {
                Gizmos.DrawSphere(link.transform.position, 0.05f); // Draw a small sphere at each link's position
            }
        }
    }
}