using System.Collections;
using UnityEngine;

public class MagnetDynamic : MagnetBase
{
    public float moveDistance = 5f;
    public float moveDuration = 2f;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
        StartCoroutine(BoomerangMovement());
    }

    private void Update()
    {
        CheckForMagneticInteraction();
    }

    private IEnumerator BoomerangMovement()
    {
        while (true)
        {
            yield return MoveObject(initialPosition, initialPosition + transform.forward * moveDistance);
            yield return MoveObject(initialPosition + transform.forward * moveDistance, initialPosition);
        }
    }

    private IEnumerator MoveObject(Vector3 startPos, Vector3 endPos)
    {
        float timeElapsed = 0f;

        while (timeElapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, timeElapsed / moveDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
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
