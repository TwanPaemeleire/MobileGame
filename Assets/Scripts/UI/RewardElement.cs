using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private Image _iconImage;

    public TextMeshProUGUI AmountText => _amountText;
    public Image IconImage => _iconImage;
}
