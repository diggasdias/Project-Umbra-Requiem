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
        float posX = player.transform.position.x - player.transform.position.y;
        agent.SetDestination(player.transform.position);

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
