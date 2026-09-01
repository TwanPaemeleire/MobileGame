using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CustomScrollView : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private RectTransform _contentParentTransform;
    [SerializeField] private RectTransform _contentContainerTransform;

    [Header("Excess areas")]
    [SerializeField] private float _topMaxExcessScroll;
    [SerializeField] private float _bottomMaxExcessScroll;
    [SerializeField] private float _moveBackTime = 0.5f;

    [Header("Scroll settings")]
    [SerializeField] private float _scrollSpeedMultiplier = 50.0f;
    [SerializeField] private float _scrollSpeedDecreaseAmount = 40.0f;
    [SerializeField] private float _maxTimeToDecreaseScrollInExcessArea = 0.2f;

    private float _currentScrollSpeed = 0.0f;
    private int _trackingTouchId = -1;
    private bool _currentlyTrackingTouch = false;

    private float _topMaxPosition = 0.0f;
    private float _bottomMaxPosition = 0.0f;
    private float _topPosition = 0.0f;
    private float _bottomPosition = 0.0f;

    public void InitializeScrollView()
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
        _currentScrollSpeed = 0.0f;
        _currentlyTrackingTouch = true;
        _trackingTouchId = id;
    }

    private void OnTouchEnded(int id)
    {
        if (id != _trackingTouchId || !_currentlyTrackingTouch) return;
        _currentlyTrackingTouch = false;
        _trackingTouchId = -1;

        StopAllCoroutines();
        _currentScrollSpeed = InputHandler.Instance.GetTouchDeltaPosition(id).y * _scrollSpeedMultiplier;
        StartCoroutine(DecreaseScrollSpeedGradually());
    }

    private bool PressIsInScrollView(Vector2 mousePos)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), mousePos, null);
    }

    private void Update()
    {
        if (_currentlyTrackingTouch)
        {
            float deltaY = InputHandler.Instance.GetTouchDeltaPosition(_trackingTouchId).y;
            ApplyScroll(deltaY);
        }
        else if (!Mathf.Approximately(_currentScrollSpeed, 0.0f))
        {
            ApplyScroll(_currentScrollSpeed * Time.deltaTime);
        }
        else CheckExcessAreas();
    }

    private void ApplyScroll(float deltaY)
    {
        if (Mathf.Approximately(deltaY, 0.0f)) return;

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

    private void CheckExcessAreas()
    {
        if (_contentParentTransform.anchoredPosition.y > _bottomPosition)
        {
            StartCoroutine(MoveBackToNonExcessArea(false));
        }
        else if (_contentParentTransform.anchoredPosition.y < _topPosition)
        {
            StartCoroutine(MoveBackToNonExcessArea(true));
        }
    }

    private bool IsInExcessArea()
    {
        return _contentParentTransform.anchoredPosition.y > _bottomPosition || _contentParentTransform.anchoredPosition.y < _topPosition;
    }

    public void ResetScrollView()
    {
        StopAllCoroutines();
        _contentParentTransform.anchoredPosition = new Vector2(_contentParentTransform.anchoredPosition.x, 0.0f);
    }

    private IEnumerator MoveBackToNonExcessArea(bool isTop)
    {
        float timer = 0.0f;
        float startPos = _contentParentTransform.anchoredPosition.y;
        float endPos = (isTop) ? _topPosition : _bottomPosition;
        while(timer < _moveBackTime)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / _moveBackTime);
            float newYPos = Mathf.SmoothStep(startPos, endPos, progress);
            _contentParentTransform.anchoredPosition = new Vector2(_contentParentTransform.anchoredPosition.x, newYPos);
            yield return null;
        }
        _contentParentTransform.anchoredPosition = new Vector2(_contentParentTransform.anchoredPosition.x, endPos);
    }

    private IEnumerator DecreaseScrollSpeedGradually()
    {
        bool isUp = _currentScrollSpeed < 0.0f;
        bool isInExcessArea = IsInExcessArea();
        bool decreasesSpeedInTime = (_scrollSpeedDecreaseAmount * _maxTimeToDecreaseScrollInExcessArea) > Mathf.Abs(_currentScrollSpeed); // Check if the speed would reach 0 in the appropriate time or not

        float amountToDecrease = Mathf.Abs(_currentScrollSpeed);
        float amountToDecreasePerSecond = amountToDecrease / _maxTimeToDecreaseScrollInExcessArea;

        while (true)
        {
            float change = 0.0f;
            if (decreasesSpeedInTime)
            {
                change = _scrollSpeedDecreaseAmount * Time.deltaTime;
            }
            else
            {
                change = amountToDecreasePerSecond * Time.deltaTime;
            }

            if (isUp) _currentScrollSpeed += change;
            else _currentScrollSpeed -= change;
            if((isUp && _currentScrollSpeed >= 0.0f) || (!isUp && _currentScrollSpeed <= 0.0f))
            {
                _currentScrollSpeed = 0.0f;
                CheckExcessAreas();
                yield break;
            }
            yield return null;
        }
    }
}