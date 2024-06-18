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
        HandleObjectInteraction();
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
                    IPickup pickup = collider.GetComponent<IPickup>();
                    if (pickup != null)
                    {
                        pickup.OnPickup(this);
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

    void HandleObjectInteraction()
    {
        if (isHoldingObject)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRadius);
            foreach (Collider2D collider in colliders)
            {
                IInteractable interactable = collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact(pickedObject);
                }
            }
        }
    }

    public void PickUpObject(GameObject obj)
    {
        pickedObject = obj;
        pickedObject.transform.SetParent(transform);
        pickedObject.transform.localPosition = new Vector3(holdDistance, 0, 0);
        pickedObject.transform.localRotation = Quaternion.identity;

        Rigidbody2D objRb = pickedObject.GetComponent<Rigidbody2D>();
        objRb.simulated = false;
        objRb.isKinematic = true;

        Collider2D objCollider = pickedObject.GetComponent<Collider2D>();
        objCollider.enabled = true;

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
        objRb.isKinematic = false;

        pickedObject = null;
        isHoldingObject = false;
    }

    void ThrowObject()
    {
        Rigidbody2D objRb = pickedObject.GetComponent<Rigidbody2D>();
        objRb.simulated = true;
        objRb.isKinematic = false;

        pickedObject.transform.SetParent(null);
        Vector2 throwDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        objRb.velocity = throwDirection * throwForce;

        pickedObject = null;
        isHoldingObject = false;
    }

    private bool IsCollidingFromAbove(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y >= 0.5f)
            {
                return true;
            }
        }
        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CompositeCollider2D>() != null && IsCollidingFromAbove(collision))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CompositeCollider2D>() != null && IsCollidingFromAbove(collision))
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
