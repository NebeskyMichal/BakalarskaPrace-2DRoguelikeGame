using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPortraitUI : MonoBehaviour
{
    [SerializeField] private Image portrait;
    [SerializeField] private TMP_Text portraitText;
    [SerializeField] private Button selectButton;

    public void Setup(HeroData heroData, Action<HeroData> callback)
    {
        portrait.sprite = heroData.Image;
        portraitText.text = heroData.name;
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() => callback(heroData));
    }
}