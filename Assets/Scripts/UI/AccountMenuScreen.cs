using TMPro;
using UnityEngine;

public class AccountMenuScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _playerIdText;
    [SerializeField] private TextMeshProUGUI _levelText;

    private void OnEnable()
    {
        _nameText.text = UnityServicesHandler.Instance.AuthenticationHandler.GetPlayerName();
        _playerIdText.text = UnityServicesHandler.Instance.AuthenticationHandler.GetPlayerId();
    }
}
