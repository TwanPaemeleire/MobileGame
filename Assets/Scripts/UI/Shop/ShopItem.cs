using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Image _image;
    public TextMeshProUGUI TitleText => _titleText;
    public TextMeshProUGUI DescriptionText => _descriptionText;
    public TextMeshProUGUI PriceText => _priceText;
    public Image Image => _image;
}
