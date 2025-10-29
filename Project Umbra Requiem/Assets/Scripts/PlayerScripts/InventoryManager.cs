using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.UI;
public class InventoryManager : MonoBehaviour
{
    public List <WeaponController> weaponSlots = new List <WeaponController> (6);
    public int[] weaponLevels = new int [6];
    public List<Image> weaponUISlots = new List<Image>(6);
    public List <PassiveItem> passiveItemSlots = new List <PassiveItem> (6);
    public int[] passiveItemLevels = new int [6];
    public List<Image> passiveItemUISlots = new List<Image>(6);

    public void AddWeapon(int SlotIndex , WeaponController weapon) //Adiciona uma arma para um slot específico
    {
        weaponSlots[SlotIndex] = weapon;
        weaponLevels[SlotIndex] = weapon.weaponData.Level;
        weaponUISlots[SlotIndex].enabled = true;
        weaponUISlots[SlotIndex].sprite = weapon.weaponData.Icon;
    }

    public void AddPassiveItem(int SlotIndex , PassiveItem passiveItem) //Adiciona um trinket para um slot específico
    {
        passiveItemSlots[SlotIndex] = passiveItem;
        passiveItemLevels[SlotIndex] = passiveItem.passiveItemData.Level;
        weaponUISlots[SlotIndex].enabled = true;
        passiveItemUISlots[SlotIndex].sprite = passiveItem.passiveItemData.Icon;
    }

    public void LevelUpWeapon(int slotIndex)
    {
        if (weaponSlots.Count > slotIndex)
        {
            WeaponController weapon = weaponSlots[slotIndex];
            if (!weapon.weaponData.NextLevelPrefab)
            {
                Debug.LogError("NO NEXT LEVEL FOR " + weapon.name);
                return;
            }
            GameObject upgradeWeapon = Instantiate(weapon.weaponData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradeWeapon.transform.SetParent(transform); //Seta como filho do player
            AddWeapon(slotIndex, upgradeWeapon.GetComponent<WeaponController>());
            Destroy(weapon.gameObject);
            weaponLevels[slotIndex] = upgradeWeapon.GetComponent<WeaponController>().weaponData.Level; //Certifica que este é o nivel correto do item
        }
    }

    public void LevelUpPassiveItem(int slotIndex)
    {
        if (passiveItemSlots.Count > slotIndex)
        {
            PassiveItem passiveItem = passiveItemSlots[slotIndex];
            if (!passiveItem.passiveItemData.NextLevelPrefab)
            {
                Debug.LogError("NO NEXT LEVEL FOR " + passiveItem.name);
                return;
            }
            GameObject upgradePassiveItem = Instantiate(passiveItem.passiveItemData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradePassiveItem.transform.SetParent(transform); //Seta como filho do player
            AddPassiveItem(slotIndex, upgradePassiveItem.GetComponent<PassiveItem>()); 
            Destroy(passiveItem.gameObject);
            passiveItemLevels[slotIndex] = upgradePassiveItem.GetComponent<PassiveItem>().passiveItemData.Level; //Certifica que este é o nivel correto do item
        }
    }
}
