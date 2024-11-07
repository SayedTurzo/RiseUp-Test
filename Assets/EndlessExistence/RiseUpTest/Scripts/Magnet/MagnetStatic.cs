using UnityEngine;

public class MagnetStatic : MagnetBase
{
    private void Update()
    {
        CheckForMagneticInteraction();
    }

    protected override void CheckForMagneticInteraction()
    {
        Collider[] affectedObjects = Physics.OverlapSphere(transform.position, magnetRange, affectedLayers);

        foreach (Collider collider in affectedObjects)
        {
            if (collider.CompareTag("Player"))
            {
                Vector3 surfaceNormal = (collider.transform.position - transform.position).normalized;
                ApplyMagneticForce(collider.gameObject, surfaceNormal);
            }
        }
    }

    private void ApplyMagneticForce(GameObject affectedObject, Vector3 surfaceNormal)
    {
        PlayerController playerController = affectedObject.GetComponent<PlayerController>();
        CharacterController controller = affectedObject.GetComponent<CharacterController>();

        if (playerController != null && controller != null)
        {
            float forceMagnitude = 10f;
            bool isAttractive = (isNorthPole == playerController.isNorthPole);

            Vector3 force = isAttractive ? surfaceNormal * forceMagnitude : -surfaceNormal * forceMagnitude;
            Vector3 movement = new Vector3(force.x, (playerController.GetYVelocity()-1f), force.z) * Time.deltaTime;  // No Y-axis influence

            controller.Move(movement);
        }
    }
}