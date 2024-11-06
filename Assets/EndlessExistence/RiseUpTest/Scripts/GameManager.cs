using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Joystick joystick;
    public PlayerController playerController;

    private TouchController touchController;

    private void Start()
    {
        touchController = GetComponent<TouchController>();

        // Subscribe to the swipe-up event for jump action
        touchController.OnSwipeUpEvent.AddListener(playerController.JumpAction);

        // Subscribe to the double-tap event for speed boost action
        touchController.OnDoubleTapEvent.AddListener(playerController.SpeedBoost);
    }

    private void Update()
    {
        // Handle player movement using joystick inputs
        playerController.HandleMovement(joystick.Horizontal, joystick.Vertical);
    }
}