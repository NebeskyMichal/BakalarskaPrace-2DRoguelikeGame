using System.Collections.Generic;
using UnityEngine;

public class Card
{
    private readonly CardData _data;
    public string Title => _data.name;

    public string Description => _data.Description;

    public Sprite Image => _data.Image;

    public Sprite CardBackground => _data.CardBackground;

    public Effect ManualTargetEffect => _data.ManualTargetEffect;

    public List<AutoTargetEffect> OtherEffects => _data.OtherEffects;

    public int Mana { get; private set; }

    public Card(CardData cardData)
    {
        _data = cardData;
        Mana = cardData.Mana;
    }
}