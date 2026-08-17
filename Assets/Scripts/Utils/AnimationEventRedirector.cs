using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventRedirector : MonoBehaviour
{
    private Dictionary<string, Action> _animationActions = new Dictionary<string, Action>();

    public void RegisterAction(string name, Action action)
    {
        if (string.IsNullOrEmpty(name) || action == null) return;
        _animationActions[name] = action;
    }

    public bool TryExecuteAction(string name)
    {
        if (_animationActions == null) return false;
        if (_animationActions.TryGetValue(name, out var action))
        {
            action.Invoke();
            return true;
        }
        return false;
    }
}
