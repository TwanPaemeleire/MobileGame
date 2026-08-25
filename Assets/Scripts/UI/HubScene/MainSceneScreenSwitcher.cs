using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TransformEntry
{
    public RectTransform Transform;
    [HideInInspector] public Vector2 OriginalPos = Vector2.zero;
    [HideInInspector] public Vector2 CurrentScreenPosition = Vector2.zero;
}

public class MainSceneScreenSwitcher : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<string, TransformEntry> _screenTransforms;
    [SerializeField] private float _transitionTime = 0.5f;
    [SerializeField] private TransformEntry _currentTransform;
    bool _isSwitchingScreens = false;

    private void Start()
    {
        foreach (var entry in _screenTransforms)
        {
            entry.Value.OriginalPos = entry.Value.Transform.anchoredPosition;
            entry.Value.CurrentScreenPosition = entry.Value.OriginalPos;
        }
    }

    public void StartScreenSwitch(string newScreen)
    {
        if (_isSwitchingScreens || _currentTransform == _screenTransforms[newScreen]) return;
        StartCoroutine(SwitchScreens(newScreen));
    }

    private IEnumerator SwitchScreens(string newScreen)
    {
        _isSwitchingScreens = true;
        float timer = 0.0f;
        Vector2 posChange = _currentTransform.CurrentScreenPosition - _screenTransforms[newScreen].CurrentScreenPosition;

        while (timer < _transitionTime)
        {
            timer += Time.deltaTime;
            float progress = timer / _transitionTime;
            float smoothProgress = Mathf.SmoothStep(0.0f, 1.0f, progress);
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

        _currentTransform = _screenTransforms[newScreen];
        _isSwitchingScreens = false;
    }
}