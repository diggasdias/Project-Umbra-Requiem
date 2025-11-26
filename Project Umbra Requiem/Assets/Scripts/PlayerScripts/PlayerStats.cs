using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObject characterData;

    //Status atuais
    float currentHealth;
    float currentRecovery;
    float currentMoveSpeed;
    float currentMight;
    float currentProjectileSpeed;
    float currentMagnet;

    #region Current Statss Properties
    public float CurrentHealth 
    {
        get { return currentHealth; }
        set
        {
            if (currentHealth != value)
            {
                currentHealth = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
                }
            }
        }
    }

    public float CurrentRecovery
    {
        get { return currentRecovery; }
        set
        {
            if (currentRecovery != value)
            {
                currentRecovery = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + CurrentRecovery;
                }
            }
        }
    }

    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set
        {
            if (currentMoveSpeed != value)
            {
                currentMoveSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedDisplay.text = "MoveSpeed: " + CurrentMoveSpeed;
                }
            }
        }
    }

    public float CurrentMight
    {
        get { return currentMight; }
        set
        {
            if (currentMight != value)
            {
                currentMight = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMightDisplay.text = "Might: " + CurrentMight;
                }
            }
        }
    }

    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed; }
        set
        {
            if (currentProjectileSpeed != value)
            {
                currentProjectileSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedDisplay.text = "Prjectile Speed: " + currentProjectileSpeed;
                }
            }
        }
    }

    public float CurrentMagnet
    {
        get { return currentMagnet; }
        set
        {
            if (currentMagnet != value)
            {
                currentMagnet = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;
                }
            }
        }
    }
    #endregion

    // Nivel e Xp do player
    [Header("Nivel/Experiencia")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;
    

    //Classe para definir a faixa de nivel e o tanto de xp necessário para subir
    [System.Serializable]
    public class LevelRange 
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    //I-Frames
    public float invincibilityDuration;
     float invincibilityTimer;
     bool isInvincible;

    public List<LevelRange> levelRanges;

    InventoryManager inventory;
    public int weaponIndex;
    public int passiveItemIndex;


    [Header("UI")]
    public Image healthBar;
    public Image expBar;
    public Text levelText;
    void Awake()
    {
        inventory = GetComponent<InventoryManager>();

        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroSingleton();

        // Status iniciais
        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;
        currentProjectileSpeed = characterData.ProjectileSpeed;
        currentMagnet = characterData.Magnet;

        //Spawna arma inicial
        SpawnWeapon(characterData.StartingWeapon);
        //SpawnWeapon(secondWeapontest);
        //SpawnPassiveItem(firstPassiveItemTest);
        //SpawnPassiveItem(secondPassiveItemTest);
    }       

    void Start()
    {
        experienceCap = levelRanges[0].experienceCapIncrease;

        GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
        GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
        GameManager.instance.currentMoveSpeedDisplay.text = "MoveSpeed: " + currentMoveSpeed;
        GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
        GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed;
        GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;

        GameManager.instance.AssignChosenCharacterUI(characterData);

        UpdateHealthBar();
        UpdateExpBar();
        UpdateLevelText();
    }

    void Update()
    {
       if(invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
       else if (isInvincible)
        {
            isInvincible = false;
        }

        Recover();
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;
        LevelUpChecker();

        UpdateExpBar();
    }

    void LevelUpChecker()
    {
        if(experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;

            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;

            UpdateLevelText();

            GameManager.instance.StartLevelUp();
        }

        
    }

    void UpdateExpBar()
    {
        // Update exp bar fill amount
        expBar.fillAmount = (float)experience / experienceCap;
    }

    void UpdateLevelText()
    {
        // Update level text
        levelText.text = "LV. " + level;
    }

    public void TakeDamage(float dmg)
    {
        if (!isInvincible)
        {
            currentHealth -= dmg;

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (currentHealth <= 0)
            {
                Kill();
            }
        }
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / characterData.MaxHealth;
        }
    }

    public void Kill()
    {
        if (!GameManager.instance.isGameOver)
        {
            GameManager.instance.AssignLevelReachedUI(level);
            GameManager.instance.GameOver();
            GameManager.instance.AssignChosenWeaponsAndPassiveItemsUI(inventory.weaponUISlots, inventory.weaponUISlots);
        }
    }

    public void RestoreHealth(float amount)
    {
        if (currentHealth < characterData.MaxHealth)
        {
            currentHealth += amount;

            if (currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }

    void Recover()
    {
        if (currentHealth < characterData.MaxHealth)
        {
            currentHealth += currentRecovery * Time.deltaTime;
        }

        if (currentHealth > characterData.MaxHealth)
        {
            currentHealth = characterData.MaxHealth;
        }
    }

    public void SpawnWeapon(GameObject weapon)
    {
        //Checando se o inventário tá cheio
        if (weaponIndex >= inventory.weaponSlots.Count - 1)
        {
            Debug.LogError("Inventory slots are full");
            return;
        }

        if (weapon == null)
        {
            Debug.LogError("SpawnWeapon: prefab null");
            return;
        }

        // Verifica se o prefab é um prefab de arma (contém WeaponController em raiz ou nos filhos)
        var prefabWeaponCtrl = weapon.GetComponent<WeaponController>() ?? weapon.GetComponentInChildren<WeaponController>();
        if (prefabWeaponCtrl == null)
        {
            Debug.LogError($"SpawnWeapon: o prefab '{weapon.name}' não contém WeaponController. Parece ser um projétil em vez de um prefab de arma.");
            return;
        }

        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform); //Seta a arma como um filho do player

        var spawnedWeaponCtrl = spawnedWeapon.GetComponent<WeaponController>() ?? spawnedWeapon.GetComponentInChildren<WeaponController>();
        if (spawnedWeaponCtrl == null)
        {
            Debug.LogError($"SpawnWeapon: instância de '{weapon.name}' não contém WeaponController após instanciar.");
            Destroy(spawnedWeapon);
            return;
        }

        inventory.AddWeapon(weaponIndex, spawnedWeaponCtrl); //Adiciona a arma para o slot certo no inventário

        weaponIndex++;
    }

    public void SpawnPassiveItem(GameObject passiveItem)
    {
        //Checando se o inventário tá cheio
        if (passiveItemIndex >= inventory.passiveItemSlots.Count - 1)
        {
            Debug.LogError("Inventory slots are full");
            return;
        }

        if (passiveItem == null)
        {
            Debug.LogError("SpawnPassiveItem: prefab null");
            return;
        }

        var prefabPassive = passiveItem.GetComponent<PassiveItem>() ?? passiveItem.GetComponentInChildren<PassiveItem>();
        if (prefabPassive == null)
        {
            Debug.LogError($"SpawnPassiveItem: o prefab '{passiveItem.name}' não contém PassiveItem. Verifique o prefab.");
            return;
        }

        //Spawna o trinket inicial
        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform); //Seta a arma como um filho do player

        var spawnedPassiveCtrl = spawnedPassiveItem.GetComponent<PassiveItem>() ?? spawnedPassiveItem.GetComponentInChildren<PassiveItem>();
        if (spawnedPassiveCtrl == null)
        {
            Debug.LogError($"SpawnPassiveItem: instância de '{passiveItem.name}' não contém PassiveItem após instanciar.");
            Destroy(spawnedPassiveItem);
            return;
        }

        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveCtrl); //Adiciona a arma para o slot certo no inventário

        passiveItemIndex++;
    }
}
