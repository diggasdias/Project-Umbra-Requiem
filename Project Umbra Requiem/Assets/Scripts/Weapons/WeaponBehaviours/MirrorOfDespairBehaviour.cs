using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MirrorOfDespairBehavior : MeleeBehaviour
{
    List<GameObject> markedEnemies;

   protected override void Start()
   {
       base.Start();
       markedEnemies = new List<GameObject>();
    }
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") && !markedEnemies.Contains(col.gameObject))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage());

            markedEnemies.Add(col.gameObject);
        }
    }
}
