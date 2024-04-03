using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class MoveAlongSpline : MonoBehaviour
{
    public SplineContainer spline;
    public float speed = 1f;
    public float maxSpeed = 5f; // Maximum speed limit
    public float jumpHeight = 2f; // Height of the jump
    public float jumpDuration = 0.5f; // Duration of the jump

    private float distancePercentage;
    private float splineLength;
    private bool isJumping;
    private float jumpStartTime;
    private Vector3 jumpStartPosition;
    private float jumpStartSpeed;
    private Rigidbody rb;

    private void Start()
    {
        splineLength = spline.CalculateLength();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Get the input axes
        float horizontal = Input.GetAxis("Horizontal");

        // Clamp the speed to the maximum speed
        float currentSpeed = Mathf.Clamp(speed * -horizontal, -maxSpeed, maxSpeed);

        // Update the distance percentage based on the current speed
        distancePercentage += currentSpeed * Time.fixedDeltaTime / splineLength;

        // Wrap the distance percentage if it goes beyond 0 or 1
        distancePercentage = Mathf.Repeat(distancePercentage, 1f);

        // Evaluate the position on the spline based on the distance percentage
        Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);

        // Evaluate the tangent direction on the spline based on the distance percentage
        Vector3 direction = spline.EvaluateTangent(distancePercentage);

        // Set the Rigidbody's velocity based on the current speed and direction
        rb.velocity = direction.normalized * currentSpeed;

        // Rotate the Rigidbody to align with the spline's direction
        Quaternion targetRotation = Quaternion.LookRotation(direction, transform.up);
        rb.MoveRotation(targetRotation);

        // Check for jump input
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            jumpStartTime = Time.time;
            jumpStartPosition = rb.position;
            jumpStartSpeed = currentSpeed;
        }

        // Handle jumping
        if (isJumping)
        {
            float jumpProgress = (Time.time - jumpStartTime) / jumpDuration;

            if (jumpProgress < 1f)
            {
                // Calculate the jump offset
                Vector3 jumpOffset = Vector3.up * Mathf.Sin(jumpProgress * Mathf.PI) * jumpHeight;

                // Maintain the running momentum during the jump
                Vector3 runningVelocity = direction.normalized * jumpStartSpeed;
                rb.velocity = runningVelocity + Vector3.up * Mathf.Cos(jumpProgress * Mathf.PI) * jumpHeight / jumpDuration;
            }
            else
            {
                isJumping = false;
            }
        }
    }
}