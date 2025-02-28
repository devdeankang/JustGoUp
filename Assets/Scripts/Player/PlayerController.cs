using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    float currentHealth = 10000f; // ## temp 임시값

    public Rigidbody rb;
    public Animator anim;
    public Transform tr;
    public CapsuleCollider coll;
    public RaycastHit hit;
    public RaycastHit hit2;    
    public JoystickPanel joystickPanel;
    public CameraController cameraController;
    Renderer[] renderers;
    Coroutine invincibilityCoroutine;

    public bool IsActive { get; set; }
    public bool isGrounded;
    public int idleTime;
    public float waitTime = 2f;
    float walkSpeed = 1.5f;
    float runSpeed = 3f;
    float jumpMultiplier = 1.5f;
    public float moveSpeed;
    Vector3 lastMovementDirection;
    int defaultLayer;
    int crawlingLayer;

    public Vector3 PlayerForce { get; set; }
    public float rotationSpeed = 5f;

    public bool isMobileMode = true;
    public bool IsTransitionAllowed { get; set; } = true;
    public bool isInvincible = false;
    public float invincibleDuration = 2f;
    float blinkInterval = 0.1f;
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
        renderers = GetComponentsInChildren<Renderer>();

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
        Vector3 cameraForward = cameraController.transform.forward;
        Vector3 cameraRight = cameraController.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

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

        defaultLayer = LayerMask.NameToLayer(LayerTagManager.PlayerLayer);
        crawlingLayer = LayerMask.NameToLayer(LayerTagManager.CrawlingPlayerLayer);
        gameObject.layer = defaultLayer;
    }

    private void Update()
    {
        stateMachine.CurrentState.Update(this);

        HandleRotation();
        HandleLayerCollision();
    }

    private void FixedUpdate()
    {        
        UpdateGroundedState();
        stateMachine.CurrentState.FixedUpdate(this);
    }

    void HandleLayerCollision()
    {
        if (anim.GetInteger("up") < 4)
        {
            int nonCrawlObstacle = LayerMask.NameToLayer(LayerTagManager.NonCrawlObstacleLayer);
            LayerTagManager.SetLayerRecursively(gameObject, crawlingLayer);
            Physics.IgnoreLayerCollision(crawlingLayer, nonCrawlObstacle , true);
        }
        else
        {
            LayerTagManager.SetLayerRecursively(gameObject, defaultLayer);
        }
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

    public void ApplyHit(float damage, float knockbackForce, Vector3 collisionPoint)
    {
        if (isInvincible) return;
        if (stateMachine.CurrentState == stateMap[State.Hit]) return;

        if (stateMap[State.Hit] is SafeState<PlayerController> safeState &&
            safeState.GetWrappedState() is HitState hitState)
        {
            hitState.SetHitData(damage, knockbackForce, collisionPoint);
            stateMachine.ChangeState(stateMap[State.Hit]);
        }
    }

    public void TakeDamage(float damage)
    {           
        currentHealth -= damage;
        Debug.Log($"플레이어가 {damage}의 피해를 입었습니다. 남은 체력 : {currentHealth}");

        if(currentHealth <= 0)
        {
            stateMachine.ChangeState(stateMap[State.Dead]);
            Debug.Log("DeadState로 전환합니다.");
        }
    }

    public void StartInvincibility()
    {
        if(!isInvincible)
        {
            isInvincible = true;
            if(invincibilityCoroutine != null)
            {
                StopCoroutine(invincibilityCoroutine);
            }
            invincibilityCoroutine = StartCoroutine(InvincibilityRoutine());
        }
    }
    
    IEnumerator InvincibilityRoutine()
    {
        float elapsedTime = 0;
        bool isVisible = true;

        while (elapsedTime < invincibleDuration)
        {
            foreach(var renderer in renderers)
            {
                renderer.enabled = isVisible;
            }

            isVisible = !isVisible;
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }

        foreach(var renderer in renderers)
        {
            renderer.enabled = true;
        }
        isInvincible = false;
        invincibilityCoroutine = null;
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
        }
        else
        {
            isGrounded = false;
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
        rb.AddForce(Vector3.up * jumpforce * jumpMultiplier + (tr.forward * anim.GetFloat("run") * 0.125f), ForceMode.Impulse);
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
