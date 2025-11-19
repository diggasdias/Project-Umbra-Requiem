using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public int slotsCount = 6;

    public List<WeaponController> weaponSlots = new List<WeaponController>();
    public int[] weaponLevels;
    public List<Image> weaponUISlots = new List<Image>();

    public List<PassiveItem> passiveItemSlots = new List<PassiveItem>();
    public int[] passiveItemLevels;
    public List<Image> passiveItemUISlots = new List<Image>();

    [System.Serializable]
    public class WeaponUpgrade
    {
        public int weaponUpgradeIndex;
        public GameObject initialWeapon;
        public WeaponScriptableObject weaponData;
    }

    [System.Serializable]
    public class PassiveItemUpgrade
    {
        public int passiveItemUpgradeIndex;
        public GameObject initialPassiveItem;
        public PassiveItemScriptableObject passiveItemData;
    }

    [System.Serializable]
    public class UpgradeUI
    {
        public Text upgradeNameDisplay;
        public Text upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;
        // Root do elemento de UI (painel que contém os componentes). Atribua no Inspector.
        public GameObject upgradeRoot;
    }

    public List<WeaponUpgrade> weaponUpgradeOptions = new List<WeaponUpgrade>();
    public List<PassiveItemUpgrade> passiveItemUpgradeOptions = new List<PassiveItemUpgrade>();
    public List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>();

    PlayerStats player;

    void Awake()
    {
        // Inicializa arrays e garante tamanho mínimo das listas para evitar IndexOutOfRange
        weaponLevels = new int[slotsCount];
        passiveItemLevels = new int[slotsCount];

        EnsureListSize(weaponSlots, slotsCount);
        EnsureListSize(weaponUISlots, slotsCount);
        EnsureListSize(passiveItemSlots, slotsCount);
        EnsureListSize(passiveItemUISlots, slotsCount);
    }

    void Start()
    {
        player = GetComponent<PlayerStats>() ?? FindAnyObjectByType<PlayerStats>();
        if (player == null)
            Debug.LogError("InventoryManager: PlayerStats não encontrado na cena.");
    }

    void EnsureListSize<T>(List<T> list, int size)
    {
        if (list == null) return;
        while (list.Count < size) list.Add(default);
    }

    bool IsValidSlot(int index) => index >= 0 && index < slotsCount;

    public void AddWeapon(int SlotIndex, WeaponController weapon)
    {
        if (!IsValidSlot(SlotIndex)) { Debug.LogWarning($"AddWeapon: slot inválido {SlotIndex}"); return; }

        weaponSlots[SlotIndex] = weapon;
        weaponLevels[SlotIndex] = (weapon != null && weapon.weaponData != null) ? weapon.weaponData.Level : 0;

        var img = weaponUISlots[SlotIndex];
        if (img != null && weapon != null && weapon.weaponData != null)
        {
            img.enabled = true;
            img.sprite = weapon.weaponData.Icon;
        }

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
            GameManager.instance.EndLevelUp();
    }

    public void AddPassiveItem(int SlotIndex, PassiveItem passiveItem)
    {
        if (!IsValidSlot(SlotIndex)) { Debug.LogWarning($"AddPassiveItem: slot inválido {SlotIndex}"); return; }

        passiveItemSlots[SlotIndex] = passiveItem;
        passiveItemLevels[SlotIndex] = (passiveItem != null && passiveItem.passiveItemData != null) ? passiveItem.passiveItemData.Level : 0;

        var img = passiveItemUISlots[SlotIndex];
        if (img != null && passiveItem != null && passiveItem.passiveItemData != null)
        {
            img.enabled = true;
            img.sprite = passiveItem.passiveItemData.Icon;
        }

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
            GameManager.instance.EndLevelUp();
    }

    public void LevelUpWeapon(int slotIndex, int upgradeItem)
    {
        if (!IsValidSlot(slotIndex)) { Debug.LogWarning($"LevelUpWeapon: slot inválido {slotIndex}"); return; }

        var weapon = weaponSlots[slotIndex];
        if (weapon == null || weapon.weaponData == null) { Debug.LogWarning($"LevelUpWeapon: sem arma no slot {slotIndex}"); return; }

        var nextPrefab = weapon.weaponData.NextLevelPrefab;
        if (nextPrefab == null) { Debug.LogError($"NO NEXT LEVEL FOR {weapon.name}"); return; }

        var upgradeWeapon = Instantiate(nextPrefab, transform.position, Quaternion.identity);
        upgradeWeapon.transform.SetParent(transform);
        var newWeaponCtrl = upgradeWeapon.GetComponent<WeaponController>();
        if (newWeaponCtrl == null) { Debug.LogError("LevelUpWeapon: prefab não contém WeaponController"); Destroy(upgradeWeapon); return; }

        AddWeapon(slotIndex, newWeaponCtrl);
        Destroy(weapon.gameObject);
        weaponLevels[slotIndex] = newWeaponCtrl.weaponData != null ? newWeaponCtrl.weaponData.Level : weaponLevels[slotIndex];

        weaponUpgradeOptions[upgradeItem].weaponData = upgradeWeapon.GetComponent<WeaponController>().weaponData;

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
            GameManager.instance.EndLevelUp();
    }

    public void LevelUpPassiveItem(int slotIndex, int upgradeItem)
    {
        if (!IsValidSlot(slotIndex)) { Debug.LogWarning($"LevelUpPassiveItem: slot inválido {slotIndex}"); return; }

        var passive = passiveItemSlots[slotIndex];
        if (passive == null || passive.passiveItemData == null) { Debug.LogWarning($"LevelUpPassiveItem: sem item no slot {slotIndex}"); return; }

        var nextPrefab = passive.passiveItemData.NextLevelPrefab;
        if (nextPrefab == null) { Debug.LogError($"NO NEXT LEVEL FOR {passive.name}"); return; }

        var upgrade = Instantiate(nextPrefab, transform.position, Quaternion.identity);
        upgrade.transform.SetParent(transform);
        var newPassive = upgrade.GetComponent<PassiveItem>();
        if (newPassive == null) { Debug.LogError("LevelUpPassiveItem: prefab não contém PassiveItem"); Destroy(upgrade); return; }

        AddPassiveItem(slotIndex, newPassive);
        Destroy(passive.gameObject);
        passiveItemLevels[slotIndex] = newPassive.passiveItemData != null ? newPassive.passiveItemData.Level : passiveItemLevels[slotIndex];

        passiveItemUpgradeOptions[upgradeItem].passiveItemData = upgrade.GetComponent<PassiveItem>().passiveItemData;

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
            GameManager.instance.EndLevelUp();
    }

    void RemoveUpgradeOptions()
    {
        if (upgradeUIOptions == null) return;
        foreach (var opt in upgradeUIOptions)
        {
            if (opt == null) continue;
            if (opt.upgradeButton != null) { opt.upgradeButton.onClick.RemoveAllListeners(); opt.upgradeButton.interactable = false; }
            if (opt.upgradeNameDisplay != null) opt.upgradeNameDisplay.text = "";
            if (opt.upgradeDescriptionDisplay != null) opt.upgradeDescriptionDisplay.text = "";
            if (opt.upgradeIcon != null) opt.upgradeIcon.sprite = null;
            if (opt.upgradeRoot != null) opt.upgradeRoot.SetActive(false); // esconde a opção por padrão
        }
    }

    void ApplyUpgradeoptions()
    {
        if (upgradeUIOptions == null || upgradeUIOptions.Count == 0) { Debug.Log("ApplyUpgradeoptions: sem UI de upgrades"); return; }

        // limpa antes de preencher
        RemoveUpgradeOptions();

        bool hasWeapons = weaponUpgradeOptions != null && weaponUpgradeOptions.Count > 0;
        bool hasPassives = passiveItemUpgradeOptions != null && passiveItemUpgradeOptions.Count > 0;
        if (!hasWeapons && !hasPassives) { Debug.LogWarning("ApplyUpgradeoptions: nenhuma opção de upgrade disponível"); return; }

        for (int uiIndex = 0; uiIndex < upgradeUIOptions.Count; uiIndex++)
        {
            var ui = upgradeUIOptions[uiIndex];
            if (ui == null) continue;

            int upgradeType;
            if (hasWeapons && hasPassives) upgradeType = Random.Range(1, 3); // 1 ou 2
            else if (hasWeapons) upgradeType = 1;
            else upgradeType = 2;

            if (upgradeType == 1)
            {
                var chosen = weaponUpgradeOptions[Random.Range(0, weaponUpgradeOptions.Count)];
                if (chosen == null || chosen.weaponData == null) continue;

                int matchedIndex = weaponSlots.FindIndex(w => w != null && w.weaponData == chosen.weaponData);
                if (matchedIndex >= 0)
                {
                    int capturedSlot = matchedIndex;
                    if (ui.upgradeButton != null) { ui.upgradeButton.onClick.AddListener(() => LevelUpWeapon(capturedSlot, chosen.weaponUpgradeIndex)); ui.upgradeButton.interactable = true; }

                    var nextPrefab = chosen.weaponData.NextLevelPrefab;
                    if (nextPrefab != null)
                    {
                        var wc = nextPrefab.GetComponent<WeaponController>();
                        if (wc != null && wc.weaponData != null)
                        {
                            if (ui.upgradeDescriptionDisplay != null) ui.upgradeDescriptionDisplay.text = wc.weaponData.Description;
                            if (ui.upgradeNameDisplay != null) ui.upgradeNameDisplay.text = wc.weaponData.Name;
                        }
                    }
                    else
                    {
                        if (ui.upgradeDescriptionDisplay != null) ui.upgradeDescriptionDisplay.text = chosen.weaponData.Description;
                        if (ui.upgradeNameDisplay != null) ui.upgradeNameDisplay.text = chosen.weaponData.Name;
                    }
                }
                else
                {
                    var initial = chosen.initialWeapon;
                    if (ui.upgradeButton != null) { ui.upgradeButton.onClick.AddListener(() => player?.SpawnWeapon(initial)); ui.upgradeButton.interactable = true; }
                    if (ui.upgradeDescriptionDisplay != null) ui.upgradeDescriptionDisplay.text = chosen.weaponData.Description;
                    if (ui.upgradeNameDisplay != null) ui.upgradeNameDisplay.text = chosen.weaponData.Name;
                }

                if (ui.upgradeIcon != null) ui.upgradeIcon.sprite = chosen.weaponData.Icon;
                if (ui.upgradeRoot != null) ui.upgradeRoot.SetActive(true); // mostra a opção preenchida
            }
            else // passive
            {
                var chosen = passiveItemUpgradeOptions[Random.Range(0, passiveItemUpgradeOptions.Count)];
                if (chosen == null || chosen.passiveItemData == null) continue;

                int matchedIndex = passiveItemSlots.FindIndex(p => p != null && p.passiveItemData == chosen.passiveItemData);
                if (matchedIndex >= 0)
                {
                    int capturedSlot = matchedIndex;
                    if (ui.upgradeButton != null) { ui.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(capturedSlot, chosen.passiveItemUpgradeIndex)); ui.upgradeButton.interactable = true; }

                    var nextPrefab = chosen.passiveItemData.NextLevelPrefab;
                    if (nextPrefab != null)
                    {
                        var pi = nextPrefab.GetComponent<PassiveItem>();
                        if (pi != null && pi.passiveItemData != null)
                        {
                            if (ui.upgradeDescriptionDisplay != null) ui.upgradeDescriptionDisplay.text = pi.passiveItemData.Description;
                            if (ui.upgradeNameDisplay != null) ui.upgradeNameDisplay.text = pi.passiveItemData.Name;
                        }
                    }
                    else
                    {
                        if (ui.upgradeDescriptionDisplay != null) ui.upgradeDescriptionDisplay.text = chosen.passiveItemData.Description;
                        if (ui.upgradeNameDisplay != null) ui.upgradeNameDisplay.text = chosen.passiveItemData.Name;
                    }
                }
                else
                {
                    var initial = chosen.initialPassiveItem;
                    if (ui.upgradeButton != null) { ui.upgradeButton.onClick.AddListener(() => player?.SpawnPassiveItem(initial)); ui.upgradeButton.interactable = true; }
                    if (ui.upgradeDescriptionDisplay != null) ui.upgradeDescriptionDisplay.text = chosen.passiveItemData.Description;
                    if (ui.upgradeNameDisplay != null) ui.upgradeNameDisplay.text = chosen.passiveItemData.Name;
                }

                if (ui.upgradeIcon != null) ui.upgradeIcon.sprite = chosen.passiveItemData.Icon;
                if (ui.upgradeRoot != null) ui.upgradeRoot.SetActive(true); // mostra a opção preenchida
            }
        }
    }

    public void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeoptions();
    }
}
