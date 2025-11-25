using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShakramBehaviour : ProjectileWeaponBehaviour
{

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        transform.position += weaponData.Speed * Time.deltaTime * direction;
    }

}
