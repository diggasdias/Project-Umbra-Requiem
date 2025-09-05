using UnityEngine;

public class BeholderV1Config : MonoBehaviour
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
        Debug.Log("teste funcionou"); 
        if (health <= 0) 
        { 
            animator.SetTrigger("Die");
            Destroy(gameObject, 0.5f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("entrou no trigger");
        if (collision.CompareTag("Scythe"))
        {
            TakeDamage(1);
            animator.SetTrigger("Hit");
        }
    }
}
