using UnityEngine;

public class MagnetStatic : MagnetBase
{
    private void Update()
    {
        // Static magnets only check for interactions in every frame
        //CheckForMagneticInteraction();
    }

    // Implementing the abstract method from MagnetBase to define how static magnets interact
    protected override void CheckForMagneticInteraction()
    {
        // Detect objects within the magnet's range
        Collider[] affectedObjects = Physics.OverlapSphere(transform.position, magnetRange, affectedLayers);

        foreach (Collider collider in affectedObjects)
        {
            // If the collider is affected (e.g., a player or other object), apply the magnetic force
            if (collider.CompareTag("Player"))  // Check for objects with a specific tag, like "Player"
            {
                // Calculate direction towards the magnet
                Vector3 direction = collider.transform.position - transform.position;

                // Apply magnetic force (attraction or repulsion) based on magnet's pole
                ApplyMagneticForce(collider.gameObject, direction);
            }
        }
    }

    private void ApplyMagneticForce(GameObject affectedObject, Vector3 direction)
    {
        direction.Normalize();
        float forceMagnitude = 10f; // Define how strong the force is

        if (isNorthPole)
        {
            affectedObject.GetComponent<Rigidbody>().AddForce(direction * forceMagnitude); // Attraction
        }
        else
        {
            affectedObject.GetComponent<Rigidbody>().AddForce(-direction * forceMagnitude); // Repulsion
        }
    }
}