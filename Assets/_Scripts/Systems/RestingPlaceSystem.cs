using UnityEngine;

public class RestingPlaceSystem : Singleton<RestingPlaceSystem>
{
    [SerializeField] private ParticleSystem healParticles;

    public void HealHero(PlayerRunData livePlayerData)
    {
        livePlayerData.UpdateCurrentHealth(livePlayerData.MaxHealth);
        if (healParticles != null)
        {
            healParticles.Play();
        }
    }
}