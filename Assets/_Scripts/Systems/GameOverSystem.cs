using TMPro;
using UnityEngine;

public class GameOverSystem : Singleton<GameOverSystem>
{
    [SerializeField] private GameObject gameOverUIPanel;

    [SerializeField]
    private TMP_Text killCountText, goldCountText, floorCountText, damageDoneText, gameCompletionText, title;

    public void FillWithStats(bool didPlayerWin, PlayerRunData playerRunData, PlayerRunStatistics playerRunStatistics)
    {
        killCountText.text = playerRunStatistics.EnemiesKilled.ToString();
        goldCountText.text = playerRunStatistics.GoldEarned.ToString();
        floorCountText.text = playerRunData.CurrentFloor.ToString();
        damageDoneText.text = playerRunStatistics.DamageDoneTotal.ToString();
        if (!didPlayerWin)
        {
            title.text = "You died, try again!";
            gameCompletionText.text = "Still alive :(";
        }
        else
        {
            title.text = "You won! Congratulations!";
            gameCompletionText.text = "Conquered!";
        }
    }

    public void OpenGameOver(bool didPlayerWin, PlayerRunData playerRunData, PlayerRunStatistics playerRunStatistics)
    {
        gameOverUIPanel.SetActive(true);
        FillWithStats(didPlayerWin, playerRunData, playerRunStatistics);
    }

    public void CloseGameOver()
    {
        gameOverUIPanel.SetActive(false);
        RunManagerSystem.Instance.ReturnToMenu();
    }
}