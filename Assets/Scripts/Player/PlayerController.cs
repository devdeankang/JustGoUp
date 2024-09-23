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
    public float walkSpeed = 0.5f;
    public float runSpeed = 1.0f;
    public float moveSpeed;
    Vector3 lastMovementDirection;

    public Vector3 PlayerForce { get; set; }
    public float rotationSpeed = 5f;

    public bool isMobileMode = true;
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

    private void Start()
    {
        IsActive = true;
        RunAnimSpeed = 0;
        RunButtonHoldTime = 0.1f;
        RunButtonHoldTimer = 0f;        
    }

    private void Update()
    {
        stateMachine.CurrentState.Update(this);

        HandleRotation();        
    }

    private void FixedUpdate()
    {
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

    private void OnCollisionStay(Collision collision)
    {
        if (collision.contacts[0].point.y < transform.position.y)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
        anim.SetInteger("up", 4);
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
