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

    [SerializeField] private float swipeThreshold = 10f; // Minimum distance for swipe
    [SerializeField] private float doubleTapTime = 0.3f; // Time threshold for double-tap
    private Vector2 touchBeganPos;
    [SerializeField] private int currentTouchId = -1;
    private bool firstTapped;
    private bool tapActive;
    private float tapTimer;

    private void Update()
    {
        HandleTouchInput();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                // Ignore if the touch ID is the joystick's or is in the excluded area
                if (touch.fingerId == joystickTouchId || IsTouchInExcludedArea(touch.position))
                    continue;

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        touchBeganPos = touch.position;
                        currentTouchId = touch.fingerId;

                        // Handle double-tap detection
                        if (firstTapped && tapActive)
                        {
                            OnDoubleTapEvent.Invoke(); // Trigger the speed boost action
                            ResetTap(); // Reset tap after double-tap event
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
                    case TouchPhase.Ended:
                        Vector2 touchEndPos = touch.position;
                        // Check if swipe threshold is met and swipe is upward
                        if (touchEndPos.y > touchBeganPos.y)  //touch.fingerId == currentTouchId && Vector2.Distance(touchBeganPos, touchEndPos) > swipeThreshold &&
                        {
                            OnSwipeUpEvent.Invoke(); // Trigger swipe-up event for jump
                            ResetTap(); // Reset after swipe-up
                        }
                        break;
                }
            }
        }

        // Reset tap timer if active
        if (tapActive)
        {
            tapTimer -= Time.deltaTime;
            if (tapTimer <= 0) tapActive = false;
        }
    }

    // Reset the tap variables
    public void ResetTap()
    {
        firstTapped = false;
        tapActive = false;
        //currentTouchId = -1;  // Reset touch ID to allow new actions
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
