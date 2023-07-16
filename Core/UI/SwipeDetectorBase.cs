using System;
using UnityEngine;

public class SwipeDetectorBase : MonoBehaviour
{
    public Action<SwipeDirection> onSwipe;
}
public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right
}
