using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image healthBarFillImage;
    [SerializeField] private Image healthBarTrailingImage;
    [SerializeField] private float trailDelay = 0.4f;
    [SerializeField] private TMP_Text healthBarText;


    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthBarText.text = $"{currentHealth}/{maxHealth}";

        float ratio = ((float)currentHealth / maxHealth) * 0.83f;


        Sequence sequence = DOTween.Sequence().SetLink(gameObject);

        sequence.Append(healthBarFillImage.DOFillAmount(ratio, 0.25f)).SetEase(Ease.InOutSine);
        sequence.AppendInterval(trailDelay);
        sequence.Append(healthBarTrailingImage.DOFillAmount(ratio, 0.3f)).SetEase(Ease.InOutSine);

        sequence.Play();
    }
}