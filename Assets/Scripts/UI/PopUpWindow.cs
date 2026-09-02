using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PopUpWindow : MonoBehaviour
{
    [SerializeField] private bool _closesOnClickOutside = true;

    [SerializeField] private bool _doPopUpAnimation = true;
    [ShowIf(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(_doPopUpAnimation))]
    [SerializeField] private float _popUpMaxScale = 1.1f;
    [ShowIf(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(_doPopUpAnimation))]
    [SerializeField] private float _scaleUpTime = 0.2f;
    [ShowIf(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(_doPopUpAnimation))]
    [SerializeField] private float _scaleDownTime = 0.2f;

    public UnityEvent OnWindowOpened = new UnityEvent();
    public UnityEvent OnWindowClosed = new UnityEvent();

    public void OpenWindow()
    {
        InputHandler.Instance.OnTouchStarted.AddListener(OnClickStarted);
        OnWindowOpenedInternal();
        gameObject.SetActive(true);
        StartCoroutine(AnimateWindowPopUp());
        OnWindowOpened.Invoke();
    }

    public void CloseWindow()
    {
        InputHandler.Instance.OnTouchStarted.RemoveListener(OnClickStarted);
        StopAllCoroutines();
        OnWindowClosedInternal();
        gameObject.SetActive(false);
        OnWindowClosed.Invoke();
    }

    protected virtual void OnWindowOpenedInternal() { }
    protected virtual void OnWindowClosedInternal() { }

    private void OnClickStarted(int id)
    {
        if (!_closesOnClickOutside || InputHandler.Instance.TouchCount != 1) return;
        if (!InputHandler.Instance.IsTouchInsideRectTransform(id, GetComponent<RectTransform>()))
        {
            CloseWindow();
        }
    }

    private IEnumerator AnimateWindowPopUp()
    {
        yield return StartCoroutine(ChangeScale(1.0f, _popUpMaxScale, _scaleUpTime));
        yield return StartCoroutine(ChangeScale(_popUpMaxScale, 1.0f, _scaleDownTime));
    }

    private IEnumerator ChangeScale(float from, float to, float duration)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        float timer = 0.0f;
        while(timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            float scale = Mathf.SmoothStep(from, to, t);
            rectTransform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
        rectTransform.localScale = new Vector3(to, to, to);
    }
}
