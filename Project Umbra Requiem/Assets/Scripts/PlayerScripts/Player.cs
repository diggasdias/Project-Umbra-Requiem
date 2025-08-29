using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private Rigidbody2D rig;

    [Header("Speed Settings")]
    private float speed;
    private float initialSpeed = 3;
    private Vector2 direction;

    [Header("Attack Settings")]
    [SerializeField] private GameObject scytheAttackPrefab;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float attackDistance = 1f; // Distância do ataque
    private float lastAttackTime;

    public Vector2 Direction { get => direction; set => direction = value; }
    public float LastHorizontal { get; private set; }

    void FixedUpdate()
    {
        OnMove();
    }

    void Update()
    {
        OnInput();
        HandleAttack();
    }

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        speed = initialSpeed; 
    }

    #region Movement

    void OnInput()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (direction.x != 0)
            LastHorizontal = direction.x;
    }

    void OnMove()
    {
        rig.MovePosition(rig.position + Direction * speed * Time.fixedDeltaTime);
    }

    void HandleAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Vector2 attackDir = new Vector2(Mathf.Sign(LastHorizontal), 0);

            // Calcula a posição do ataque um pouco à frente do player
            Vector3 spawnPos = transform.position + (Vector3)(attackDir * attackDistance);

            GameObject attack = Instantiate(scytheAttackPrefab, spawnPos, Quaternion.identity);
            ScytheAttack scythe = attack.GetComponent<ScytheAttack>();
            if (scythe != null)
                scythe.SetDirection(attackDir);

            lastAttackTime = Time.time;
        }
    }
    #endregion

}
