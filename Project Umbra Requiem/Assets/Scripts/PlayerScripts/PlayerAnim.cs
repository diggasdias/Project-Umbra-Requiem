using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAnim : MonoBehaviour
{
    Player pm;
    Animator anim;
    private float lastHorizontal = 0f; // Guarda a última direção horizontal

    void Start()
    {
        pm = GetComponent<Player>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        OnMove();
    }

    #region Movement

    void OnMove()
    {

        if (pm.moveDir.x != 0)
        {
            lastHorizontal = pm.moveDir.x;
        }
        anim.SetInteger("transition", pm.moveDir.sqrMagnitude > 0 ? 1 : 0);
        if (lastHorizontal > 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (lastHorizontal < 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
    #endregion
}
