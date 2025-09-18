using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
public class WeaponScriptableObject : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;
    public GameObject Prefab { get => prefab; private set => prefab = value; }

    float damage;
    public float Damage { get => damage; private set => damage = value; }

    float speed;
    public float Speed { get => speed; private set => speed = value; }

    float cooldownDuration;
    public float CooldownDuration { get => cooldownDuration; private set => cooldownDuration = value; }

    public int pierce;
    public int Pierce { get => pierce; private set => pierce = value; }
}
