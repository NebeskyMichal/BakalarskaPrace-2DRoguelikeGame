using UnityEngine;

public class SettingsInMap : MonoBehaviour
{
    [SerializeField] private GameObject settingsUIPanel;

    public void OpenSettings()
    {
        settingsUIPanel.SetActive(true);
        LegendUI.Instance.HideLegend();
        RunManagerSystem.Instance.AreSettingsOpen = true;
    }

    public void CloseSettings()
    {
        RunManagerSystem.Instance.AreSettingsOpen = false;
        settingsUIPanel.SetActive(false);
        if (RunManagerSystem.Instance.CurrentEncounter == null)
        {
            LegendUI.Instance.ShowLegend();   
        }
    }

    public void LeaveToMenu()
    {
        RunManagerSystem.Instance.ReturnToMenu();
    }
}