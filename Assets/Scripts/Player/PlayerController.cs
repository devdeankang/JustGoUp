using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public Animator anim;
    public Transform tr;
    public CapsuleCollider coll;
    public RaycastHit hit;
    public RaycastHit hit2;    
    public JoystickPanel joystickPanel;
    public CameraController cameraController;

    public bool IsActive { get; set; }
    public bool isGrounded;
    public int idleTime;
    public float waitTime = 2f;
    float walkSpeed = 1.5f;
    float runSpeed = 3f;
    public float moveSpeed;
    Vector3 lastMovementDirection;
    int defaultLayer;
    int crawlingLayer;

    public Vector3 PlayerForce { get; set; }
    public float rotationSpeed = 5f;

    public bool isMobileMode = true;
    public bool IsTransitionAllowed { get; set; } = true;
    public float rayCorrection = 0.025f;
    public StateMachine<PlayerController> stateMachine;
    public Dictionary<State, IState<PlayerController>> stateMap;

    public int RunAnimSpeed { get; set; }
    public float RunButtonHoldTime { get; set; }
    public float RunButtonHoldTimer { get; set; }
    public bool IsRun { get; set; }
    public bool IsJump { set; get; }
    public bool IsCrawl { set; get; }
    public bool IsClimb { set; get; }

    public enum State
    {
        Idle, Move, Jump, Fall, Crawl, Climb, Hit, Dead
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        coll = GetComponent<CapsuleCollider>();

        InitializeStates();
    }

    private void OnEnable()
    {
        InputManager.Instance.MoveInput += OnMovePressed;        
        InputManager.Instance.RunPressed += OnRunPressed;
        InputManager.Instance.JumpPressed += OnJumpPressed;
        InputManager.Instance.CrawlPressed += OnCrawlPressed;
        InputManager.Instance.ClimbPressed += OnClimbPressed;
    }

    private void OnDisable()
    {
        InputManager.Instance.MoveInput -= OnMovePressed;
        InputManager.Instance.RunPressed -= OnRunPressed;
        InputManager.Instance.JumpPressed -= OnJumpPressed;
        InputManager.Instance.CrawlPressed -= OnCrawlPressed;
        InputManager.Instance.ClimbPressed -= OnClimbPressed;
    }

    void OnMovePressed(Vector3 movement)
    { 
        // 카메라의 방향을 기준으로 입력 좌표를 변환
        Vector3 cameraForward = cameraController.transform.forward;
        Vector3 cameraRight = cameraController.transform.right;

        // 카메라가 캐릭터 위에서 바라보므로 수평축만 고려 (y축 제외)
        cameraForward.y = 0;
        cameraRight.y = 0;

        // 입력된 좌표를 카메라 기준 좌표로 변환
        Vector3 adjustedMovement = (cameraForward.normalized * movement.z) + (cameraRight.normalized * movement.x);

        PlayerForce = adjustedMovement * moveSpeed;
    }
    void OnRunPressed(bool isRunning)
    {
        moveSpeed = isRunning ? runSpeed : walkSpeed;
        IsRun = isRunning;
    }
    void OnJumpPressed() => IsJump = true;
    void OnCrawlPressed() => IsCrawl = !IsCrawl; 
    void OnClimbPressed() => IsClimb = !IsClimb;

    private void Start()
    {
        IsActive = true;
        RunAnimSpeed = 0;
        RunButtonHoldTime = 0.1f;
        RunButtonHoldTimer = 0f;        
        anim.SetInteger("up", 4);
    }

    private void Update()
    {
        stateMachine.CurrentState.Update(this);

        HandleRotation();        
    }

    private void FixedUpdate()
    {        
        UpdateGroundedState();
        stateMachine.CurrentState.FixedUpdate(this);                
    }
       

    void RotateCharacter()
    {
        if (PlayerForce != Vector3.zero)
        {
            Vector3 direction = new Vector3(PlayerForce.x, 0, PlayerForce.z);
            if (direction.magnitude > 0.01f)
            {
                lastMovementDirection = direction;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                tr.rotation = Quaternion.Slerp(tr.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
        else if (lastMovementDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lastMovementDirection);
            tr.rotation = Quaternion.Slerp(tr.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public IEnumerator Wait(float waitTime)
    {
        IsActive = false;
        yield return new WaitForSeconds(waitTime);
        IsActive = true;
    }

    private void UpdateGroundedState()
    {
        float rayDistance = 0.3f;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        Ray ray = new Ray(rayOrigin, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, rayDistance))
        {
            isGrounded = true;            
            //Debug.Log("Player is Grounded");
        }
        else
        {
            isGrounded = false;
            //Debug.Log("Player is Not Grounded");
        }
    }

    public void Straight()
    {
        Vector3 direction = new Vector3(PlayerForce.x, 0f, PlayerForce.z);
        if (direction != Vector3.zero)
        {
            tr.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

    public void Jump(float jumpforce)
    {
        rb.AddForce(Vector3.up * jumpforce + (tr.forward * anim.GetFloat("run") * 0.125f), ForceMode.Impulse);
    }

    void HandleRotation()
    {
        if (stateMachine.CurrentState != stateMap[State.Crawl])
        {
            RotateCharacter();
        }
    }

    #region FSM
    void InitializeStates()
    {
        stateMap = new Dictionary<State, IState<PlayerController>>()
        {
            { State.Idle, new SafeState<PlayerController>(new IdleState()) },
            { State.Move, new SafeState<PlayerController>(new MoveState()) },
            { State.Jump, new SafeState<PlayerController>(new JumpState()) },
            { State.Fall, new SafeState<PlayerController>(new FallState()) },
            { State.Crawl, new SafeState<PlayerController>(new CrawlState()) },
            { State.Climb, new SafeState<PlayerController>(new ClimbState()) },
            { State.Hit, new SafeState<PlayerController>(new HitState()) },
            { State.Dead, new SafeState<PlayerController>(new DeadState()) }
        };

        stateMachine = new StateMachine<PlayerController>(this, stateMap[State.Idle]);
    }
    #endregion
}
