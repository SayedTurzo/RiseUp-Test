using System.Collections;
using UnityEngine;

public class MagnetDynamic : MagnetBase
{
    public float moveDistance = 5f;       // Distance the object moves forward/backward along its local Z axis
    public float moveDuration = 2f;       // Duration for moving the object the full distance

    private Vector3 initialPosition;      // To store the starting position of the object

    private void Start()
    {
        // Store the initial position of the object to return to it after movement
        initialPosition = transform.position;

        // Start the boomerang movement coroutine
        StartCoroutine(BoomerangMovement());
    }
    
    
    private void Update()
    {
        CheckForMagneticInteraction();
    }

    // Coroutine for boomerang movement
    private IEnumerator BoomerangMovement()
    {
        while (true)
        {
            // Move forward
            yield return MoveObject(initialPosition, initialPosition + transform.forward * moveDistance);

            // Move backward
            yield return MoveObject(initialPosition + transform.forward * moveDistance, initialPosition);
        }
    }

    // Coroutine to move the object from start position to target position over a specified duration
    private IEnumerator MoveObject(Vector3 startPos, Vector3 endPos)
    {
        float timeElapsed = 0f;

        while (timeElapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, timeElapsed / moveDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is exactly the target position
        transform.position = endPos;
    }

    // Implementing the abstract method from MagnetBase to define how dynamic magnets interact
    protected override void CheckForMagneticInteraction()
    {
        // Check for nearby objects in range and apply magnetic forces (similar to static magnets)
        Collider[] affectedObjects = Physics.OverlapSphere(transform.position, magnetRange, affectedLayers);

        foreach (Collider collider in affectedObjects)
        {
            if (collider.CompareTag("Player"))
            {
                // Calculate the direction and apply force
                Vector3 direction = collider.transform.position - transform.position;
                ApplyMagneticForce(collider.gameObject, direction);
            }
        }
    }

    private void ApplyMagneticForce(GameObject affectedObject, Vector3 direction)
    {
        direction.Normalize();
        float forceMagnitude = 10f; // Force magnitude can be adjusted

        if (isNorthPole)
        {
            affectedObject.GetComponent<Rigidbody>().AddForce(direction * forceMagnitude);
        }
        else
        {
            affectedObject.GetComponent<Rigidbody>().AddForce(-direction * forceMagnitude);
        }
    }
}
