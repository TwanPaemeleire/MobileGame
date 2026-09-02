using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TransformEntry
{
    public RectTransform Transform;
    public BaseScreen Screen;
    public RectTransform BottomIconTransform;
    public CanvasGroup CanvasGroup;
    [HideInInspector] public Vector2 OriginalPos = Vector2.zero;
    [HideInInspector] public Vector2 CurrentScreenPosition = Vector2.zero;
}

public class MainSceneScreenSwitcher : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<string, TransformEntry> _screenTransforms;
    [SerializeField] private float _transitionTime = 0.5f;
    [SerializeField] private string _startTransform;
    [SerializeField] private RectTransform _selectedScreenIndicator;
    bool _isSwitchingScreens = false;

    private TransformEntry _currentTransform = null;

    private void Start()
    {
        _currentTransform = _screenTransforms[_startTransform];
        foreach (var entry in _screenTransforms)
        {
            entry.Value.CanvasGroup.blocksRaycasts = false;
            entry.Value.CanvasGroup.interactable = false;
            entry.Value.OriginalPos = entry.Value.Transform.anchoredPosition;
            entry.Value.CurrentScreenPosition = entry.Value.OriginalPos;
        }
        _currentTransform.CanvasGroup.interactable = true;
        _currentTransform.CanvasGroup.blocksRaycasts = true;
    }

    public void StartScreenSwitch(string newScreen)
    {
        if (_isSwitchingScreens || !_screenTransforms.ContainsKey(newScreen) || _currentTransform == _screenTransforms[newScreen]) return;
        StartCoroutine(SwitchScreens(newScreen));
    }

    private IEnumerator SwitchScreens(string newScreen)
    {
        _isSwitchingScreens = true;
        float timer = 0.0f;
        Vector2 posChange = _currentTransform.CurrentScreenPosition - _screenTransforms[newScreen].CurrentScreenPosition;

        float inidcatorStartX = _selectedScreenIndicator.anchoredPosition.x;
        float indicatorTargetX = _screenTransforms[newScreen].BottomIconTransform.anchoredPosition.x;

        _currentTransform.CanvasGroup.interactable = false;
        _currentTransform.CanvasGroup.blocksRaycasts = false;

        while (timer < _transitionTime)
        {
            timer += Time.deltaTime;
            float progress = timer / _transitionTime;
            float smoothProgress = Mathf.SmoothStep(0.0f, 1.0f, progress);
            _selectedScreenIndicator.anchoredPosition = new Vector2(Mathf.SmoothStep(inidcatorStartX, indicatorTargetX, progress), _selectedScreenIndicator.anchoredPosition.y);
            foreach (var entry in _screenTransforms)
            {
                entry.Value.Transform.anchoredPosition = Vector2.Lerp(entry.Value.CurrentScreenPosition, entry.Value.CurrentScreenPosition + posChange, smoothProgress);
            }
            yield return null;
        }

        foreach (var entry in _screenTransforms)
        {
            entry.Value.Transform.anchoredPosition = entry.Value.CurrentScreenPosition + posChange;
            entry.Value.CurrentScreenPosition = entry.Value.Transform.anchoredPosition;
        }
        _selectedScreenIndicator.anchoredPosition = new Vector2(indicatorTargetX, _selectedScreenIndicator.anchoredPosition.y);

        _currentTransform.Screen.OnScreenClosedInternal();
        _currentTransform.Screen.OnScreenClosed.Invoke();
        _currentTransform = _screenTransforms[newScreen];
        _currentTransform.CanvasGroup.interactable = true;
        _currentTransform.CanvasGroup.blocksRaycasts = true;
        _currentTransform.Screen.OnScreenOpenedInternal();
        _currentTransform.Screen.OnScreenOpened.Invoke();
        _isSwitchingScreens = false;
    }
}