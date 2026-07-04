using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardShopView : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text mana;
    [SerializeField] private Image imageSource;
    [SerializeField] private Image backgroundSource;
    [SerializeField] private TMP_Text buyButtonText;


    private CardData _cardData;
    private int _goldCost;

    public void Setup(CardData data, int goldCost)
    {
        _cardData = data;
        title.text = data.name;
        description.text = data.Description;
        mana.text = data.Mana.ToString();
        imageSource.sprite = data.Image;
        backgroundSource.sprite = data.CardBackground;
        _goldCost = goldCost;
        buyButtonText.text = goldCost.ToString();
    }

    public void OnBuyButtonClicked()
    {
        if (RunManagerSystem.Instance.PlayerData.SpendGold(_goldCost))
        {
            RunManagerSystem.Instance.AddCardToDeck(_cardData);
            gameObject.SetActive(false);
        }
    }
}