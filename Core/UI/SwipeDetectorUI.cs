using UnityEngine;
using UnityEngine.EventSystems;


public class SwipeDetectorUI : SwipeDetectorBase, IPointerDownHandler, IPointerUpHandler
{
    
    private Vector2 fingerDown;
    private Vector2 fingerUp;

    public bool detectSwipeOnlyAfterRelease = false;

    public float minDistanceForSwipe = 20f;

    public void OnPointerDown(PointerEventData eventData)
    {
        fingerUp = fingerDown = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        fingerDown = eventData.position;
        CheckSwipe();
    }

    private void CheckSwipe()
    {
        if (SwipeDistanceCheck())
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

