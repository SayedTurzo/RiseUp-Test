using System;
using System.Collections;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    #region Serialized Fields

    [Header("Movement Settings")]
    [SerializeField] private float normalSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float airControlMultiplier = 0.5f;
    [SerializeField] private float rotationSpeed = 10f; // Speed of rotation to face the movement direction

    [Header("Ground Detection Settings")]
    [SerializeField] private LayerMask groundLayer; // LayerMask for ground detection
    [SerializeField] private float groundCheckDistance = 0.2f; // Distance to check for ground

    [Header("References")]
    [SerializeField] private Rigidbody rb;

    #endregion

    #region Private Fields

    private Vector3 moveDirection;
    private float currentSpeed;
    private bool isGrounded;
    private bool isSprinting;

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        // Set initial speed to normal speed
        currentSpeed = normalSpeed;

        // Subscribe to InputManager events
        InputManager.Instance._onJump.AddListener(Jump);
        InputManager.Instance._onSprint.AddListener(StartSprint);
    }

    private void Update()
    {
        // Check if the player is grounded
        CheckGroundStatus();
    }

    private void FixedUpdate()
    {
        // Get movement input from InputManager
        Vector2 input = InputManager.Instance.MoveInput;

        // Calculate move direction based on joystick input
        moveDirection = new Vector3(input.x, 0f, input.y).normalized;

        // Apply movement if there's input
        if (moveDirection.magnitude >= 0.1f)
        {
            // Apply movement
            Vector3 movement = moveDirection * currentSpeed * (isGrounded ? 1f : airControlMultiplier);
            rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);

            // Rotate the player to face the movement direction
            RotateToMovementDirection(movement);
        }
    }

    #endregion

    #region Ground Detection

    private void CheckGroundStatus()
    {
        // Check if there is ground below the player using a Raycast
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    #endregion

    #region Movement Functions

    private void Jump()
    {
        if (isGrounded)
        {
            // Apply vertical force to simulate jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            Debug.Log("Jump triggered");
        }
    }

    private void StartSprint()
    {
        if (!isSprinting)
        {
            isSprinting = true;
            currentSpeed = sprintSpeed;
            Debug.Log("Sprint started");
            StartCoroutine(SprintDuration());
        }
    }

    private IEnumerator SprintDuration()
    {
        // Sprint lasts for 5 seconds
        yield return new WaitForSeconds(5f);
        StopSprint();
    }

    private void StopSprint()
    {
        // Reset speed to normal and end sprint
        isSprinting = false;
        currentSpeed = normalSpeed;
        Debug.Log("Sprint ended");
    }

    #endregion

    #region Rotation

    private void RotateToMovementDirection(Vector3 movement)
    {
        // Calculate the target rotation based on the movement direction
        Quaternion targetRotation = Quaternion.LookRotation(movement);

        // Smoothly rotate towards the target direction
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    #endregion
}
