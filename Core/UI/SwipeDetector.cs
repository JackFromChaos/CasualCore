using System;
using UnityEngine;

public class SwipeDetector : SwipeDetectorBase
{
    private Vector2 fingerDown;
    private Vector2 fingerUp;

    public bool detectSwipeOnlyAfterRelease = false;

    public float minDistanceForSwipe = 20f;

    // Флаг, определяющий, было ли уже выполнено событие для данного касания
    private bool swipeEventDone = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            fingerUp = fingerDown = Input.mousePosition;
            swipeEventDone = false;
        }

        if (!detectSwipeOnlyAfterRelease && Input.GetMouseButton(0))
        {
            fingerDown = Input.mousePosition;
            CheckSwipe();
        }

        if (Input.GetMouseButtonUp(0))
        {
            fingerDown = Input.mousePosition;
            CheckSwipe();
        }
    }

    private void CheckSwipe()
    {
        if (!swipeEventDone && SwipeDistanceCheck())
        {
            if (IsVerticalSwipe())
            {
                var direction = fingerDown.y - fingerUp.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                onSwipe?.Invoke(direction);
            }
            else
            {
                var direction = fingerDown.x - fingerUp.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                onSwipe?.Invoke(direction);
            }

            swipeEventDone = true;
            fingerUp = fingerDown;
        }
    }

    private bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }

    private bool SwipeDistanceCheck()
    {
        return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;
    }

    private float VerticalMovementDistance()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    private float HorizontalMovementDistance()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }
}

