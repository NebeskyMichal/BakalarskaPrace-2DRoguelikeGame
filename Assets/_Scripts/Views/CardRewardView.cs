using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardRewardView : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text mana;
    [SerializeField] private Image imageSource;
    [SerializeField] private Image backgroundSource;

    public static event Action<CardData> onRewardCardSelected;

    private CardData _cardData;

    public void Setup(CardData data)
    {
        _cardData = data;
        title.text = data.name;
        description.text = data.Description;
        mana.text = data.Mana.ToString();
        imageSource.sprite = data.Image;
        backgroundSource.sprite = data.CardBackground;
    }

    public void OnSelectRewardClicked()
    {
        RunManagerSystem.Instance.AddCardToDeck(_cardData);
        gameObject.SetActive(false);
        onRewardCardSelected?.Invoke(_cardData);
    }
}