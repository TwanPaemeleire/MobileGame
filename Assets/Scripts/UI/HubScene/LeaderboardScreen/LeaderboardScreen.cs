using UnityEngine;
using System.Collections.Generic;
using Unity.Services.Leaderboards.Models;
using UnityEngine.Events;
using System.Collections;

public class LeaderboardScreen : BaseScreen
{
    [SerializeField] private GameObject _UIEntryPrefab;
    [SerializeField] private Transform _entriesParent;
    [SerializeField] private string _leaderboardName;
    [SerializeField] private int _minimumAmountOfEntriesToDisplay = 10;
    [SerializeField] private RectTransform _scrollViewTransform;

    [Header("Entry appear animation")]
    [SerializeField] private float _spawnOffset = 20.0f;
    [SerializeField] private float _slideInDuration = 0.2f;
    [SerializeField] private float _fadeInDuration = 0.05f;
    [SerializeField] private float _delayBetweenEntries = 0.1f;

    private List<UILeaderboardEntry> _UIEntries = new List<UILeaderboardEntry>();

    public UnityEvent OnLeaderboardLoaded = new UnityEvent();
    private bool _isLeaderboardLoaded = false;
    private int _amountOfVisibleEntries = 0;

    private void Start()
    {
        LoadAndInitializeLeaderboard();
    }

    public override void OnScreenOpenedInternal()
    {
        if (!_isLeaderboardLoaded) return;
        StartCoroutine(AnimateAllEntries(_amountOfVisibleEntries));
    }

    private async void LoadAndInitializeLeaderboard()
    {
        List<LeaderboardEntry> entries = await UnityServicesHandler.Instance.LeaderboardHandler.GetEntries(_leaderboardName, 0, 100);
        LeaderboardEntry playerEntry = await UnityServicesHandler.Instance.LeaderboardHandler.GetPlayerEntry(_leaderboardName);

        for (int i = entries.Count - 1; i >= 0; i--)
        {
            LeaderboardEntry entry = entries[i];

            GameObject entryObject = Instantiate(_UIEntryPrefab);
            entryObject.transform.SetParent(_entriesParent, false);
            UILeaderboardEntry uiEntry = entryObject.GetComponent<UILeaderboardEntry>();
            int removeStartIndex = entry.PlayerName.LastIndexOf("#");
            uiEntry.NameText.text = entry.PlayerName.Remove(removeStartIndex);
            uiEntry.RankText.text = (entry.Rank + 1).ToString();
            uiEntry.ScoreText.text = entry.Score.ToString();
            _UIEntries.Add(uiEntry);
        }

        if(entries.Count < _minimumAmountOfEntriesToDisplay)
        {
            int amountToMake = _minimumAmountOfEntriesToDisplay - entries.Count;
            for (int i = 0; i < amountToMake; i++)
            {
                GameObject entryObject = Instantiate(_UIEntryPrefab);
                entryObject.transform.SetParent(_entriesParent, false);
                UILeaderboardEntry uiEntry = entryObject.GetComponent<UILeaderboardEntry>();
                uiEntry.NameText.text = "Vacated";
                uiEntry.RankText.text = (entries.Count + i + 1).ToString();
                uiEntry.ScoreText.text = "...";
                _UIEntries.Add(uiEntry);
            }
        }
        CalculateVisibleEntries();
        _isLeaderboardLoaded = true;
        OnLeaderboardLoaded.Invoke();
    }

    private void CalculateVisibleEntries()
    {
        float elementHeight = _UIEntries[0].GetComponent<RectTransform>().rect.height;
        float scrollViewHeight = _scrollViewTransform.rect.height;
        float amountVisible = scrollViewHeight / elementHeight;
        amountVisible++; // 1 Extra just to be safe
        _amountOfVisibleEntries = (int)amountVisible;

        for (int i = _amountOfVisibleEntries - 1; i >= 0; i--)
        {
            UILeaderboardEntry entry = _UIEntries[i];
            entry.CanvasGroup.alpha = 0.0f;
            entry.VisualsTransform.anchoredPosition = new Vector2(entry.VisualsTransform.anchoredPosition.x, 0.0f);
        }
    }

    private IEnumerator AnimateAllEntries(int amount)
    {
        int counter = 0;
        while(counter < amount)
        {
            StartCoroutine(AppearEntryAnimation(_UIEntries[amount - counter - 1]));
            ++counter;
            yield return new WaitForSeconds(_delayBetweenEntries);
        }
    }

    private IEnumerator AppearEntryAnimation(UILeaderboardEntry entry)
    {
        float timer = 0.0f;
        while(true)
        {
            timer += Time.deltaTime;

            float fadeProgress = Mathf.Clamp01(timer / _fadeInDuration);
            entry.CanvasGroup.alpha = Mathf.SmoothStep(0.0f, 1.0f, fadeProgress);

            float slideProgress = Mathf.Clamp01(timer / _slideInDuration);
            float newY = Mathf.SmoothStep(0.0f, _spawnOffset, slideProgress);
            entry.VisualsTransform.anchoredPosition = new Vector2(entry.VisualsTransform.anchoredPosition.x, newY);

            if (fadeProgress == 1.0f && slideProgress == 1.0f) yield break;
            yield return null;
        }
    }

    public override void OnScreenClosedInternal()
    {
        for (int i = _amountOfVisibleEntries - 1; i >= 0; i--)
        {
            UILeaderboardEntry entry = _UIEntries[i];
            entry.CanvasGroup.alpha = 0.0f;
            entry.VisualsTransform.anchoredPosition = new Vector2(entry.VisualsTransform.anchoredPosition.x, 0.0f);
        }
        _scrollViewTransform.GetComponent<CustomScrollView>().ResetScrollView();
    }
}