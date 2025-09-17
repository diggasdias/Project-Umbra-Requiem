using UnityEngine;

public class ShakramController : WeaponController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedShakram = Instantiate(prefab);
        spawnedShakram.transform.position = transform.position;
    }

}
