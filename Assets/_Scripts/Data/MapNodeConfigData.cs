using UnityEngine;

[CreateAssetMenu(menuName = "Map/Icon Config")]
public class MapIconConfig : ScriptableObject
{
    [SerializeField] private Sprite basicEncounter, eliteEncounter, bossEncounter, shopEncounter, restEncounter;

    public Sprite GetSprite(EncounterType type) => type switch
    {
        EncounterType.Basic => basicEncounter,
        EncounterType.Elite => eliteEncounter,
        EncounterType.Boss => bossEncounter,
        EncounterType.Rest => restEncounter,
        EncounterType.Shop => shopEncounter,
        _ => null
    };
}