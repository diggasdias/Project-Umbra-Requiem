using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private Rigidbody2D rig;
    private PlayerItens playerItens;

    [Header("Speed Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float runSpeed;
    private float initialSpeed;
    private bool _isRunning;
    private bool _isRolling;
    private bool _isCutting;
    private bool _isDigging;
    private bool _isWatering;
    private int handling;
    private Vector2 _direction;

    public bool IsRunning {
        get { return _isRunning; }
        set { _isRunning = value; }
    }

    public bool IsRolling
    {
        get { return _isRolling; }
        set { _isRolling = value; }
    }

    public Vector2 Direction {
        get { return _direction; }
        set { _direction = value; }
    }

    public bool IsCutting {
        get { return _isCutting; }
        set { _isCutting = value; }
    }

    public bool IsDigging { get => _isDigging; set => _isDigging = value; }
    public bool IsWatering { get => _isWatering; set => _isWatering = value; }

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        playerItens = GetComponent<PlayerItens>();
        initialSpeed = speed;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            handling = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            handling = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            handling = 2;
        }
        OnInput();
        OnRun();
        OnRoll();
        OnCutting();
        OnDigging();
        OnWatering();
    }

    void FixedUpdate()
    {
        OnMove();
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

    void OnRun()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = runSpeed;
            _isRunning = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = initialSpeed;
            _isRunning = false;
        }
    }

    void OnRoll()
    {
        if (Input.GetMouseButtonDown(1))
        {
            speed = runSpeed*2;
            _isRolling = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            speed = initialSpeed;
            _isRolling = false;
        }
    }

    void OnCutting()
    {
        if (handling == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isCutting = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                _isCutting = false;
            }
        }
    }

    void OnDigging()
    {
        if (handling == 1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isDigging = true;
                speed = 0;
            }
            if (Input.GetMouseButtonUp(0))
            {
                _isDigging = false;
                speed = initialSpeed;
            }
        }
    }

    void OnWatering()
    {
        if (handling == 2)
        {
            if (Input.GetMouseButtonDown(0) && playerItens.CurrentWater > 0)
            {
                _isWatering = true;
                speed = 0;
            }
            if (Input.GetMouseButtonUp(0) || playerItens.CurrentWater <= 0)
            {
                _isWatering = false;
                speed = initialSpeed;
            }

            if (_isWatering)
            {
                playerItens.CurrentWater -= 0.03f;
            }
        }
    }
    #endregion

}
