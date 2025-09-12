using UnityEngine;

public class GreenXp : MonoBehaviour
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
    }
}
