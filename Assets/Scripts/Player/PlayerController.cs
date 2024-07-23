using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public Animator anim;
    public Transform tr;
    public Collider coll;
    public JoystickPanel joystickPanel;
    public CameraController cameraController;

    public bool IsActive;
    public bool isGrounded;
    public int idleTime;
    public float waitTime = 2f;
    public float walkSpeed = 0.5f;
    public float runSpeed = 1.5f;
    public float moveSpeed;

    public Vector3 PlayerForce { get; set; }
    public float rotationSpeed = 5f;

    public bool isMobileMode = true;
    float groundCheckDistance = 0.1f;
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
        coll = GetComponent<Collider>();

        InitializeStates();
    }

    private void Start()
    {
        IsActive = true;
        RunAnimSpeed = 0;
        RunButtonHoldTime = 0.1f;
        RunButtonHoldTimer = 0f;
        if (joystickPanel != null) joystickPanel.dragEvent.AddListener(SetJoystick);
    }

    private void Update()
    {
        stateMachine.CurrentState.Update(this);

        HandleInputMode();
        RotateCharacter();
        HandleRunButton();
        UpdateGroundedState();
    }

    private void FixedUpdate()
    {
        stateMachine.CurrentState.FixedUpdate(this);
    }

    void HandleInputMode()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            SwitchToPCMode();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SwitchToMobileMode();
        }

        if (isMobileMode)
        {
            HandleJoystickInput();
        }
        else
        {
            HandleKeyboardInput();
        }
    }

    void RotateCharacter()
    {
        if (PlayerForce != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(PlayerForce);
            tr.rotation = Quaternion.Slerp(tr.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void HandleRunButton()
    {
        if (!isMobileMode && Input.GetKey(KeyCode.LeftShift))
        {
            IsRun = true;
            RunButtonHoldTimer = RunButtonHoldTime;
        }
    }

    private void OnDestroy()
    {
        if (joystickPanel != null) joystickPanel.dragEvent.RemoveListener(SetJoystick);
    }

    public void SetJoystick(string inputName, Vector2 input)
    {
        if (isMobileMode && inputName.ToLower() == "movement")
        {
            PlayerForce = new Vector3(input.x, 0, input.y);            
        }
    }

    void HandleJoystickInput()
    {
        if (PlayerForce != Vector3.zero)
        {
            stateMachine.ChangeState(stateMap[State.Move]);
        }
    }

    void HandleKeyboardInput()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        PlayerForce = new Vector3(x, 0, z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsJump = true;
        }
    }

    void SwitchToPCMode()
    {
        isMobileMode = false;
        if (joystickPanel != null) joystickPanel.gameObject.SetActive(false);
    }

    void SwitchToMobileMode()
    {
        isMobileMode = true;
        if (joystickPanel != null) joystickPanel.gameObject.SetActive(true);
    }

    public void SetInput(string inputName, Vector2 direction)
    {
        switch (inputName.ToLower())
        {
            case "jump":
                IsJump = true;
                break;
            case "run":
                IsRun = true;
                RunButtonHoldTimer = RunButtonHoldTime;
                break;
            case "crawl":
                IsCrawl = true;
                break;
            case "climb":
                IsClimb = true;
                break;
            default:
                break;
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
        if (Physics.Raycast(transform.position + Vector3.up * 0.04f + transform.right * 0.1f + transform.forward * 0.08f, Vector3.down, 0.07f)
        || Physics.Raycast(transform.position + Vector3.up * 0.04f + transform.right * 0.1f + transform.forward * -0.08f, Vector3.down, 0.07f)
        || Physics.Raycast(transform.position + Vector3.up * 0.04f + transform.right * -0.1f + transform.forward * 0.08f, Vector3.down, 0.07f)
        || Physics.Raycast(transform.position + Vector3.up * 0.04f + transform.right * -0.1f + transform.forward * -0.08f, Vector3.down, 0.07f))
        {
            isGrounded = true;
            coll.material.dynamicFriction = 1;
            coll.material.staticFriction = 1;
        }
        else
        {
            isGrounded = false;
            coll.material.dynamicFriction = 0;
            coll.material.staticFriction = 0;
            rayCorrection = 0.025f;
            anim.SetInteger("up", 4);
        }
        anim.SetBool("grounded", isGrounded);
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
