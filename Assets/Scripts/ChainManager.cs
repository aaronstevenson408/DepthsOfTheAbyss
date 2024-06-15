using UnityEngine;
using System.Collections.Generic;

public class ChainManager : MonoBehaviour
{
    public GameObject character;
    public GameObject lantern;
    public GameObject chainLinkPrefab;
    public Transform handAnchor;
    public int initialChainLength = 5;
    public int maxChainLength = 10;
    public float linkDistance = 0.2f;
    private List<GameObject> chainLinks = new List<GameObject>();

    void Start()
    {
        CreateChain(initialChainLength);
    }

    public void CreateChain(int length)
    {
        ClearChain();

        GameObject previousLink = null;
        for (int i = 0; i < length; i++)
        {
            // Add some randomness to the initial position
            Vector2 randomOffset = Random.insideUnitCircle * 0.1f;
            GameObject chainLink = Instantiate(chainLinkPrefab, (Vector2)handAnchor.position + randomOffset, Quaternion.identity);
            chainLinks.Add(chainLink);

            Rigidbody2D rb = chainLink.GetComponent<Rigidbody2D>();
            rb.mass = 0.1f;
            rb.drag = 0.2f;
            rb.angularDrag = 0.2f;

            if (i == 0)
            {
                SpringJoint2D joint = chainLink.AddComponent<SpringJoint2D>();
                joint.connectedBody = handAnchor.GetComponent<Rigidbody2D>();
                joint.autoConfigureDistance = false;
                joint.distance = 0.1f;
                joint.dampingRatio = 0.8f;
                joint.frequency = 2f;
            }
            else
            {
                SpringJoint2D joint = chainLink.AddComponent<SpringJoint2D>();
                joint.connectedBody = previousLink.GetComponent<Rigidbody2D>();
                joint.autoConfigureDistance = false;
                joint.distance = linkDistance;
                joint.dampingRatio = 0.8f;
                joint.frequency = 2f;

                HingeJoint2D hingeJoint = chainLink.AddComponent<HingeJoint2D>();
                hingeJoint.connectedBody = previousLink.GetComponent<Rigidbody2D>();
                hingeJoint.useLimits = true;
                JointAngleLimits2D limits = new JointAngleLimits2D();
                limits.min = -60;
                limits.max = 60;
                hingeJoint.limits = limits;
            }

            previousLink = chainLink;
        }

        // Attach lantern to the last chain link
        SpringJoint2D lanternJoint = lantern.AddComponent<SpringJoint2D>();
        lanternJoint.connectedBody = previousLink.GetComponent<Rigidbody2D>();
        lanternJoint.autoConfigureDistance = false;
        lanternJoint.distance = linkDistance;
        lanternJoint.dampingRatio = 0.8f;
        lanternJoint.frequency = 2f;

        Rigidbody2D lanternRb = lantern.GetComponent<Rigidbody2D>();
        lanternRb.mass = 1f;
        lanternRb.drag = 0.5f;
        lanternRb.angularDrag = 0.5f;
    }

    public void ClearChain()
    {
        foreach (GameObject link in chainLinks)
        {
            Destroy(link);
        }
        chainLinks.Clear();

        Destroy(lantern.GetComponent<DistanceJoint2D>());
        Destroy(lantern.GetComponent<HingeJoint2D>());
    }

    public void SetChainLength(int newLength)
    {
        if (newLength > maxChainLength) newLength = maxChainLength;
        CreateChain(newLength);
    }
    public int GetCurrentChainLength()
    {
        return chainLinks.Count;
    }

    void LateUpdate()
    {
        // // Simple chain stabilization
        // for (int i = 1; i < chainLinks.Count; i++)
        // {
        //     GameObject currentLink = chainLinks[i];
        //     GameObject previousLink = chainLinks[i - 1];

        //     Vector2 desiredPosition = (Vector2)previousLink.transform.position - (Vector2.up * linkDistance);
        //     currentLink.GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(currentLink.transform.position, desiredPosition, Time.deltaTime * 50f));
        // }

        // Update lantern position
        if (chainLinks.Count > 0)
        {
            GameObject lastLink = chainLinks[chainLinks.Count - 1];
            Vector2 desiredPosition = (Vector2)lastLink.transform.position - (Vector2.up * linkDistance);
            lantern.GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(lantern.transform.position, desiredPosition, Time.deltaTime * 50f));
        }
    }
}
