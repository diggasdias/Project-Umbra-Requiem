using UnityEngine;

public class ScytheAttack : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private int damage = 1; // Valor do dano
    private Vector2 direction;

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
        Destroy(gameObject, 0.5f); // Destroi após 0.5 segundos
    }

    // Detecta colisão com inimigos
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o objeto tem um script de inimigo
        var enemy = other.GetComponent<EnemyGeneral>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject); // Destroi o ataque após acertar
        }
    }
}
