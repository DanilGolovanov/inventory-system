using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    private Player player;
    private int[] texturePartIndexes;

    [SerializeField]
    private string textureLocation = "Character/";

    static string[] customiseParts = { "Skin", "Hair", "Mouth", "Eyes", "Clothes", "Armour" };
    public List<Texture2D>[] textureParts = new List<Texture2D>[customiseParts.Length];

    public Renderer characterRenderer;

    [SerializeField]
    private TMPro.TextMeshProUGUI playerName;

    [SerializeField]
    private TMPro.TextMeshProUGUI maxHealthText;
    [SerializeField]
    private TMPro.TextMeshProUGUI maxStaminaText;
    [SerializeField]
    private TMPro.TextMeshProUGUI maxManaText;

    // Start is called before the first frame update
    void Start()
    {
        AddTextures();

        player = FindObjectOfType<Player>();
        PlayerData data = SaveSystem.LoadPlayerData();

        player.name = data.name;
        player.playerStats.level = data.level;

        player.playerStats.moveSpeed = data.moveSpeed;

        player.playerStats.maxHealth = data.maxHealth;
        player.playerStats.maxStamina = data.maxStamina;
        player.playerStats.maxMana = data.maxMana;

        player.playerStats.regenHealth = data.regenHealth;
        player.playerStats.regenStamina = data.regenStamina;
        player.playerStats.regenMana = data.regenMana;

        player.playerStats.healthLevelUp = data.healthLevelUp;
        player.playerStats.staminaLevelUp = data.staminaLevelUp;
        player.playerStats.manaLevelUp = data.manaLevelUp;

        player.Profession = data.profession;
        player.playerStats.baseStats = data.stats;
        player.playerStats.baseStatPoints = data.basePoints;

        texturePartIndexes = data.currentTexturePartsIndexes;

        playerName.text = data.name;
        maxHealthText.text = "Max: " + data.maxHealth.ToString();
        maxStaminaText.text = "Max: " + data.maxStamina.ToString();
        maxManaText.text = "Max: " + data.maxMana.ToString();

        SetSavedTextures();
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

    public void SetSavedTextures()
    {
        for (int partIndex = 0; partIndex < textureParts.Length; partIndex++)
        {
            int currentTextureIndex = texturePartIndexes[partIndex];

            Material[] materials = characterRenderer.materials;
            materials[partIndex].mainTexture = textureParts[partIndex][currentTextureIndex];
            characterRenderer.materials = materials;
        }
    }
}
