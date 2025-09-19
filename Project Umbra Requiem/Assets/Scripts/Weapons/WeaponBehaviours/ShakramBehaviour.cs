using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShakramBehaviour : ProjectileWeaponBehaviour
{

    ShakramController sc;

    protected override void Start()
    {
        base.Start();
        sc = FindAnyObjectByType<ShakramController>();
    }

    void Update()
    {
        transform.position += sc.speed * Time.deltaTime * direction;
    }

}
