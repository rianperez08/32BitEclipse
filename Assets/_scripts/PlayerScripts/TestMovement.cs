using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class TestMovement : MonoBehaviour
{
    public SplineContainer spline;
    public float speed = 1f;
    public float jumpForce = 5f;
    public float sideMovementSpeed = 2f;
    public float sideMovementLimit = 1f;

    private float distancePercentage = 0f;
    private float splineLength;
    private Rigidbody rb;
    private bool isGrounded = true;

    private void Start()
    {
        splineLength = spline.CalculateLength();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Move left and right using A and D keys
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 sideMovement = transform.right * horizontalInput * sideMovementSpeed * Time.deltaTime;
        rb.velocity = new Vector3(sideMovement.x, rb.velocity.y, sideMovement.z);

        // Jump using spacebar
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        // Move along the spline
        distancePercentage += speed * Time.deltaTime / splineLength;

        Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);
        Vector3 targetPosition = currentPosition + transform.right * rb.velocity.x * Time.deltaTime;

        // Clamp the side movement within the specified limit
        float distanceFromSpline = Vector3.Distance(targetPosition, currentPosition);
        if (distanceFromSpline <= sideMovementLimit)
        {
            rb.MovePosition(targetPosition);
        }
        else
        {
            rb.MovePosition(currentPosition);
        }

        if (distancePercentage > 1f)
        {
            distancePercentage = 0f;
        }

        Vector3 nextPosition = spline.EvaluatePosition(distancePercentage + 0.05f);
        Vector3 direction = nextPosition - currentPosition;
        transform.rotation = Quaternion.LookRotation(direction, transform.up);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player is grounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}