using UnityEngine;


[CreateAssetMenu(fileName = "Weapon Data", menuName = "2D Top-down Rogue-like/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public Sprite icon;
    public int maxLevel;

    [HideInInspector] public string behaviour;
    public Weapon.Stats baseStats;
    public Weapon.Stats[] linearGrowth;
    public Weapon.Stats[] randomGrowth;

    public Weapon.Stats GetLevelData(int level)
    {
        if(level - 2 < linearGrowth.Length)
            return linearGrowth[level - 2];

        if(randomGrowth.Length > 0)
            return randomGrowth[Random.Range(0, randomGrowth.Length)];

        Debug.LogWarning(string.Format("Weapon doesn't have its level up stats configured for level {0}!", level));
        return new Weapon.Stats();
    }       
}
