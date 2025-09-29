using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;
public class InventoryManager : MonoBehaviour
{
    public List <WeaponController> weaponSlots = new List <WeaponController> (6);
    public int[] weaponsLevels = new int [6];
    public List <PassiveItem> passiveItemSlots = new List <PassiveItem> (6);
    public int[] passiveItemsLevels = new int [6];


    public void AddWeapon(int SlotIndex , WeaponController weapon)
    {
        weaponSlots[SlotIndex] = weapon;
    }

    public void AddPassiveItem(int SlotIndex , PassiveItem passiveItem)
    {
        passiveItemSlots[SlotIndex] = passiveItem;
    }

    public void LevelUpWeapon(int slotIndex)
    {

    }

    public void LevelUpPassiveItem(int slotIndex)
    {

    }
}
