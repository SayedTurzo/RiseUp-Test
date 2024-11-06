using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpSpeed = 8f;
    public bool isNorthPole = true;
    public CharacterController controller;

    private GameManager gameManager;
    private Vector3 charVelocity;
    private float gravity = -9.81f;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        StartCoroutine(ChangePole());
    }

    private void Update()
    {
        //HandleMovement(); // Handle player movement input
        ApplyGravity();   // Handle gravity and jumping logic
    }

    public void HandleMovement(float horizontalInput, float verticalInput)
    { 
        // Move player horizontally
        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);

        if (moveDirection.magnitude > 0.1f)
        {
            // Move player
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);

            // Make the player face the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // Adjust rotation speed
        }
    }

    public void JumpAction()
    {
        // Jump only if the player is grounded
        if (controller.isGrounded)
        {
            charVelocity.y = jumpSpeed;  // Apply jump force
        }
        controller.Move(charVelocity * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        // Apply gravity only when not grounded
        if (!controller.isGrounded)
        {
            charVelocity.y += gravity * Time.deltaTime;  // Continuously apply gravity when in the air
        }
        else
        {
            charVelocity.y = 0f;  // Reset vertical velocity when grounded
        }

        // Move player using velocity (including gravity)
        controller.Move(charVelocity * Time.deltaTime);
    }

    public void SpeedBoost()
    {
        StartCoroutine(SpeedBoostCoroutine());
    }

    private IEnumerator SpeedBoostCoroutine()
    {
        float originalSpeed = moveSpeed;
        moveSpeed *= 2;
        yield return new WaitForSeconds(0.5f);
        moveSpeed = originalSpeed;
    }

    private IEnumerator ChangePole()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 5f));
            isNorthPole = !isNorthPole;
            Debug.Log("Pole changed: " + (isNorthPole ? "North" : "South"));
        }
    }
}
