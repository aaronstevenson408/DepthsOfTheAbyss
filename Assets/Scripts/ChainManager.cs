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
            GameObject chainLink = Instantiate(chainLinkPrefab, handAnchor.position, Quaternion.identity);
            chainLinks.Add(chainLink);

            Rigidbody2D rb = chainLink.GetComponent<Rigidbody2D>();
            rb.mass = 0.1f;
            rb.drag = 0.2f;
            rb.angularDrag = 0.2f;

            if (i == 0)
            {
                FixedJoint2D joint = chainLink.AddComponent<FixedJoint2D>();
                joint.connectedBody = handAnchor.GetComponent<Rigidbody2D>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = Vector2.zero;
            }
            else
            {
                DistanceJoint2D joint = chainLink.AddComponent<DistanceJoint2D>();
                joint.connectedBody = previousLink.GetComponent<Rigidbody2D>();
                joint.autoConfigureDistance = false;
                joint.distance = linkDistance;

                HingeJoint2D hingeJoint = chainLink.AddComponent<HingeJoint2D>();
                hingeJoint.connectedBody = previousLink.GetComponent<Rigidbody2D>();
                hingeJoint.autoConfigureConnectedAnchor = false;
                hingeJoint.anchor = new Vector2(0, -linkDistance / 2);
                hingeJoint.connectedAnchor = new Vector2(0, linkDistance / 2);

                hingeJoint.useLimits = true;
                JointAngleLimits2D limits = new JointAngleLimits2D();
                limits.min = -15;
                limits.max = 15;
                hingeJoint.limits = limits;
            }

            previousLink = chainLink;
        }

        // Attach lantern to the last chain link
        DistanceJoint2D lanternDistanceJoint = lantern.AddComponent<DistanceJoint2D>();
        lanternDistanceJoint.connectedBody = previousLink.GetComponent<Rigidbody2D>();
        lanternDistanceJoint.autoConfigureDistance = false;
        lanternDistanceJoint.distance = linkDistance;

        HingeJoint2D lanternHingeJoint = lantern.AddComponent<HingeJoint2D>();
        lanternHingeJoint.connectedBody = previousLink.GetComponent<Rigidbody2D>();
        lanternHingeJoint.autoConfigureConnectedAnchor = false;
        lanternHingeJoint.connectedAnchor = new Vector2(0, -linkDistance / 2);

        Rigidbody2D lanternRb = lantern.GetComponent<Rigidbody2D>();
        lanternRb.mass = 2f;
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
        // Simple chain stabilization
        for (int i = 1; i < chainLinks.Count; i++)
        {
            GameObject currentLink = chainLinks[i];
            GameObject previousLink = chainLinks[i - 1];

            Vector2 desiredPosition = (Vector2)previousLink.transform.position - (Vector2.up * linkDistance);
            currentLink.GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(currentLink.transform.position, desiredPosition, Time.deltaTime * 50f));
        }

        // Update lantern position
        if (chainLinks.Count > 0)
        {
            GameObject lastLink = chainLinks[chainLinks.Count - 1];
            Vector2 desiredPosition = (Vector2)lastLink.transform.position - (Vector2.up * linkDistance);
            lantern.GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(lantern.transform.position, desiredPosition, Time.deltaTime * 50f));
        }
    }
}
