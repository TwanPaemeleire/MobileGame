using UnityEngine;
using UnityEngine.Events;

public class BaseScreen : MonoBehaviour
{
    public UnityEvent OnScreenOpened = new UnityEvent();
    public UnityEvent OnScreenClosed = new UnityEvent();

    virtual protected void OnScreenOpenedInternal() { }
    virtual protected void OnScreenClosedInternal() { }
}
