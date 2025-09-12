using UnityEngine;

public class GreenXp : MonoBehaviour
{
    private Player player;
    [SerializeField] private int xpValue = 5; // Valor de XP da gema azul
     
    void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.AddXP(xpValue);
            Destroy(gameObject);
        }
    }
}
