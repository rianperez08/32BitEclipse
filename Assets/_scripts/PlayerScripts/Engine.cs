using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Engine : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float power;
    [SerializeField] private float jumpForce;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Throttle based on A and D keys
        float throttle = 0;
        if (Input.GetKey(KeyCode.A))
        {
            throttle = power;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            throttle = -power;
        }

        Throttle(throttle);

        // Jump
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Throttle(float power)
    {
        Vector3 dir = power * transform.forward;
        rb.AddForce(dir);
    }

    private void Jump()
    {
        // Apply upward impulse force for jump
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private bool IsGrounded()
    {
        // Perform a raycast downward to check if the cart is grounded
        return Physics.Raycast(transform.position, -transform.up, 0.1f);
    }
}
