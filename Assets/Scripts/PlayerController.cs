using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Object Interaction")]
    public float pickupRadius = 1f;
    public float holdDistance = 0.5f;
    public float throwForce = 5f;
    public KeyCode pickupKey = KeyCode.E;

    private Rigidbody2D rb;
    private bool isGrounded;
    private GameObject pickedObject;
    private bool isHoldingObject = false;
    private bool isFacingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        Jump();
        HandleObjectPickup();
        HandleObjectThrow();
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        if (isHoldingObject)
        {
            Vector3 objectPosition = pickedObject.transform.localPosition;
            objectPosition.x *= -1;
            pickedObject.transform.localPosition = objectPosition;
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;  // Immediately set to false for responsiveness
        }
    }

    void HandleObjectPickup()
    {
        if (Input.GetKeyDown(pickupKey))
        {
            if (isHoldingObject)
            {
                DropObject();
            }
            else
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRadius);
                foreach (Collider2D collider in colliders)
                {
                    if (collider.gameObject.CompareTag("Pickupable"))
                    {
                        PickUpObject(collider.gameObject);
                        break;
                    }
                }
            }
        }
    }

    void HandleObjectThrow()
    {
        if (isHoldingObject && Input.GetMouseButtonDown(0))
        {
            ThrowObject();
        }
    }

    void PickUpObject(GameObject obj)
    {
        pickedObject = obj;
        pickedObject.transform.SetParent(transform);
        pickedObject.transform.localPosition = new Vector3(holdDistance, 0, 0);
        pickedObject.transform.localRotation = Quaternion.identity;
        Rigidbody2D objRb = pickedObject.GetComponent<Rigidbody2D>();
        objRb.simulated = false;
        isHoldingObject = true;

        Renderer objRenderer = pickedObject.GetComponent<Renderer>();
        if (objRenderer != null)
        {
            float objectWidth = objRenderer.bounds.size.x;
            pickedObject.transform.localPosition = new Vector3(holdDistance + objectWidth / 2, 0, 0);
        }
    }

    void DropObject()
    {
        pickedObject.transform.SetParent(null);
        Rigidbody2D objRb = pickedObject.GetComponent<Rigidbody2D>();
        objRb.simulated = true;
        pickedObject = null;
        isHoldingObject = false;
    }

    void ThrowObject()
    {
        Rigidbody2D objRb = pickedObject.GetComponent<Rigidbody2D>();
        objRb.simulated = true;
        pickedObject.transform.SetParent(null);
        Vector2 throwDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        objRb.velocity = throwDirection * throwForce;
        pickedObject = null;
        isHoldingObject = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)  // Check if collision is from below
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Collider2D>() != null)
        {
            isGrounded = false;
        }
    }
}