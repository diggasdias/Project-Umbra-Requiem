using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private Rigidbody2D rig;

    [Header("Speed Settings")]
    [SerializeField] private float speed;
    private float initialSpeed = 3;
    private Vector2 _direction;

    void FixedUpdate()
    {
        OnMove();
    }

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        speed = initialSpeed;
    }

    #region Movement

    void OnInput()
    {
        _direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void OnMove()
    {
        rig.MovePosition(rig.position + _direction * speed * Time.fixedDeltaTime);
    }
    #endregion

}
