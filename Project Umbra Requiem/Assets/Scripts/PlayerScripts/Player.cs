using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private Rigidbody2D rig;

    [Header("Speed Settings")]
    private float speed;
    private float initialSpeed = 3;
    private Vector2 direction;

    [Header("Scythe Attack Settings")]
    [SerializeField] private GameObject ScytheAttackPrefab;
    [SerializeField] private float ScytheAttackCooldown = 1f;
    [SerializeField] private float ScytheAttackDistance = 1f; // Distância do ataque
    private float ScytheLastAttackTime;
    [Header("Sickle Attack Settings")]
    [SerializeField] private GameObject SickleAttackPrefab;
    [SerializeField] private float SickleAttackCooldown = 1f;
    [SerializeField] private float SickleAttackDistance = 1f; // Distância do ataque
    private float SickleLastAttackTime;

    public Vector2 Direction { get => direction; set => direction = value; }
    public float LastHorizontal { get; private set; }

    void FixedUpdate()
    {
        OnMove();
    }

    void Update()
    {
        OnInput();
        ScytheAttack();
        SickleAttack();
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

    void ScytheAttack()
    {
        if (Time.time >= ScytheLastAttackTime + ScytheAttackCooldown)
        {
            Vector2 attackDir = new Vector2(Mathf.Sign(LastHorizontal), 0);

            // Calcula a posição do ataque um pouco à frente do player
            Vector3 spawnPos = transform.position + (Vector3)(attackDir * ScytheAttackDistance);

            GameObject attack = Instantiate(ScytheAttackPrefab, spawnPos, Quaternion.identity);
            ScytheAttack scythe = attack.GetComponent<ScytheAttack>();
            if (scythe != null)
                scythe.SetDirection(attackDir);

            ScytheLastAttackTime = Time.time;
        }
    }
    void SickleAttack()
    {
        if (Time.time >= SickleLastAttackTime + SickleAttackCooldown)
        {
            Vector2 attackDir = new Vector2(Mathf.Sign(LastHorizontal), 0);

            // Calcula a posição do ataque um pouco à frente do player
            Vector3 spawnPos = transform.position + (Vector3)(attackDir * SickleAttackDistance);

            GameObject attack = Instantiate(SickleAttackPrefab, spawnPos, Quaternion.identity);
            SickleAttack sickle = attack.GetComponent<SickleAttack>();
            if (sickle != null)
                sickle.SetDirection(attackDir);

            SickleLastAttackTime = Time.time;
        }
    }
    #endregion

}
