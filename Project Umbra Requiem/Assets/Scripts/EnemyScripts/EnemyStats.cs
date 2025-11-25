using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    //Status atuais
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;

    public float despawnDistance = 20f;
    Transform player;

    EnemySpawner es;

    void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
    }

    void Start()
    {
        player = FindAnyObjectByType<PlayerStats>().transform;
        es = FindAnyObjectByType<EnemySpawner>();
    }

    void Update()
    {
        ReturnEnemy();   
    }

    public void TakeDamage(float dmg, Vector3 source)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            kill();
        }
    }

    public void kill()
    {
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);
        }
    }

    void OnDestroy()
    {
        es.OnEnemyKilled();
    }

    void ReturnEnemy()
    {
        if (Vector3.Distance(transform.position, player.position) > despawnDistance)
        {
            transform.position = player.transform.position + es.relativeSpawnPoints[Random.Range(0, es.relativeSpawnPoints.Count)].position;
        }
    }
}
