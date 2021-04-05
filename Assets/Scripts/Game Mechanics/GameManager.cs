using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public float currentHealth = 100;
    public float maxHealth = 100;
    public float regenHealth = 1;
    public static float currentStamina = 100;
    public static float maxStamina = 100;
    public float regenStamina = 1;
    public static float currentMana = 100;
    public static float maxMana = 100;
    public float regenMana = 1;

    private bool disableHealthRegen = false;
    private float disableHealthRegenTime = 1;
    public float healthRegenCooldownTime = 5f;

    private bool disableStaminaRegen = false;
    private float disableStaminaRegenTime = 1;
    public float staminaRegenCooldownTime = 5f;

    private bool disableManaRegen = false;
    private float disableManaRegenTime = 1;
    public float manaRegenCooldownTime = 5f;

    [SerializeField]
    private GameObject deathScreen;
    [SerializeField]
    private PlayerMovement playerController;
    private Camera fpsCamera;

    public GameObject currentCheckpoint;

    [SerializeField]
    private float respawnDelay = 0;
    private bool coroutineIsRunning = false;

    [SerializeField]
    private AudioSource deathSound;
    [SerializeField]
    private AudioSource respawnSound;

    public bool playerIsAlive = true;

    private Player player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        fpsCamera = playerController.GetComponentInChildren<Camera>();

        maxHealth = player.playerStats.maxHealth;
        maxStamina = player.playerStats.maxStamina;
        maxMana = player.playerStats.maxMana;

        regenHealth = player.playerStats.regenHealth;
        regenStamina = player.playerStats.regenStamina;
        regenMana = player.playerStats.regenMana;

        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentMana = maxMana;
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            if (playerIsAlive)
            {
                deathSound.Play();
            }
            playerIsAlive = false;
            deathScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            fpsCamera.gameObject.SetActive(false);
        }

        ManageHealthRegen();
        ManageManaRegen();
    }

    private void ManageManaRegen()
    {
        if (!disableManaRegen)
        {
            if (currentMana < maxMana)
            {
                currentMana += regenMana * Time.deltaTime;
            }
        }
        else
        {
            if (Time.time > disableManaRegenTime + manaRegenCooldownTime)
            {
                disableManaRegen = false;
            }
        }
    }

    private void ManageHealthRegen()
    {
        if (!disableHealthRegen)
        {
            if (currentHealth < maxHealth)
            {
                currentHealth += regenHealth * Time.deltaTime;
            }
        }
        else
        {
            if (Time.time > disableHealthRegenTime + healthRegenCooldownTime)
            {
                disableHealthRegen = false;
            }
        }
    }

    public void RespawnPlayer()
    {
        if (!coroutineIsRunning)
        {
            StartCoroutine("RespawnPlayerCo");
        }
    }

    public IEnumerator RespawnPlayerCo()
    {
        coroutineIsRunning = true;

        yield return new WaitForSeconds(respawnDelay);

        currentHealth = maxHealth;
        currentMana = maxMana;
        currentStamina = maxStamina;

        deathScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;

        playerController.transform.position = new Vector3(currentCheckpoint.transform.position.x, 
                                                currentCheckpoint.transform.position.y + 5, 
                                                currentCheckpoint.transform.position.z);
        fpsCamera.gameObject.SetActive(true);
        coroutineIsRunning = false;
        respawnSound.Play();
        playerIsAlive = true;
    }
}
