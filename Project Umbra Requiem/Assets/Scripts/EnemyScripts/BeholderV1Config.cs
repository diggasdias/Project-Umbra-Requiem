using UnityEngine;

public class BeholderV1Config : MonoBehaviour, IDamageable
{
    public int health = 5;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        anim.SetTrigger("Hit"); // Toca animação de dano
        if (health <= 0)
        {
            anim.SetTrigger("IsDead");
            Destroy(gameObject, 1f); // Aguarda animação antes de destruir
        }
    }
}