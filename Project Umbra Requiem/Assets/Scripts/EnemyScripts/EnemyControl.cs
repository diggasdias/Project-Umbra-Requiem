using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    private Player player;
    [SerializeField] private UnityEngine.AI.NavMeshAgent agent;

    void Start()
    {
        player = FindAnyObjectByType<Player>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        agent.SetDestination(player.transform.position);
        float posX = player.transform.position.x - transform.position.y;

        if (posX > 0)
        {
            transform.eulerAngles = new Vector2(0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector2(0, 180);
        }
    }
}
