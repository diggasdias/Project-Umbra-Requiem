using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAnim : MonoBehaviour
{
    Player player;
    Animator anim;
    
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
        if (player.Direction.sqrMagnitude > 0)
        {
            anim.SetInteger("transition", 1);
        }
        else
        {
            anim.SetInteger("transition", 0);

        }

        if (player.Direction.x > 0)
        {
            transform.eulerAngles = new Vector2(0, 180);
        }
        else if (player.Direction.x < 0)
        {
            transform.eulerAngles = new Vector2(0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector2(0, 180);
        }

    }
    #endregion
}
