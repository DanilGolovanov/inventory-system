using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Customisation : MonoBehaviour
{
    public Player player;

    [SerializeField]
    private PlayerProfession[] playerProfessions;

    [SerializeField]
    private string textureLocation = "Character/";

    static string[] customiseParts = { "Skin", "Hair", "Mouth", "Eyes", "Clothes", "Armour" };

    public List<Texture2D>[] textureParts = new List<Texture2D>[customiseParts.Length];
    public int[] currentTexturePartsIndexes = new int[customiseParts.Length];

    public Renderer characterRenderer;

    public Vector2 scrollPosition = Vector2.zero;

    [SerializeField]
    private Text professionDescText;

    [SerializeField]
    private Text pointsText;

    [SerializeField]
    private Text[] stats;

    public InputField playerNameText;

    private void Start()
    {
        AddTextures();

        if (player == null)
        {
            Debug.LogError("player in Customisation is null");
        }

        if (playerProfessions != null && playerProfessions.Length > 0)
        {
            player.Profession = playerProfessions[0];
        }

        pointsText.text = "Points: " + player.playerStats.baseStatPoints;
    }

    public void AddTextures()
    {
        for (int i = 0; i < customiseParts.Length; i++)
        {
            int count = 0;

            Texture2D tempTexture;
            textureParts[i] = new List<Texture2D>();
            do
            {
                tempTexture = (Texture2D)Resources.Load(textureLocation + customiseParts[i] + "_" + count);
                if (tempTexture != null)
                {
                    textureParts[i].Add(tempTexture);
                }
                count++;
            } while (tempTexture != null);
        }
    }

    void SetTexture(string type, int direction)
    {
        int partIndex = GetTypeIndex(type);

        int max = textureParts[partIndex].Count;

        int currentTexture = currentTexturePartsIndexes[partIndex];
        currentTexture += direction;
        if (currentTexture < 0)
        {
            currentTexture = max - 1;
        }
        else if (currentTexture > max - 1)
        {
            currentTexture = 0;
        }
        currentTexturePartsIndexes[partIndex] = currentTexture;

        Material[] materials = characterRenderer.materials;
        materials[partIndex].mainTexture = textureParts[partIndex][currentTexture];
        characterRenderer.materials = materials;
    }

    private int GetTypeIndex(string type)
    {
        int partIndex = 0;

        switch (type)
        {
            case "Skin":
                partIndex = 0;
                break;
            case "Hair":
                partIndex = 1;
                break;
            case "Mouth":
                partIndex = 2;
                break;
            case "Eyes":
                partIndex = 3;
                break;
            case "Clothes":
                partIndex = 4;
                break;
            case "Armour":
                partIndex = 5;
                break;
        }

        return partIndex;
    }

    public void SetRandomTexture()
    {
        for (int partIndex = 0; partIndex < textureParts.Length; partIndex++)
        {
            int max = textureParts[partIndex].Count;

            int currentTexture = UnityEngine.Random.Range(0, max);
            currentTexturePartsIndexes[partIndex] = currentTexture;

            Material[] materials = characterRenderer.materials;
            materials[partIndex].mainTexture = textureParts[partIndex][currentTexture];
            characterRenderer.materials = materials;
        }
    }

    public void SetTextureLeftButton(string type)
    {
        SetTexture(type, -1);
    }

    public void SetTextureRightButton(string type)
    {
        SetTexture(type, 1);
    }

    public void ResetTextures()
    {
        for (int partIndex = 0; partIndex < textureParts.Length; partIndex++)
        {
            int currentTexture = 0;
            currentTexturePartsIndexes[partIndex] = currentTexture;

            Material[] materials = characterRenderer.materials;
            materials[partIndex].mainTexture = textureParts[partIndex][currentTexture];
            characterRenderer.materials = materials;
        }
    }

    private int GetProfessionIndex(string professionName)
    {
        int professionIndex = 0;

        switch (professionName)
        {
            case "Barbarian":
                professionIndex = 0;
                break;
            case "Monk":
                professionIndex = 1;
                break;
            case "Scholar":
                professionIndex = 2;
                break;
        }

        return professionIndex;
    }

    public void SetProfession(string professionName)
    {
        player.Profession = playerProfessions[GetProfessionIndex(professionName)];

        professionDescText.text = player.Profession.professionName + "\n" + player.Profession.abilityName + "\n" + player.Profession.abilityDescription;
        
        UpdateStats();
    }

    public void UpdateStats()
    {
        pointsText.text = "Points: " + player.playerStats.baseStatPoints;
        for (int i = 0; i < player.playerStats.baseStats.Length; i++)
        {
            stats[i].text = player.playerStats.baseStats[i].baseStatName + ": " + player.playerStats.baseStats[i].finalStat.ToString();
        }
    }

    public void SetStatLeftButton(string statName)
    {
        int statIndex = GetStatIndex(statName);

        player.playerStats.SetStats(statIndex, -1);

        stats[statIndex].text = player.playerStats.baseStats[statIndex].baseStatName + ": " + player.playerStats.baseStats[statIndex].finalStat;

        pointsText.text = "Points: " + player.playerStats.baseStatPoints;
    }

    public void SetStatRightButton(string statName)
    {
        int statIndex = GetStatIndex(statName);

        player.playerStats.SetStats(statIndex, 1);

        stats[statIndex].text = player.playerStats.baseStats[statIndex].baseStatName + ": " + player.playerStats.baseStats[statIndex].finalStat;

        pointsText.text = "Points: " + player.playerStats.baseStatPoints;
    }

    private int GetStatIndex(string statName)
    {
        int statIndex = 0;

        switch (statName)
        {
            case "Strength":
                statIndex = 0;
                break;
            case "Dexterity":
                statIndex = 1;
                break;
            case "Constitution":
                statIndex = 2;
                break;
            case "Wisdom":
                statIndex = 3;
                break;
            case "Intelligence":
                statIndex = 4;
                break;
            case "Charisma":
                statIndex = 5;
                break;
        }

        return statIndex;
    }
    
    public void ChangePlayerName(string input)
    {
        player.name = input;
    }

    public void SaveAndPlay()
    {
        player.playerStats.UpdateHealthStats(player.playerStats.baseStats[GetStatIndex("Constitution")].finalStat);
        player.playerStats.UpdateStaminaStats(player.playerStats.baseStats[GetStatIndex("Strength")].finalStat);
        player.playerStats.UpdateManaStats(player.playerStats.baseStats[GetStatIndex("Wisdom")].finalStat);
        player.playerStats.UpdateMoveSpeed(player.playerStats.baseStats[GetStatIndex("Dexterity")].finalStat);

        SaveSystem.SavePlayerData(this);
        SceneManager.LoadScene("SampleScene");
    }

    //-----------IMGUI Prototype

    //private void OnGUI()
    //{
    //    //CustomiseOnGUI();
    //    StatsOnGUI();
    //    //ProfessionsOnGUI();
    //}

    //private void ProfessionsOnGUI()
    //{
    //    int currentHeight = 0;

    //    GUI.Box(new Rect(Screen.width - 170, 230, 155, 80), "Professions");

    //    scrollPosition = GUI.BeginScrollView(new Rect(Screen.width - 170, 250, 155, 50), scrollPosition, new Rect(0, 0, 100, 30 * playerProfessions.Length));

    //    for (int i = 0; i < playerProfessions.Length; i++)
    //    {
    //        if (GUI.Button(new Rect(20, currentHeight + i * 30, 100, 20), playerProfessions[i].professionName))
    //        {
    //            player.Profession = playerProfessions[i];
    //        }
    //    }

    //    GUI.EndScrollView();

    //    GUI.Box(new Rect(Screen.width - 170, Screen.height - 90, 155, 80), "Display");
    //    GUI.Label(new Rect(Screen.width - 140, Screen.height - 90 + 30, 100, 20), player.Profession.professionName);
    //    GUI.Label(new Rect(Screen.width - 140, Screen.height - 90 + 45, 100, 20), player.Profession.abilityName);
    //    GUI.Label(new Rect(Screen.width - 140, Screen.height - 90 + 60, 100, 20), player.Profession.abilityDescription);
    //}

    //private void StatsOnGUI()
    //{
    //    float currentHeight = 40;
    //    GUI.Box(new Rect(Screen.width - 170, 10, 155, 210), "Stats: " + player.playerStats.baseStatPoints);
    //    for (int i = 0; i < player.playerStats.baseStats.Length; i++)
    //    {
    //        BaseStats stat = player.playerStats.baseStats[i];

    //        if (GUI.Button(new Rect(Screen.width - 165, currentHeight + i * 30, 20, 20), "-"))
    //        {
    //            player.playerStats.SetStats(i, -1);
    //        }

    //        GUI.Label(new Rect(Screen.width - 140, currentHeight + i * 30, 100, 20), stat.baseStatName + ": " + stat.finalStat);

    //        if (GUI.Button(new Rect(Screen.width - 40, currentHeight + i * 30, 20, 20), "+"))
    //        {
    //            player.playerStats.SetStats(i, 1);
    //        }
    //    }
    //}

    //private void CustomiseOnGUI()
    //{
    //    GUI.Box(new Rect(10, 10, 120, 210), "Visuals");
    //    int currentHeight = 40;

    //    for (int i = 0; i < names.Length; i++)
    //    {
    //        if (GUI.Button(new Rect(20, currentHeight + i * 30, 20, 20), "<"))
    //        {
    //            SetTexture(names[i], -1);
    //        }

    //        GUI.Label(new Rect(45, currentHeight + i * 30, 60, 20), names[i]);

    //        if (GUI.Button(new Rect(100, currentHeight + i * 30, 20, 20), ">"))
    //        {
    //            SetTexture(names[i], 1);

    //        }
    //    }
    //}


}


