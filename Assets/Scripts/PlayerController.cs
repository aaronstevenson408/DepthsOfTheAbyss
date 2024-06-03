using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        CheckIfGrounded();
        Jump();
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void Jump()
    {
        //Debug.Log("Is Grounded:" + isGrounded);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void CheckIfGrounded()
    {
        // Adjust the raycast origin and length as needed
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - 0.5f); // Adjust the 0.5f based on your player's collider size
        float raycastLength = 0.2f; // Adjust as needed

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, raycastLength, LayerMask.GetMask("Ground"));
        Debug.DrawRay(origin, Vector2.down * raycastLength, Color.red);

        if (hit.collider != null)
        {
            isGrounded = true;
            //Debug.Log("Grounded by Raycast");
        }
        else
        {
            isGrounded = false;
            // Debug.Log("Not Grounded by Raycast");
        }
    }
}