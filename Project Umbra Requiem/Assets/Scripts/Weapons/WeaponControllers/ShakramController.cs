using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShakramController : WeaponController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedShakram = Instantiate(weaponData.Prefab);
        spawnedShakram.transform.position = transform.position;
        spawnedShakram.GetComponent<ShakramBehaviour>().DirectionChecker(pm.lastMovedVector);
    }

}
