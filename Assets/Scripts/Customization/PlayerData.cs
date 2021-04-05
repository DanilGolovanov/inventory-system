using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string name;
    public int level;

    public float moveSpeed;

    public float maxHealth;
    public float maxStamina;
    public float maxMana;

    public float regenHealth;
    public float regenStamina;
    public float regenMana;

    public float healthLevelUp;
    public float staminaLevelUp;
    public float manaLevelUp;

    public BaseStat[] stats;
    public int basePoints;
    public PlayerProfession profession;

    //public float[] position;
    public int[] currentTexturePartsIndexes;

    public PlayerData(Customisation player)
    {
        name = player.player.name;
        level = player.player.playerStats.level;

        moveSpeed = player.player.playerStats.moveSpeed;

        maxHealth = player.player.playerStats.maxHealth;
        maxStamina = player.player.playerStats.maxStamina;
        maxMana = player.player.playerStats.maxMana;

        regenHealth = player.player.playerStats.regenHealth;
        regenStamina = player.player.playerStats.regenStamina;
        regenMana = player.player.playerStats.regenMana;

        healthLevelUp = player.player.playerStats.healthLevelUp;
        staminaLevelUp = player.player.playerStats.staminaLevelUp;
        manaLevelUp = player.player.playerStats.manaLevelUp;

        stats = player.player.playerStats.baseStats;
        basePoints = player.player.playerStats.baseStatPoints;
        profession = player.player.Profession;

        currentTexturePartsIndexes = player.currentTexturePartsIndexes;

        //position = new float[3];
        //position[0] = player.transform.position.x;
        //position[1] = player.transform.position.y;
        //position[2] = player.transform.position.z;
    }
}
