using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TouchController : MonoBehaviour
{
    public List<RectTransform> excludeRectList;
    public int joystickTouchId = -1;

    [HideInInspector]
    public UnityEvent OnSwipeUpEvent;

    [HideInInspector]
    public UnityEvent OnDoubleTapEvent;

    [SerializeField] private float swipeThreshold = 10f;
    [SerializeField] private float doubleTapTime = 0.3f;
    private Vector2 touchBeganPos;
    private int currentTouchId = -1;
    private bool firstTapped;
    private bool tapActive;
    private float tapTimer;

    private void Update()
    {
        HandleTouchInput();
    }

    private void HandleTouchInput()
    {
        // Handle real touches
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                // Process touch only if it's the first touch (no other fingers should trigger swipe)
                if (currentTouchId == -1) 
                {
                    // If we haven't started tracking any touch yet, let's track the first touch
                    ProcessTouch(touch.fingerId, touch.position, touch.phase);
                }
                else if (touch.fingerId == currentTouchId)
                {
                    // If we're already tracking a touch, only process that touch
                    ProcessTouch(touch.fingerId, touch.position, touch.phase);
                }
            }
        }
    }

    private void ProcessTouch(int fingerId, Vector2 position, TouchPhase phase)
    {
        // Ignore if touch is for joystick or within excluded areas
        if (fingerId == joystickTouchId || IsTouchInExcludedArea(position))
            return;

        switch (phase)
        {
            case TouchPhase.Began:
                touchBeganPos = position;
                currentTouchId = fingerId;

                // Handle double-tap detection
                if (firstTapped && tapActive)
                {
                    OnDoubleTapEvent.Invoke();
                    ResetTap();
                }
                else
                {
                    firstTapped = true;
                    tapActive = true;
                    tapTimer = doubleTapTime;
                    StartCoroutine(DoubleTapHold());
                }
                break;

            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                CheckSwipeUp(fingerId, position);
                break;

            case TouchPhase.Ended:
                CheckSwipeUp(fingerId, position);
                if (fingerId == currentTouchId)
                    currentTouchId = -1;
                break;
        }
    }

    // Method to check for swipe-up gesture
    private void CheckSwipeUp(int fingerId, Vector2 position)
    {
        // Only allow swipe-up with the current touch (first touch)
        if (fingerId == currentTouchId && position.y > touchBeganPos.y &&
            Vector2.Distance(touchBeganPos, position) > swipeThreshold)
        {
            Debug.Log("Swiped up");
            OnSwipeUpEvent.Invoke();
            ResetTap();
        }
    }

    // Reset the tap variables
    public void ResetTap()
    {
        firstTapped = false;
        tapActive = false;
        currentTouchId = -1;
    }

    private IEnumerator DoubleTapHold()
    {
        yield return new WaitForSeconds(doubleTapTime);
        if (tapActive)
        {
            firstTapped = false;
            tapActive = false;
        }
    }

    private bool IsTouchInExcludedArea(Vector2 point)
    {
        foreach (RectTransform rect in excludeRectList)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(rect, point))
                return true;
        }
        return false;
    }
}
