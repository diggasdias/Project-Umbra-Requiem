using UnityEngine;

public class BeholderV2Config : MonoBehaviour
{
    public int health = 3;
    private Animator animator;
    void Start()
    {
        animator = FindAnyObjectByType<Animator>();
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            animator.SetTrigger("IsDead");
            Destroy(gameObject, 0.9f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Scythe"))
        {
            animator.SetTrigger("Hit");
            TakeDamage(1);
        }
    }
}
