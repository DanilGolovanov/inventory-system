﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string name;
    public PlayerStats playerStats;

    [SerializeField]
    private Customisation customisation;

    //-----PROTOTYPE
    //private bool disableRegen = false;
    //private float disableRegenTime;
    //public float regenCooldownTime = 5f;

    private PlayerProfession profession;

    public InventoryObject inventory;   

    public PlayerProfession Profession { get => profession; set => ChangeProfession(value); }

    private void Start()
    {
        inventory.Load();
    }

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<GroundItem>();
        if (item)
        {
            inventory.AddItem(new Item(item.item), 1);
            inventory.Save();
            Destroy(other.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Items = new InventorySlot[42];
    }

    //-----PROTOTYPE

    public void LevelUp()
    {
        playerStats.level++;
        playerStats.baseStatPoints += 3;

        for (int i = 0; i < playerStats.baseStats.Length; i++)
        {
            playerStats.baseStats[i].levelUpStat += 1;
        }

        if (customisation != null)
        {
            customisation.UpdateStats();
        }
    }

    public void ChangeProfession(PlayerProfession profession)
    {
        this.profession = profession;
        SetupProfession();
    }

    public void SetupProfession()
    {
        for (int i = 0; i < playerStats.baseStats.Length; i++)
        {
            if (i < profession.defaultStats.Length)
            {
                playerStats.baseStats[i].defaultStat = profession.defaultStats[i].defaultStat;
            }
        }
    }

    //public void DealDamage(float damage)
    //{
    //    playerStats.CurrentHealth -= damage;
    //    disableRegen = true;
    //    disableRegenTime = Time.time;
    //}

    //public float testHealth = 100;

    //public int health = 100;
    //public int level = 5;

    ////for testing purposes
    //public void Save()
    //{
    //    SaveSystem.SavePlayer(this);
    //}

    //public void Load()
    //{
    //    PlayerData data = SaveSystem.LoadPlayer();

    //    level = data.level;
    //    health = data.health;
    //    Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
    //    transform.position = position;
    //}

    //private void Update()
    //{
    //    //playerStats.CurrentHealth = testHealth;
    //    if (!disableRegen)
    //    {
    //        if (playerStats.CurrentHealth < playerStats.maxHealth)
    //        {
    //            playerStats.CurrentHealth += playerStats.regenHealth * Time.deltaTime;
    //        }
    //    }
    //    else
    //    {
    //        if (Time.time > disableRegenTime + regenCooldownTime)
    //        {
    //            disableRegen = false;
    //        }
    //    }
    //}

    //private void OnGUI()
    //{
    //    if (GUI.Button(new Rect(150, 10, 120, 20), "Level Up"))
    //    {
    //        LevelUp();
    //    }

    //    if (GUI.Button(new Rect(150, 40, 120, 20), "Do Damage: " + playerStats.CurrentHealth))
    //    {
    //        DealDamage(25f);
    //    }
    //}
}
