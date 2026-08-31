using TMPro;
using UnityEngine;

public class UILeaderboardEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _rankText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private RectTransform _visualsTransform;
    [SerializeField] private CanvasGroup _canvasGroup;

    public TextMeshProUGUI RankText => _rankText;
    public TextMeshProUGUI NameText => _nameText;
    public TextMeshProUGUI ScoreText => _scoreText;
    public RectTransform VisualsTransform => _visualsTransform;
    public CanvasGroup CanvasGroup => _canvasGroup;
}
