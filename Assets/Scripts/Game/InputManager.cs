using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public event Action<Vector3> MoveInput;
    public event Action<bool> RunPressed;
    public event Action JumpPressed;
    public event Action CrawlPressed;
    public event Action ClimbPressed;

    public JoystickPanel joystickPanel;
    public UITouchButton jumpButton;   
    public UITouchButton runButton;    
    public UITouchButton crawlButton;  
    public UITouchButton climbButton;  
        
    public bool isRunning;
    public bool isMobileMode = true;
    bool canJump = true;
    bool canCrawl = true;
    bool canClimb = true;
    float jumpCooldown = 0.5f;
    float crawlCooldown = 0.5f;
    float climbCooldown = 0.5f;
    float jumpCooldownTimer = 0f;
    float crawlCooldownTimer = 0f;
    float climbCooldownTimer = 0f;
    Vector3 mobileMovement;
    Vector3 pcMovement;

    public Vector3 CurrentMoveVector { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        HandleCooldown();

        if(Input.GetKeyDown(KeyCode.Q))
        {
            isMobileMode = !isMobileMode;            
        }
        
        if(isMobileMode)
        {
            HandleMobileInput();
            MoveInput?.Invoke(mobileMovement);
        }
        else
        {
            HandlePCInput();
            MoveInput?.Invoke(pcMovement);
        }        
    }
    
    private void HandleCooldown()
    {
        if (!canJump)
        {
            jumpCooldownTimer += Time.deltaTime;
            if (jumpCooldownTimer >= jumpCooldown)
            {
                canJump = true;
                jumpCooldownTimer = 0f;
            }
        }

        if (!canCrawl)
        {
            crawlCooldownTimer += Time.deltaTime;
            if (crawlCooldownTimer >= crawlCooldown)
            {
                canCrawl = true;
                crawlCooldownTimer = 0f;
            }
        }

        if (!canClimb)
        {
            climbCooldownTimer += Time.deltaTime;
            if (climbCooldownTimer >= climbCooldown)
            {
                canClimb = true;
                climbCooldownTimer = 0f;
            }
        }
    }

    private void HandleMobileInput()
    {
        mobileMovement = new Vector3(joystickPanel.Horizontal, 0, joystickPanel.Vertical);
        
        if (jumpButton.IsPressed && canJump)
        {            
            JumpPressed?.Invoke();
            canJump = false;
        }

        if (runButton.IsPressed)
        {
            isRunning = true;
            RunPressed?.Invoke(true);            
        }
        else
        {
            isRunning = false;
            RunPressed?.Invoke(false);            
        }

        if (crawlButton.IsPressed && canCrawl)
        {
            CrawlPressed?.Invoke();
            canCrawl = false;
        }

        if (climbButton.IsPressed && canClimb)
        {
            ClimbPressed?.Invoke();
            canClimb = false;
        }
    }

    private void HandlePCInput()
    {
        pcMovement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            JumpPressed?.Invoke();
            canJump = false;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            RunPressed?.Invoke(true);            
        }
        else
        {
            isRunning = false;
            RunPressed?.Invoke(false);            
        }

        if (Input.GetKeyDown(KeyCode.C) && canCrawl)
        {
            CrawlPressed?.Invoke();
            canCrawl = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && canClimb)
        {
            ClimbPressed?.Invoke();
            canClimb = false;
        }
    }
    
}