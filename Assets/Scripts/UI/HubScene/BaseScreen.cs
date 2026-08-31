using UnityEngine;
using UnityEngine.Events;

public abstract class BaseScreen : MonoBehaviour
{
    public UnityEvent OnScreenOpened = new UnityEvent();
    public UnityEvent OnScreenClosed = new UnityEvent();

    abstract public void OnScreenOpenedInternal();
    abstract public void OnScreenClosedInternal();
}
