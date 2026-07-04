using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectorSystem : MonoBehaviour
{
    [SerializeField] private List<HeroData> heroDatas = new();
    [SerializeField] private Transform characterSelectorContainer;
    [SerializeField] private CharacterPortraitUI portraitUI;
    [SerializeField] private MenuNavigationSystem menuNavigationSystem;

    private void Start()
    {
        foreach (var heroData in heroDatas)
        {
            var spawnedPortrait = Instantiate(portraitUI, characterSelectorContainer);
            spawnedPortrait.Setup(heroData, menuNavigationSystem.SelectCharacterPortrait);
        }
    }
}