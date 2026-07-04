using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CombatantView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private StatusEffectsUI statusEffectsUI;
    [SerializeField] private HealthBarUI healthBarUI;


    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public int CurrentBonusDamage { get; set; }
    public int CurrentScalingCounter { get; set; }

    private Dictionary<StatusEffectType, int> _statusEffects = new();

    protected void SetupBase(int health, int currHealth, Sprite image)
    {
        MaxHealth = health;
        CurrentHealth = currHealth;
        spriteRenderer.sprite = image;
        healthBarUI.UpdateHealthBar(CurrentHealth, MaxHealth);
    }

    public void Damage(int damagedForAmount)
    {
        int remainingDamage = damagedForAmount;
        int currArmor = GetStatusEffectStacks(StatusEffectType.Armor);
        if (currArmor > 0)
        {
            if (currArmor >= damagedForAmount)
            {
                RemoveStatusEffect(StatusEffectType.Armor, remainingDamage);
                remainingDamage = 0;
            }
            else if (currArmor < damagedForAmount)
            {
                RemoveStatusEffect(StatusEffectType.Armor, currArmor);
                remainingDamage -= currArmor;
            }
        }

        if (remainingDamage > 0)
        {
            CurrentHealth -= remainingDamage;
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                CombatantDeathGameAction deathAction = new(this);
                ActionSystem.Instance.AddReaction(deathAction);
            }
        }

        transform.DOShakePosition(0.2f, 0.5f).SetLink(gameObject);
        healthBarUI.UpdateHealthBar(CurrentHealth, MaxHealth);
    }

    public void Heal(int healForAmount)
    {
        CurrentHealth += healForAmount;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }

        healthBarUI.UpdateHealthBar(CurrentHealth, MaxHealth);
    }

    public void AddStatusEffect(StatusEffectType type, int stackCount)
    {
        if (_statusEffects.ContainsKey(type))
        {
            _statusEffects[type] += stackCount;
        }
        else
        {
            _statusEffects.Add(type, stackCount);
        }

        statusEffectsUI.UpdateStatusEffectUI(type, GetStatusEffectStacks(type));
    }

    public void RemoveStatusEffect(StatusEffectType type, int stackCount)
    {
        if (_statusEffects.ContainsKey(type))
        {
            _statusEffects[type] -= stackCount;
            if (_statusEffects[type] <= 0)
            {
                _statusEffects.Remove(type);
            }
        }

        statusEffectsUI.UpdateStatusEffectUI(type, GetStatusEffectStacks(type));
    }

    public int GetStatusEffectStacks(StatusEffectType type)
    {
        if (_statusEffects.ContainsKey(type))
        {
            return _statusEffects[type];
        }

        return 0;
    }
}