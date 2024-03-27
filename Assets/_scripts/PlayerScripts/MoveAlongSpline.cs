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

    private void Start()
    {
        splineLength = spline.CalculateLength();
    }

    private void Update()
    {
        // Get the input axes
        float horizontal = Input.GetAxis("Horizontal");

        // Clamp the speed to the maximum speed
        float currentSpeed = Mathf.Clamp(speed * -horizontal, -maxSpeed, maxSpeed);

        // Update the distance percentage based on the current speed
        distancePercentage += currentSpeed * Time.deltaTime / splineLength;

        // Wrap the distance percentage if it goes beyond 0 or 1
        distancePercentage = Mathf.Repeat(distancePercentage, 1f);

        // Evaluate the position on the spline based on the distance percentage
        Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);

        // Check for jump input
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            jumpStartTime = Time.time;
            jumpStartPosition = currentPosition;
            jumpStartSpeed = currentSpeed;
        }

        // Apply jump
        if (isJumping)
        {
            float jumpTime = Time.time - jumpStartTime;
            float normalizedJumpTime = Mathf.Clamp01(jumpTime / jumpDuration);
            float jumpHeight = Mathf.Sin(normalizedJumpTime * Mathf.PI) * this.jumpHeight;

            // Calculate the target position on the spline based on the jump progress
            float jumpDistancePercentage = Mathf.Lerp(distancePercentage, distancePercentage + jumpStartSpeed * Time.deltaTime / splineLength, normalizedJumpTime);
            Vector3 targetPosition = spline.EvaluatePosition(jumpDistancePercentage);

            // Update the player position with the jump height and spline position
            transform.position = targetPosition + Vector3.up * jumpHeight;

            if (jumpTime >= jumpDuration)
            {
                isJumping = false;
                distancePercentage = jumpDistancePercentage; // Update the distance percentage after the jump
            }
        }
        else
        {
            transform.position = currentPosition;
        }

        // Calculate the next position and rotation
        Vector3 nextPosition = spline.EvaluatePosition(distancePercentage + 0.05f);
        Vector3 direction = nextPosition - currentPosition;
        transform.rotation = Quaternion.LookRotation(direction, transform.up);
    }
}