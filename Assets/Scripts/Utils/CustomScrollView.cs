using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CustomScrollView : MonoBehaviour
{
    [SerializeField] private float _topMaxExcessScroll;
    [SerializeField] private float _bottomMaxExcessScroll;
    [SerializeField] private RectTransform _contentParentTransform;
    [SerializeField] private RectTransform _contentContainerTransform;
    [SerializeField] private float _moveBackTime = 0.5f;

    private float _currentScrollSpeed = 0.0f;
    private int _trackingTouchId = -1;
    private bool _currentlyTrackingTouch = false;

    private float _topMaxPosition = 0.0f;
    private float _bottomMaxPosition = 0.0f;
    private float _topPosition = 0.0f;
    private float _bottomPosition = 0.0f;

    private void OnEnable()
    {
        RectTransform bottomElementTransform = _contentContainerTransform.GetChild(0).GetComponent<RectTransform>();
        int childCount = _contentContainerTransform.childCount;
        float elementHeight = bottomElementTransform.rect.height;
        float startY = -elementHeight / 2.0f;
        float bottomElementY = startY + (elementHeight * childCount);

        float bottomElementHalfHeight = elementHeight / 2.0f;
        float scrollViewHeight = GetComponent<RectTransform>().rect.height;

        _topPosition = 0.0f;
        _topMaxPosition = -_topMaxExcessScroll;
        _bottomPosition = bottomElementY + bottomElementHalfHeight - scrollViewHeight;
        _bottomMaxPosition = _bottomPosition + _bottomMaxExcessScroll;

        _contentParentTransform.anchoredPosition = Vector2.zero;
        _currentScrollSpeed = 0.0f;
        InputHandler.Instance.OnTouchStarted.AddListener(OnTouchStarted);
        InputHandler.Instance.OnTouchEnded.AddListener(OnTouchEnded);
    }

    private void OnDisable()
    {
        _currentScrollSpeed = 0.0f;
        InputHandler.Instance.OnTouchStarted.RemoveListener(OnTouchStarted);
        InputHandler.Instance.OnTouchEnded.RemoveListener(OnTouchEnded);
    }

    private void OnTouchStarted(int id)
    {
        if (InputHandler.Instance.TouchCount != 1 || !PressIsInScrollView(InputHandler.Instance.GetTouchPosition(id))) return;
        StopAllCoroutines();
        _currentlyTrackingTouch = true;
        _trackingTouchId = id;
    }

    private void OnTouchEnded(int id)
    {
        if (id != _trackingTouchId || !_currentlyTrackingTouch) return;
        _currentlyTrackingTouch = false;
        _trackingTouchId = -1;

        StopAllCoroutines();
        if (_contentParentTransform.anchoredPosition.y > _bottomPosition)
        {
            StartCoroutine(MoveBackToNonExcessArea(false));
        }
        else if (_contentParentTransform.anchoredPosition.y < _topPosition)
        {
            StartCoroutine(MoveBackToNonExcessArea(true));
        }
    }

    private bool PressIsInScrollView(Vector2 mousePos)
    {
        return true;
    }

    private void Update()
    {
        if (!_currentlyTrackingTouch) return;

        float deltaY = InputHandler.Instance.GetTouchDeltaPosition(_trackingTouchId).y;
        if (deltaY == 0.0f) return;
        float currentY = _contentParentTransform.anchoredPosition.y;

        bool topExceeded = currentY < _topPosition;
        bool bottomExceeded = currentY > _bottomPosition;

        if (topExceeded)
        {
            float excess = _topPosition - currentY;
            float maxExcess = _topPosition - _topMaxPosition;

            float progress = Mathf.Clamp01(excess / maxExcess);
            float resistance = 1.0f - progress;

            if (deltaY < 0.0f) deltaY *= resistance;
        }
        else if (bottomExceeded)
        {
            float excess = currentY - _bottomPosition;
            float maxExcess = _bottomMaxPosition - _bottomPosition;

            float progress = Mathf.Clamp01(excess / maxExcess);
            float resistance = 1.0f - progress;

            if (deltaY > 0.0f) deltaY *= resistance;
        }

        _contentParentTransform.anchoredPosition += new Vector2(0.0f, deltaY);
    }

    private IEnumerator MoveBackToNonExcessArea(bool isTop)
    {
        float timer = 0.0f;
        float startPos = _contentParentTransform.anchoredPosition.y;
        float endPos = (isTop) ? _topPosition : _bottomPosition;
        while(timer < _moveBackTime)
        {
            timer += Time.deltaTime;
            float progress = timer / _moveBackTime;
            float newYPos = Mathf.SmoothStep(startPos, endPos, progress);
            _contentParentTransform.anchoredPosition = new Vector2(_contentParentTransform.anchoredPosition.x, newYPos);
            yield return null;
        }
        _contentParentTransform.anchoredPosition = new Vector2(_contentParentTransform.anchoredPosition.x, endPos);
    }
}