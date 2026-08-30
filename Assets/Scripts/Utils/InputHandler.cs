using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Linq;

public class InputHandler : MonoSingleton<InputHandler>
{
    public int TouchCount => Touchscreen.current.touches.Count(touch => touch.press.isPressed);
    public int TouchCountIncludingReleases => Touchscreen.current.touches.Count(touch => touch.press.isPressed || touch.press.wasReleasedThisFrame);

    public UnityEvent<int> OnTouchStarted = new UnityEvent<int>();
    public UnityEvent<int> OnTouchEnded = new UnityEvent<int>();

    private void Update()
    {
        if (TouchCountIncludingReleases == 0) return;

        foreach(TouchControl touch in Touchscreen.current.touches)
        {
            if(touch.press.wasPressedThisFrame) // Touch started
            {
                OnTouchStarted.Invoke(touch.touchId.value);
            }
            else if(touch.press.wasReleasedThisFrame) // Touch ended
            {
                OnTouchEnded.Invoke(touch.touchId.value);
            }
        }
    }

    public Vector2 GetTouchDeltaPosition(int touchId)
    {
        return Touchscreen.current.touches.Where((TouchControl touch) => touch.touchId.value == touchId).FirstOrDefault().delta.value;
    }

    public Vector2 GetTouchPosition(int touchId)
    {
        return Touchscreen.current.touches.Where((TouchControl touch) => touch.touchId.value == touchId).FirstOrDefault().position.value;
    }
}
