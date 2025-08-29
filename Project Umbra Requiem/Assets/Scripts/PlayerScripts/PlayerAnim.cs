using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAnim : MonoBehaviour
{
    Player player;
    Animator anim;
    private float lastHorizontal = 0f; // Guarda a �ltima dire��o horizontal

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
        // Atualiza a �ltima dire��o horizontal se houver movimento
        if (player.Direction.x != 0)
            lastHorizontal = player.Direction.x;

        // Anima��o de transi��o
        anim.SetInteger("transition", player.Direction.sqrMagnitude > 0 ? 1 : 0);

        // Mant�m a orienta��o baseada na �ltima dire��o horizontal
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
