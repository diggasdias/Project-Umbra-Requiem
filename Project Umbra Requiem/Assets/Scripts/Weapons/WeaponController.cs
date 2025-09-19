using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
    [Header("Stats")]
    public WeaponScriptableObject weaponData;
    float currentCooldown;

    protected Player pm;
    protected virtual void Start()
    {
        pm = GetComponentInParent<Player>();
        currentCooldown = weaponData.CooldownDuration;
    }

    protected virtual private void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0f)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        currentCooldown = weaponData.CooldownDuration;
    }
}
