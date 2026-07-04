public class LegendUI : Singleton<LegendUI>
{
    public void HideLegend()
    {
        gameObject.SetActive(false);
    }

    public void ShowLegend()
    {
        gameObject.SetActive(true);
    }
}