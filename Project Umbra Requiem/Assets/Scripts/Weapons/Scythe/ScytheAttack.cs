using UnityEngine;

public class ScytheAttack : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        // Mantém apenas o componente horizontal
        direction = new Vector2(Mathf.Sign(dir.x), 0);

        // Ajusta a rotação: 0° para direita, 180° para esquerda
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
}
