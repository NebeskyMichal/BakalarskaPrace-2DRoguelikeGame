public class HeroView : CombatantView
{
    public void Setup(HeroData heroData, int currHealth)
    {
        SetupBase(heroData.Health, currHealth, heroData.Image);
    }
}