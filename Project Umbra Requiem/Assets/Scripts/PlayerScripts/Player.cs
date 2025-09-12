using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private Rigidbody2D rig;

    [Header("Speed Settings")]
    private float speed;
    private float initialSpeed = 3;
    private Vector2 direction;

  
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
        speed = initialSpeed; 
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
        rig.MovePosition(rig.position + Direction * speed * Time.fixedDeltaTime);
    }

    
    
    #endregion
}
