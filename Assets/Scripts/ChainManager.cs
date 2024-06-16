using UnityEngine;
using System.Collections.Generic;

// This class manages a chain of links between a player and a lantern in a 2D Unity game.
// It handles the creation, initialization, and connection of chain links using physics joints.
public class ChainManager : MonoBehaviour
{
    public GameObject chainLinkPrefab; // Prefab for creating individual chain links
    public Transform playerAnchor; // Player's anchor point for chain attachment
    public Transform lanternAnchor; // Lantern's anchor point for chain attachment
    public int initialLinkCount = 5; // Number of chain links to create on initialization
    public float linkSpacing = 0.01f; // Vertical distance between each chain link when positioned

    private List<GameObject> chainLinks = new List<GameObject>(); // List to track all chain link GameObjects

    // Called when the script instance is being loaded.
    void Start()
    {
        InitializeChain(); // Set up the initial chain when the game starts
    }

    // Sets up the initial chain by creating chain links and connecting them between the player and the lantern.
    // This method handles the instantiation, positioning, and joint setup for each link in the chain.
    void InitializeChain()
    {
        Vector2 currentPosition = playerAnchor.position; // Start the chain at the player's anchor position

        GameObject firstLink = Instantiate(chainLinkPrefab, currentPosition, Quaternion.identity); // Create the first chain link
        firstLink.name = "Chain Link 0 (Player)"; // Name the first link
        chainLinks.Add(firstLink); // Add the first chain link to the list

        HingeJoint2D playerJoint = playerAnchor.GetComponent<HingeJoint2D>(); // Get the player's HingeJoint2D component
        playerJoint.connectedBody = firstLink.GetComponent<Rigidbody2D>(); // Connect player's joint to the first link
        SetupHingeJoint(playerJoint, firstLink.transform); // Configure player's hinge joint properties

        // Loop to create the intermediate chain links
        for (int i = 1; i < initialLinkCount - 1; i++) // Note the change here: initialLinkCount - 1
        {
            GameObject previousLink = chainLinks[i - 1]; // Get the previously created chain link
            currentPosition = GetNextLinkPosition(previousLink); // Calculate position for the new link

            GameObject newLink = Instantiate(chainLinkPrefab, currentPosition, Quaternion.identity); // Create a new chain link
            newLink.name = $"Chain Link {i}"; // Name the new link with its index
            chainLinks.Add(newLink); // Add the new chain link to the list

            HingeJoint2D previousJoint = previousLink.GetComponent<HingeJoint2D>(); // Get the previous link's joint
            previousJoint.connectedBody = newLink.GetComponent<Rigidbody2D>(); // Connect previous link to the new link
            SetupHingeJoint(previousJoint, newLink.transform); // Configure previous link's hinge joint properties
        }

        // Create the last link (connected to the lantern) separately
        GameObject lastLink = Instantiate(chainLinkPrefab, lanternAnchor.position, Quaternion.identity);
        lastLink.name = $"Chain Link {initialLinkCount - 1} (Lantern)";
        chainLinks.Add(lastLink);

        // Connect the second to last link to the last link
        HingeJoint2D secondToLastJoint = chainLinks[chainLinks.Count - 2].GetComponent<HingeJoint2D>();
        secondToLastJoint.connectedBody = lastLink.GetComponent<Rigidbody2D>();
        SetupHingeJoint(secondToLastJoint, lastLink.transform);

        // Connect the lantern to the last link
        HingeJoint2D lanternJoint = lanternAnchor.GetComponent<HingeJoint2D>();
        lanternJoint.connectedBody = lastLink.GetComponent<Rigidbody2D>();
        SetupHingeJoint(lanternJoint, lastLink.transform);
    }
    // Determines the position for the next chain link based on the current link's position and joint configuration.
    // This ensures that each new link is properly spaced and aligned with the previous link.
    Vector2 GetNextLinkPosition(GameObject currentLink)
    {
        HingeJoint2D currentJoint = currentLink.GetComponent<HingeJoint2D>(); // Get current link's HingeJoint2D
        Vector2 currentJointWorldPosition = currentLink.transform.TransformPoint(currentJoint.anchor); // Convert joint's local anchor to world position
        return currentJointWorldPosition - new Vector2(0, linkSpacing); // Calculate new position below the current joint
    }

    // Configures a HingeJoint2D with specific properties to create a limited and stable connection between two chain links.
    // This method sets up angle limits and properly aligns the connected anchor point.
    void SetupHingeJoint(HingeJoint2D joint, Transform connectedTransform)
    {
        joint.useLimits = true; // Enable angle limits on the joint
        joint.limits = new JointAngleLimits2D { min = -5, max = 5 }; // Set min and max rotation angles

        // Calculate and set the connected anchor point in the local space of the connected object
        joint.connectedAnchor = connectedTransform.InverseTransformPoint(
            connectedTransform.TransformPoint(
                connectedTransform.GetComponent<HingeJoint2D>().anchor
            )
        );
    }
}