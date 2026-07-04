using TMPro;
using UnityEngine;

public class MenuNavigationSystem : MonoBehaviour
{
    [SerializeField] private Transform mainMenuContainer;
    [SerializeField] private Transform characterSelectorContainer;
    [SerializeField] private Transform settingsContainter;
    [SerializeField] private TMP_InputField seedInput;

    public void GoToCharacterSelect()
    {
        mainMenuContainer.gameObject.SetActive(false);
        characterSelectorContainer.gameObject.SetActive(true);
    }

    public void GoToSettings()
    {
        mainMenuContainer.gameObject.SetActive(false);
        settingsContainter.gameObject.SetActive(true);
    }

    public void GoToMainMenu()
    {
        characterSelectorContainer.gameObject.SetActive(false);
        settingsContainter.gameObject.SetActive(false);
        mainMenuContainer.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SelectCharacterPortrait(HeroData chosenCharacter)
    {
        RunManagerSystem.Instance.StartRun(chosenCharacter, seedInput.text);
    }
}