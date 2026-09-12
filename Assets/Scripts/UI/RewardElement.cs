using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private Image _iconImage;
    [SerializeField] private float _popScale = 1.1f;
    [SerializeField] private float _popScaleUpTime = 0.1f;
    [SerializeField] private float _popScaleDownTime = 0.4f;

    public TextMeshProUGUI AmountText => _amountText;
    public Image IconImage => _iconImage;

    public void DoPopAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(PopAnimation());
    }

    private IEnumerator PopAnimation()
    {
        yield return ChangeScale(1.0f, _popScale, _popScaleUpTime);
        yield return ChangeScale(_popScale, 1.0f, _popScaleDownTime);
    }

    private IEnumerator ChangeScale(float from, float to, float duration)
    {
        float timer = 0.0f;
        RectTransform rect = GetComponent<RectTransform>();
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float newValue = Mathf.SmoothStep(from, to, timer / duration);
            rect.localScale = new Vector3(newValue, newValue, newValue);
            yield return null;
        }
        rect.localScale = new Vector3(to, to, to);
    }
}
