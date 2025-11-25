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

        // Atualiza a opção de upgrade (se existir) para refletir novo estado
        if (upgradeItem >= 0 && upgradeItem < weaponUpgradeOptions.Count)
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

        if (upgradeItem >= 0 && upgradeItem < passiveItemUpgradeOptions.Count)
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

    // Helpers para validar prefabs e ler dados do prefab quando possível
    private bool PrefabHasWeaponController(GameObject prefab)
    {
        if (prefab == null) return false;
        return prefab.GetComponent<WeaponController>() != null || prefab.GetComponentInChildren<WeaponController>() != null;
    }

    private WeaponScriptableObject GetWeaponDataFromPrefab(GameObject prefab)
    {
        if (prefab == null) return null;
        var wc = prefab.GetComponent<WeaponController>() ?? prefab.GetComponentInChildren<WeaponController>();
        return wc?.weaponData;
    }

    private bool PrefabHasPassiveItem(GameObject prefab)
    {
        if (prefab == null) return false;
        return prefab.GetComponent<PassiveItem>() != null || prefab.GetComponentInChildren<PassiveItem>() != null;
    }

    private PassiveItemScriptableObject GetPassiveDataFromPrefab(GameObject prefab)
    {
        if (prefab == null) return null;
        var pi = prefab.GetComponent<PassiveItem>() ?? prefab.GetComponentInChildren<PassiveItem>();
        return pi?.passiveItemData;
    }

    // Verifica se o jogador possui qualquer nível da família da arma começando em 'data'
    private bool IsWeaponFamilyOwned(WeaponScriptableObject data)
    {
        if (data == null) return false;

        // checa níveis equipados igual ao próprio data
        foreach (var w in weaponSlots)
            if (w != null && w.weaponData == data) return true;

        // percorre cadeia nextLevel a partir de data
        var visited = new HashSet<WeaponScriptableObject>();
        var current = data;
        while (current != null && !visited.Contains(current))
        {
            visited.Add(current);
            var nextPrefab = current.NextLevelPrefab;
            if (nextPrefab == null) break;
            var nextData = GetWeaponDataFromPrefab(nextPrefab);
            if (nextData == null) break;
            foreach (var w in weaponSlots)
                if (w != null && w.weaponData == nextData) return true;
            current = nextData;
        }

        return false;
    }

    // Verifica se o jogador possui qualquer nível da família do trinket começando em 'data'
    private bool IsPassiveFamilyOwned(PassiveItemScriptableObject data)
    {
        if (data == null) return false;

        foreach (var p in passiveItemSlots)
            if (p != null && p.passiveItemData == data) return true;

        var visited = new HashSet<PassiveItemScriptableObject>();
        var current = data;
        while (current != null && !visited.Contains(current))
        {
            visited.Add(current);
            var nextPrefab = current.NextLevelPrefab;
            if (nextPrefab == null) break;
            var nextData = GetPassiveDataFromPrefab(nextPrefab);
            if (nextData == null) break;
            foreach (var p in passiveItemSlots)
                if (p != null && p.passiveItemData == nextData) return true;
            current = nextData;
        }

        return false;
    }

    private bool PlayerHasWeapon(WeaponScriptableObject data)
    {
        // mantém compatibilidade: agora considera família inteira
        return IsWeaponFamilyOwned(data);
    }

    // Gera até 4 opções (não-únicas). Retorna true se pelo menos 1 opção válida foi exibida.
    bool ApplyUpgradeoptions()
    {
        if (upgradeUIOptions == null || upgradeUIOptions.Count == 0) { Debug.Log("ApplyUpgradeoptions: sem UI de upgrades"); return false; }

        // limpa antes de preencher
        RemoveUpgradeOptions();

        // Monta pools válidas: aceita entrada se houver nextLevel (upgrade para arma equipada) ou initial (spawn) mas só spawn se jogador não possuir a família da arma
        var weaponPool = new List<int>();
        for (int i = 0; i < weaponUpgradeOptions.Count; i++)
        {
            var w = weaponUpgradeOptions[i];
            if (w == null) continue;

            // Checa se há upgrade para uma arma já equipada (equipada com NextLevelPrefab)
            bool hasEquippedUpgrade = false;
            if (w.weaponData != null)
            {
                int idx = weaponSlots.FindIndex(ws => ws != null && ws.weaponData == w.weaponData);
                if (idx >= 0 && weaponSlots[idx].weaponData != null && weaponSlots[idx].weaponData.NextLevelPrefab != null)
                    hasEquippedUpgrade = true;
            }
            else
            {
                // se weaponData não preenchido, tenta obter do initial prefab
                var initData = GetWeaponDataFromPrefab(w.initialWeapon);
                if (initData != null)
                {
                    int idx2 = weaponSlots.FindIndex(ws => ws != null && ws.weaponData == initData);
                    if (idx2 >= 0 && weaponSlots[idx2].weaponData != null && weaponSlots[idx2].weaponData.NextLevelPrefab != null)
                        hasEquippedUpgrade = true;
                }
            }

            // Checa se pode spawnar (tem prefab de arma válido e jogador ainda não possui a FAMÍLIA dessa arma)
            var initialDataFromPrefab = GetWeaponDataFromPrefab(w.initialWeapon) ?? w.weaponData;
            bool canSpawnInitial = w.initialWeapon != null && PrefabHasWeaponController(w.initialWeapon) && !IsWeaponFamilyOwned(initialDataFromPrefab);

            if (hasEquippedUpgrade || canSpawnInitial)
                weaponPool.Add(i);
            else
            {
                // informação útil para debug sobre prefabs mal atribuídos
                if (w.initialWeapon != null && !PrefabHasWeaponController(w.initialWeapon))
                    Debug.LogWarning($"WeaponUpgrade[{i}] initialWeapon parece não ser um prefab de arma (falta WeaponController): {w.initialWeapon.name}");
            }
        }

        var passivePool = new List<int>();
        for (int i = 0; i < passiveItemUpgradeOptions.Count; i++)
        {
            var p = passiveItemUpgradeOptions[i];
            if (p == null) continue;

            bool hasEquippedUpgrade = false;
            if (p.passiveItemData != null)
            {
                int idx = passiveItemSlots.FindIndex(ps => ps != null && ps.passiveItemData == p.passiveItemData);
                if (idx >= 0 && passiveItemSlots[idx].passiveItemData != null && passiveItemSlots[idx].passiveItemData.NextLevelPrefab != null)
                    hasEquippedUpgrade = true;
            }
            else
            {
                var initData = GetPassiveDataFromPrefab(p.initialPassiveItem);
                if (initData != null)
                {
                    int idx2 = passiveItemSlots.FindIndex(ps => ps != null && ps.passiveItemData == initData);
                    if (idx2 >= 0 && passiveItemSlots[idx2].passiveItemData != null && passiveItemSlots[idx2].passiveItemData.NextLevelPrefab != null)
                        hasEquippedUpgrade = true;
                }
            }

            var initialDataFromPrefab = GetPassiveDataFromPrefab(p.initialPassiveItem) ?? p.passiveItemData;
            bool canSpawnInitial = p.initialPassiveItem != null && PrefabHasPassiveItem(p.initialPassiveItem) && !IsPassiveFamilyOwned(initialDataFromPrefab);

            if (hasEquippedUpgrade || canSpawnInitial)
                passivePool.Add(i);
            else
            {
                if (p.initialPassiveItem != null && !PrefabHasPassiveItem(p.initialPassiveItem))
                    Debug.LogWarning($"PassiveItemUpgrade[{i}] initialPassiveItem parece não ser trinket válido (falta PassiveItem): {p.initialPassiveItem.name}");
            }
        }

        // Se não há opções em nenhum pool, não mostra level-up
        if ((weaponPool.Count == 0) && (passivePool.Count == 0))
        {
            Debug.Log("ApplyUpgradeoptions: nenhuma opção válida disponível (todos itens no nível máximo e sem novos itens).");
            return false;
        }

        int optionsToGenerate = Mathf.Min(4, upgradeUIOptions.Count);
        var combinedAvailable = new List<(bool isWeapon, int index)>();
        foreach (var wi in weaponPool) combinedAvailable.Add((true, wi));
        foreach (var pi in passivePool) combinedAvailable.Add((false, pi));

        if (combinedAvailable.Count == 0) return false;

        for (int uiIndex = 0; uiIndex < optionsToGenerate; uiIndex++)
        {
            var ui = upgradeUIOptions[uiIndex];
            if (ui == null) continue;

            // Tenta até N vezes encontrar uma opção válida (evita mostrar versão base para arma no max level)
            bool optionAssigned = false;
            for (int attempt = 0; attempt < 12 && !optionAssigned; attempt++)
            {
                var choice = combinedAvailable[Random.Range(0, combinedAvailable.Count)];
                if (choice.isWeapon)
                {
                    var chosen = weaponUpgradeOptions[choice.index];
                    if (chosen == null) continue;

                    // procura arma equipada com esse weaponData que possua NextLevelPrefab
                    int matchedIndex = -1;
                    if (chosen.weaponData != null)
                        matchedIndex = weaponSlots.FindIndex(w => w != null && w.weaponData == chosen.weaponData && w.weaponData.NextLevelPrefab != null);
                    if (matchedIndex < 0)
                    {
                        // tenta pela prefab inicial (caso weaponData não esteja preenchido)
                        var initData = GetWeaponDataFromPrefab(chosen.initialWeapon);
                        if (initData != null)
                            matchedIndex = weaponSlots.FindIndex(w => w != null && w.weaponData == initData && w.weaponData.NextLevelPrefab != null);
                    }

                    if (matchedIndex >= 0)
                    {
                        // mostrar upgrade do próximo prefab, preferindo os dados contidos no prefab do next level
                        var nextPrefab = weaponSlots[matchedIndex].weaponData?.NextLevelPrefab ?? chosen.weaponData?.NextLevelPrefab;
                        var nextData = GetWeaponDataFromPrefab(nextPrefab);
                        if (nextData != null)
                        {
                            if (ui.upgradeDescriptionDisplay != null) ui.upgradeDescriptionDisplay.text = nextData.Description;
                            if (ui.upgradeNameDisplay != null) ui.upgradeNameDisplay.text = nextData.Name;
                            if (ui.upgradeIcon != null) ui.upgradeIcon.sprite = nextData.Icon;
                        }
                        else
                        {
                            // fallback: se o prefab não tem weaponData, mostra nome do prefab e usa icon do base (menos ideal)
                            if (nextPrefab != null)
                            {
                                if (ui.upgradeDescriptionDisplay != null) ui.upgradeDescriptionDisplay.text = chosen.weaponData != null ? chosen.weaponData.Description : "";
                                if (ui.upgradeNameDisplay != null) ui.upgradeNameDisplay.text = nextPrefab.name;
                                if (ui.upgradeIcon != null) ui.upgradeIcon.sprite = chosen.weaponData != null ? chosen.weaponData.Icon : null;
                                Debug.LogWarning($"ApplyUpgradeoptions: NextLevelPrefab '{nextPrefab.name}' não tem WeaponController.weaponData configurado. Verifique o prefab.");
                            }
                            else
                            {
                                continue; // inválido, tenta outra opção
                            }
                        }

                        int capturedSlot = matchedIndex;
                        if (ui.upgradeButton != null)
                        {
                            ui.upgradeButton.onClick.AddListener(() => LevelUpWeapon(capturedSlot, chosen.weaponUpgradeIndex));
                            ui.upgradeButton.interactable = true;
                        }

                        ui.upgradeRoot?.SetActive(true);
                        optionAssigned = true;
                    }
                    else
                    {
                        // jogador não possui: mostrar spawn da arma inicial (se houver e jogador não possuir qualquer nível da família)
                        var initial = chosen.initialWeapon;
                        var initialData = GetWeaponDataFromPrefab(initial) ?? chosen.weaponData;
                        if (initial != null && PrefabHasWeaponController(initial) && !IsWeaponFamilyOwned(initialData))
                        {
                            if (ui.upgradeButton != null) { ui.upgradeButton.onClick.AddListener(() => player?.SpawnWeapon(initial)); ui.upgradeButton.interactable = true; }
                            if (ui.upgradeDescriptionDisplay != null) ui.upgradeDescriptionDisplay.text = initialData != null ? initialData.Description : "";
                            if (ui.upgradeNameDisplay != null) ui.upgradeNameDisplay.text = initialData != null ? initialData.Name : "";
                            if (ui.upgradeIcon != null) ui.upgradeIcon.sprite = initialData != null ? initialData.Icon : null;
                            ui.upgradeRoot?.SetActive(true);
                            optionAssigned = true;
                        }
                        else
                        {
                            // inválido (já possui e sem upgrade) — tenta outra escolha
                            continue;
                        }
                    }
                }
                else // passive
                {
                    var chosen = passiveItemUpgradeOptions[choice.index];
                    if (chosen == null) continue;

                    int matchedIndex = passiveItemSlots.FindIndex(p => p != null && p.passiveItemData == chosen.passiveItemData && p.passiveItemData.NextLevelPrefab != null);
                    if (matchedIndex >= 0)
                    {
                        var nextPrefab = passiveItemSlots[matchedIndex].passiveItemData?.NextLevelPrefab ?? chosen.passiveItemData?.NextLevelPrefab;
                        var nextData = GetPassiveDataFromPrefab(nextPrefab);
                        if (nextData != null)
                        {
                            if (ui.upgradeDescriptionDisplay != null) ui.upgradeDescriptionDisplay.text = nextData.Description;
                            if (ui.upgradeNameDisplay != null) ui.upgradeNameDisplay.text = nextData.Name;
                            if (ui.upgradeIcon != null) ui.upgradeIcon.sprite = nextData.Icon;
                        }
                        else
                        {
                            if (nextPrefab != null)
                            {
                                if (ui.upgradeDescriptionDisplay != null) ui.upgradeDescriptionDisplay.text = chosen.passiveItemData != null ? chosen.passiveItemData.Description : "";
                                if (ui.upgradeNameDisplay != null) ui.upgradeNameDisplay.text = nextPrefab.name;
                                if (ui.upgradeIcon != null) ui.upgradeIcon.sprite = chosen.passiveItemData != null ? chosen.passiveItemData.Icon : null;
                                Debug.LogWarning($"ApplyUpgradeoptions: NextLevelPrefab '{nextPrefab.name}' não tem PassiveItem.passiveItemData configurado. Verifique o prefab.");
                            }
                            else
                            {
                                continue;
                            }
                        }

                        int capturedSlot = matchedIndex;
                        if (ui.upgradeButton != null)
                        {
                            ui.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(capturedSlot, chosen.passiveItemUpgradeIndex));
                            ui.upgradeButton.interactable = true;
                        }

                        ui.upgradeRoot?.SetActive(true);
                        optionAssigned = true;
                    }
                    else
                    {
                        var initial = chosen.initialPassiveItem;
                        var initialData = GetPassiveDataFromPrefab(initial) ?? chosen.passiveItemData;
                        if (initial != null && PrefabHasPassiveItem(initial) && !IsPassiveFamilyOwned(initialData))
                        {
                            if (ui.upgradeButton != null) { ui.upgradeButton.onClick.AddListener(() => player?.SpawnPassiveItem(initial)); ui.upgradeButton.interactable = true; }
                            if (ui.upgradeDescriptionDisplay != null) ui.upgradeDescriptionDisplay.text = initialData != null ? initialData.Description : "";
                            if (ui.upgradeNameDisplay != null) ui.upgradeNameDisplay.text = initialData != null ? initialData.Name : "";
                            if (ui.upgradeIcon != null) ui.upgradeIcon.sprite = initialData != null ? initialData.Icon : null;
                            ui.upgradeRoot?.SetActive(true);
                            optionAssigned = true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            } // attempts

            if (!optionAssigned)
            {
                // Se não achou opção válida para esse slot, esconder
                ui.upgradeRoot?.SetActive(false);
            }
        }

        return true;
    }

    public void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOptions();
        bool anyShown = ApplyUpgradeoptions();
        if (!anyShown)
        {
            // Fecha a tela de levelup caso não haja opções válidas
            if (GameManager.instance != null)
                GameManager.instance.EndLevelUp();
        }
    }
}
