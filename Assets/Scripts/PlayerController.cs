using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGrounded;

    public ChainManager chainManager; // Reference to the ChainManager script
    public int chainLengthChangeRate = 1; // How fast the chain length changes

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        Jump();
        ManageChain();
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void ManageChain()
    {
        if (chainManager != null)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                PullInChain();
            }
            else if (Input.GetKey(KeyCode.E))
            {
                LetOutChain();
            }
        }
    }
    void PullInChain()
    {
        if (chainManager != null)
        {
            int currentLength = chainManager.GetCurrentChainLength();
            int newLength = currentLength - chainLengthChangeRate;
            chainManager.SetChainLength(Mathf.Max(newLength, 1)); // Ensure the chain doesn't go below 1 link
        }
    }

    void LetOutChain()
    {
        if (chainManager != null)
        {
            int currentLength = chainManager.GetCurrentChainLength();
            int newLength = currentLength + chainLengthChangeRate;
            chainManager.SetChainLength(newLength);
        }
    }

    // These methods will handle collision detection with the ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CompositeCollider2D>() != null)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CompositeCollider2D>() != null)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CompositeCollider2D>() != null)
        {
            isGrounded = false;
        }
    }
}