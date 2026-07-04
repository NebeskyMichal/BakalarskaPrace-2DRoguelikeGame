using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopBarManagerSystem : Singleton<TopBarManagerSystem>
{
    [SerializeField] private TMP_Text floorNumberText;
    [SerializeField] private TMP_Text characterText;
    [SerializeField] private Image chosenHeroPortrait;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text coinStatus;
    
    private void Start()
    {
        UpdateUI();
    }
    
    public void UpdateUI()
    {
        PlayerRunData data = RunManagerSystem.Instance.PlayerData;
        if (data == null) return;
        floorNumberText.text = $"Floor {data.CurrentFloor}";
        characterText.text = data.HeroName;
        chosenHeroPortrait.sprite = data.HeroPortrait;
        healthText.text = $"{data.CurrentHealth}/{data.MaxHealth}";
        coinStatus.text = data.CurrentGold.ToString();
    }
}