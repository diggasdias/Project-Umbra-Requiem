using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private PlayerStats player;

    [Header("Speed Settings")]
    private Vector2 direction;

    //Referenciando
    private Rigidbody2D rig;

    public Vector2 Direction { get => direction; set => direction = value; }
    public float LastHorizontal { get; private set; }

    void FixedUpdate()
    {
        OnMove();
    }

    void Update()
    {
        OnInput();
    }

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
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
        rig.MovePosition(rig.position + Direction * player.currentMoveSpeed * Time.fixedDeltaTime);
    }

    
    
    #endregion
}
