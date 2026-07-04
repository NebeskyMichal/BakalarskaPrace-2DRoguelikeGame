using UnityEngine;

public class HeroViewCreator : MonoBehaviour
{
    [SerializeField] private HeroView heroViewPrefab;

    public HeroView CreateHeroView(HeroData heroData, int currHealth, Vector3 position, Quaternion rotation, Transform spawnPoint)
    {
        HeroView heroView = Instantiate(heroViewPrefab, position, rotation, spawnPoint);
        heroView.Setup(heroData, currHealth);
        return heroView;
    }
}