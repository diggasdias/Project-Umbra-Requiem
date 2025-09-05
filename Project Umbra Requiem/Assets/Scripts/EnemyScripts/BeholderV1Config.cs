using UnityEngine;

public class BeholderV1Config : MonoBehaviour
{
    public int health = 3;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("teste funcionou"); 
        if (health <= 0)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(CircleCollider2D circleCollider)
    {
        if (circleCollider.CompareTag("Scythe"))
        {
            TakeDamage(1);
            animator.SetTrigger("Hit");
        }
    }
}
