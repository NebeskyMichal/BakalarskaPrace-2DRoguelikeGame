using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntentUI : MonoBehaviour
{
    [SerializeField] private Image intentImage;

    [SerializeField] private Sprite attackIntentSprite,
        defendIntentSprite,
        healIntentSprite,
        buffIntentSprite,
        debuffIntentSPrite;

    [SerializeField] private TMP_Text intentText;


    public void Set(IntentType intentType, int intentAmount)
    {
        Sprite sprite = GetSpriteByType(intentType);
        intentImage.sprite = sprite;
        intentText.text = intentAmount.ToString();
        intentText.gameObject.SetActive(intentAmount > 0);
    }

    private Sprite GetSpriteByType(IntentType intentType)
    {
        return intentType switch
        {
            IntentType.Attack => attackIntentSprite,
            IntentType.Defend => defendIntentSprite,
            IntentType.Heal => healIntentSprite,
            IntentType.Buff => buffIntentSprite,
            IntentType.Debuff => debuffIntentSPrite,
            _ => null
        };
    }
}