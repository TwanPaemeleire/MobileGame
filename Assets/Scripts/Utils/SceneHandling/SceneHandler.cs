using AYellowpaper.SerializedCollections;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoSingleton<SceneHandler>
{
    [SerializeField] private Animator _sceneTransitionAnimator;
    [SerializeField] private AnimationEventRedirector _animEventRedirector;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private SerializedDictionary<string, SceneTransitionData> _scenesTransitionData;
    public UnityEvent OnSceneTransitionStart = new UnityEvent();
    public UnityEvent OnSceneTransitionStartLoadAndAnimationFinished = new UnityEvent();
    public UnityEvent OnSceneTransitionNewSceneLoadFinished = new UnityEvent();
    public UnityEvent OnSceneTransitionNewSceneAnimationFinished = new UnityEvent();

    private AsyncOperation _currentSceneLoadOperation = null;
    public float SceneLoadProgress => (_currentSceneLoadOperation != null) ? _currentSceneLoadOperation.progress : 0.0f;

    private bool _currentSceneTransitionAnimationFinished = false;

    protected override void Init()
    {
        _animEventRedirector.RegisterAction("AnimFinished", OnSceneTransitionAnimationFinished);
    }

    public void RequestStartSceneTransition(string sceneName)
    {
        if (_currentSceneLoadOperation != null) return;
        StartCoroutine(SceneTransitionCoroutine(sceneName));
    }

    public void OnSceneTransitionAnimationFinished()
    {
        _currentSceneTransitionAnimationFinished = true;
    }

    private IEnumerator SceneTransitionCoroutine(string sceneName)
    {
        _canvasGroup.alpha = 1.0f;
        _canvasGroup.interactable = false;
        _currentSceneTransitionAnimationFinished = false;
        OnSceneTransitionStart.Invoke();
        _sceneTransitionAnimator.SetTrigger(_scenesTransitionData[SceneManager.GetActiveScene().name].EndAnimationTriggerName);
        _currentSceneLoadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        _currentSceneLoadOperation.allowSceneActivation = false;

        yield return new WaitUntil(() => _currentSceneTransitionAnimationFinished && SceneLoadProgress >= 0.9f);
        OnSceneTransitionStartLoadAndAnimationFinished.Invoke();
        _currentSceneLoadOperation.allowSceneActivation = true;

        yield return _currentSceneLoadOperation;
        OnSceneTransitionNewSceneLoadFinished.Invoke();

        _currentSceneLoadOperation = null;
        _currentSceneTransitionAnimationFinished = false;

        if(!_scenesTransitionData[sceneName].HasStartAnimation)
        {
            _canvasGroup.alpha = 0.0f;
            _canvasGroup.interactable = true;
            OnSceneTransitionNewSceneAnimationFinished.Invoke();
            yield break;
        }

        _sceneTransitionAnimator.SetTrigger(_scenesTransitionData[sceneName].StartAnimationTriggerName);
        yield return new WaitUntil(() => _currentSceneTransitionAnimationFinished);
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = true;
        OnSceneTransitionNewSceneAnimationFinished.Invoke();
    }
}
