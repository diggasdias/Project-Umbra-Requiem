using UnityEngine;

public class BeholderV1Config : MonoBehaviour, IDamageable
{
    public int health = 3;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("teste funcionou"); 
        if (health <= 0)
            Destroy(gameObject);
    }
}
