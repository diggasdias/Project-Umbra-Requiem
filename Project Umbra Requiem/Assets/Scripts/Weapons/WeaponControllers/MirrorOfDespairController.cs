using UnityEditor;
using UnityEngine;

public class MirrorOfDespairController : WeaponController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedMirror = Instantiate(weaponData.Prefab);
        spawnedMirror.transform.position = transform.position;
        spawnedMirror.transform.parent = transform;
    }

}
 