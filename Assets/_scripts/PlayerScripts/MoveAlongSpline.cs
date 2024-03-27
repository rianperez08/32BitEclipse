using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class MoveAlongSpline : MonoBehaviour
{
    public SplineContainer spline;
    public float speed = 1f;
    public float maxSpeed = 5f; // Maximum speed limit

    float distancePercentage = 0f;
    float splineLength;

    private void Start()
    {
        splineLength = spline.CalculateLength();
    }

    void Update()
    {
        // Get the input axes
        float horizontal = Input.GetAxis("Horizontal");

        // Clamp the speed to the maximum speed
        float currentSpeed = Mathf.Clamp(speed * horizontal, -maxSpeed, maxSpeed);

        // Update the distance percentage based on the current speed
        distancePercentage += currentSpeed * Time.deltaTime / splineLength;

        // Wrap the distance percentage if it goes beyond 0 or 1
        distancePercentage = Mathf.Repeat(distancePercentage, 1f);

        // Evaluate the position on the spline based on the distance percentage
        Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);
        transform.position = currentPosition;

        // Calculate the next position and rotation
        Vector3 nextPosition = spline.EvaluatePosition(distancePercentage + 0.05f);
        Vector3 direction = nextPosition - currentPosition;
        transform.rotation = Quaternion.LookRotation(direction, transform.up);
    }
}