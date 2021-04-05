using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores the player's stats.
/// </summary>
[System.Serializable]
public class PlayerStats 
{
    [Header("Player Stats")]
    public float speed = 6f;
    public float sprintSpeed = 12f;
    public float crouchSpeed = 3f;
    public float gravity = -9.81f;
    public float jumpHeight = 1f;
                    
    public int level;
    
    [Header("Current Stats")]
    private float currentHealth;
    public float maxHealth = 100f;
    public float currentMana = 100f;
    public float maxMana = 100f;
    public float currentStamina = 100f;
    public float maxStamina = 100f;

    public float regenHealth = 5f;
    public float regenStamina;
    public float regenMana;

    public float healthLevelUp;
    public float staminaLevelUp;
    public float manaLevelUp;

    public float moveSpeed;

    public QuarterHearts healthHearts;

    [Header("Base Stats")]
    public int baseStatPoints = 10;
    public BaseStat[] baseStats;

    public float CurrentHealth 
    { 
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            
            if (healthHearts != null)
            {
                healthHearts.UpdateHearts(value, maxHealth);
            }
        }
    }

    public bool SetStats(int statIndex, int amount)
    {
        if (amount > 0 && baseStatPoints - amount < 0) // can't add point if there are none left
        {
            return false;
        }
        else if (amount < 0 && baseStats[statIndex].additionalStat + amount < 0) // we can't change default stats
        {
            return false;
        }

        // change stat
        baseStats[statIndex].additionalStat += amount;
        baseStatPoints -= amount;

        return true;
    }

    public void UpdateHealthStats(int constitution)
    {
        maxHealth = constitution * 7;
        regenHealth = constitution / 10;
        healthLevelUp = constitution / 5;
    }

    public void UpdateStaminaStats(int strength)
    {
        maxStamina = strength * 7;
        regenStamina = strength / 5;
        staminaLevelUp = strength / 5;
    }

    public void UpdateManaStats(int wisdom)
    {
        maxMana = wisdom * 7;
        regenMana = wisdom / 10;
        manaLevelUp = wisdom / 5;
    }

    public void UpdateMoveSpeed(int dexterity)
    {
        moveSpeed = dexterity / 1.5f;
    }
}

[System.Serializable]
public struct BaseStat
{
    public string baseStatName;
    public int defaultStat;
    public int levelUpStat;
    public int additionalStat;

    public int finalStat { get => defaultStat + additionalStat + levelUpStat; }
}


