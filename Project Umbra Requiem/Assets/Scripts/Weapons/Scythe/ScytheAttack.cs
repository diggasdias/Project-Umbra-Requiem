using UnityEngine;

public class ScytheAttack : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float radius = 0.5f; // raio do ataque
    [SerializeField] private LayerMask enemyLayer; // camada dos inimigos
    [SerializeField] private Transform attackPoint; // ponto de origem do ataque
    [SerializeField] private GameObject scytheAttackPrefab; // prefab da foice

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

        // Detecta inimigos ao redor do ponto de ataque ao instanciar
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint != null ? attackPoint.position : transform.position,
            radius,
            enemyLayer
        );

        foreach (var hit in hits)
        {
            var damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }

        Destroy(gameObject, 0.5f);
    }

    public void ScytheAttackEvent()
    {
        // Instancie o prefab da foice e configure a direção, posição, etc.
        // Exemplo:
        Vector2 attackDir = new Vector2(Mathf.Sign(player.LastHorizontal), 0);
        Vector3 spawnPos = transform.position + (Vector3)(attackDir * 1f); // ajuste a distância

        GameObject attack = Instantiate(scytheAttackPrefab, spawnPos, Quaternion.identity);
        ScytheAttack scythe = attack.GetComponent<ScytheAttack>();
        if (scythe != null)
            scythe.SetDirection(attackDir);
    }

    // Visualização do raio no editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint != null ? attackPoint.position : transform.position, radius);
    }
}
