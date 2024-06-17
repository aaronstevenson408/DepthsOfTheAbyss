using UnityEngine;
using System.Collections.Generic;

public class ChainManager : MonoBehaviour
{
    public GameObject chainLinkPrefab; // Prefab for creating individual chain links
    public Transform handAnchor; // Player's anchor point for chain attachment
    public Transform lanternAnchor; // Lantern's anchor point for chain attachment
    public int initialLinkCount = 10; // Number of chain links to create on initialization
    public float linkSpacing = 0.5f; // Vertical distance between each chain link when positioned

    private List<GameObject> chainLinks = new List<GameObject>(); // List to track all chain link GameObjects

    void Start()
    {
        InitializeChain(); // Set up the initial chain when the game starts
    }

    void InitializeChain()
    {
        Vector2 currentPosition = handAnchor.position; // Start the chain at the hand's anchor position

        // Create and connect each chain link
        for (int i = 0; i < initialLinkCount; i++)
        {
            GameObject newLink = Instantiate(chainLinkPrefab, currentPosition, Quaternion.identity); // Create a new chain link
            newLink.name = $"Chain Link {i}"; // Name the new link with its index
            chainLinks.Add(newLink); // Add the new chain link to the list

            if (i == 0) // Connect first link to handAnchor
            {
                HingeJoint2D handJoint = handAnchor.gameObject.AddComponent<HingeJoint2D>(); // Add HingeJoint2D to hand anchor
                handJoint.connectedBody = newLink.GetComponent<Rigidbody2D>(); // Connect hand anchor to the first link
                SetupHingeJoint(handJoint, handAnchor.position); // Configure hand joint properties
            }
            else // Connect each link to the previous one
            {
                GameObject previousLink = chainLinks[i - 1]; // Get the previously created chain link
                HingeJoint2D newJoint = newLink.AddComponent<HingeJoint2D>(); // Add HingeJoint2D to the new link
                newJoint.connectedBody = previousLink.GetComponent<Rigidbody2D>(); // Connect new link to the previous link
                SetupHingeJoint(newJoint, previousLink.transform.position); // Configure new link's hinge joint properties
            }

            currentPosition = new Vector2(currentPosition.x, currentPosition.y - linkSpacing); // Update the position for the next link
        }

        // Connect the last link to the lanternAnchor
        GameObject lastLink = chainLinks[chainLinks.Count - 1];
        HingeJoint2D lanternJoint = lanternAnchor.gameObject.AddComponent<HingeJoint2D>();
        lanternJoint.connectedBody = lastLink.GetComponent<Rigidbody2D>();
        SetupHingeJoint(lanternJoint, lanternAnchor.position);
    }

    void SetupHingeJoint(HingeJoint2D joint, Vector2 anchorPosition)
    {
        joint.useLimits = true; // Enable angle limits on the joint
        joint.limits = new JointAngleLimits2D { min = -5, max = 5 }; // Set min and max rotation angles
        joint.anchor = joint.transform.InverseTransformPoint(anchorPosition); // Set the anchor to the correct position
        joint.connectedAnchor = Vector2.zero; // Default to center of connected body
    }
}
