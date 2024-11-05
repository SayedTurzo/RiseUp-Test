using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region Serialized Private Variables (Action References)

    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name Reference")]
    [SerializeField] private string move = "Move";
    [SerializeField] private string jump = "Jump";

    #endregion

    #region Private Variables (Input Actions)

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputActionMap actionMap;

    #endregion

    #region Public Properties (To get input Values)

    public Vector2 MoveInput { get; private set; }
    public static InputManager Instance { get; private set; }

    #endregion

    #region Events

    // Events for Jump and Sprint
    public UnityEvent _onJump;
    public UnityEvent _onSprint;

    #endregion

    #region Swipe and Double-Tap Detection Variables

    private Vector2 initialPointerPosition;
    private float lastTapTime;
    private const float doubleTapThreshold = 0.3f; // Adjust as needed

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        InitializeSingleton();
        actionMap = playerControls.FindActionMap(actionMapName);
        InitializeInputActions();
        RegisterInputActions();
    }

    private void OnEnable()
    {
        actionMap.Enable();
    }

    private void OnDisable()
    {
        actionMap.Disable();
    }

    private void Update()
    {
        DetectSwipeUp();
        DetectDoubleTap();
    }

    #endregion

    #region Initialization Methods

    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeInputActions()
    {
        moveAction = actionMap.FindAction(move);
        jumpAction = actionMap.FindAction(jump);
    }

    #endregion

    #region Input Registration

    private void RegisterInputActions()
    {
        // Movement Input
        moveAction.performed += context =>
        {
            MoveInput = context.ReadValue<Vector2>(); // Get movement input
            Debug.Log("Moving with input: " + MoveInput); // Debug log for movement
        };
        moveAction.canceled += context =>
        {
            MoveInput = Vector2.zero; // Reset movement input
            Debug.Log("Movement stopped"); // Debug log for stopping movement
        };
        
        // Jump Input
        jumpAction.performed += context =>
        {
            _onJump.Invoke();
            Debug.Log("Jump triggered");
        };
    }

    #endregion

    #region Custom Swipe Up and Double Tap Detection

    private void DetectSwipeUp()
    {
        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;

            if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                initialPointerPosition = touch.position.ReadValue();
                Debug.Log("Touch started at position: " + initialPointerPosition);
            }
            if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
            {
                Vector2 finalPointerPosition = touch.position.ReadValue();
                float swipeDistance = finalPointerPosition.y - initialPointerPosition.y;

                if (swipeDistance > 100) // Adjust swipe threshold as needed
                {
                    _onJump.Invoke();
                    Debug.Log("Swipe Up detected: Jump triggered");
                }
            }
        }
        else
        {
            // Mouse swipe detection
            if (Input.GetMouseButtonDown(0))
            {
                initialPointerPosition = Input.mousePosition; // Record the starting position of the swipe
                Debug.Log("Mouse down at position: " + initialPointerPosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                Vector2 finalMousePosition = Input.mousePosition; // Record the ending position of the swipe
                float swipeDistance = finalMousePosition.y - initialPointerPosition.y;

                if (swipeDistance > 100) // Adjust swipe threshold as needed
                {
                    _onJump.Invoke();
                    Debug.Log("Swipe Up detected: Jump triggered");
                }
            }
        }
    }

    private void DetectDoubleTap()
    {
        // Check for mouse double-click
        if (Input.GetMouseButtonDown(0))
        {
            float timeSinceLastTap = Time.time - lastTapTime;

            if (timeSinceLastTap < doubleTapThreshold)
            {
                _onSprint.Invoke();
                Debug.Log("Double Click detected: Sprint triggered");
            }

            lastTapTime = Time.time; // Update last tap time
        }

        // Check for touch double tap
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            float timeSinceLastTap = Time.time - lastTapTime;

            if (timeSinceLastTap < doubleTapThreshold)
            {
                _onSprint.Invoke();
                Debug.Log("Double Tap detected: Sprint triggered");
            }

            lastTapTime = Time.time; // Update last tap time
        }
    }

    #endregion
}
