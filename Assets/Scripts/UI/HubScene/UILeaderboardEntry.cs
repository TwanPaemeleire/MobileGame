using TMPro;
using UnityEngine;

public class UILeaderboardEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _rankText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    public TextMeshProUGUI RankText => _rankText;
    public TextMeshProUGUI NameText => _nameText;
    public TextMeshProUGUI ScoreText => _scoreText;
}
