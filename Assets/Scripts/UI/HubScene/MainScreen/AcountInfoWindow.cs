using TMPro;
using UnityEngine;

public class AcountInfoWindow : PopUpWindow
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _playerIdText;
    protected override void OnWindowOpenedInternal()
    {
        _nameText.text = UnityServicesHandler.Instance.AuthenticationHandler.GetPlayerNameWithoutId();
        _playerIdText.text = UnityServicesHandler.Instance.AuthenticationHandler.GetPlayerId();
    }
}
