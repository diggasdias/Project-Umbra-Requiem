using UnityEngine;

public class ScytheAttack : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float radius = 0.5f; // raio do ataque
    [SerializeField] private LayerMask enemyLayer; // camada dos inimigos
    [SerializeField] private Transform attackPoint; // ponto de origem do ataque

    private Vector2 direction;
    private Player player;

    public void SetDirection(Vector2 dir)
    {
        direction = new Vector2(Mathf.Sign(dir.x), 0);

        if (direction.x > 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (direction.x < 0)
            transform.rotation = Quaternion.Euler(0, 0, 180);
    }

    void Update()
    {
        if (direction.x != 0)
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    void Start()
    {
        player = FindAnyObjectByType<Player>();
        Destroy(gameObject, 0.5f);
    }

    public void ScytheAttackEvent()
    {
     
    }

    // Visualização do raio no editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint != null ? attackPoint.position : transform.position, radius);
    }
}
