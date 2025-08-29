using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAnim : MonoBehaviour
{
    Player player;
    Animator anim;
    private float lastHorizontal = 0f; // Guarda a última direção horizontal

    void Start()
    {
        player = GetComponent<Player>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        OnMove();
    }

    #region Movement

    void OnMove()
    {
        // Atualiza a última direção horizontal se houver movimento
        if (player.Direction.x != 0)
            lastHorizontal = player.Direction.x;

        // Animação de transição
        anim.SetInteger("transition", player.Direction.sqrMagnitude > 0 ? 1 : 0);

        // Mantém a orientação baseada na última direção horizontal
        if (lastHorizontal > 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else if (lastHorizontal < 0)
            transform.eulerAngles = new Vector3(0, 0, 0);
    }
    #endregion

    #region Attack
    public void OnAttack()
    {
        anim.SetTrigger("Attack");
    }
    #endregion
}
