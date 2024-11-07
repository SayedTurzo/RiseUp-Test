using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;  // Reference to the player (the Capsule object)
    public Vector3 offset;    // The offset distance between the camera and player
    public float smoothSpeed = 0.125f;  // Smooth follow speed

    private void Start()
    {
        // Keep the initial camera rotation intact
        transform.rotation = Quaternion.Euler(15f, 0f, 0f); // Ensure camera is aligned initially
    }

    private void LateUpdate()
    {
        // Calculate the desired camera position based on the player's position and the offset
        Vector3 desiredPosition = player.position + offset;

        // Smoothly move the camera to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera's position
        transform.position = smoothedPosition;
    }
}