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
        
    private bool isRunning;
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
        HandlePCInput(); 
        HandleMobileInput();

        MoveInput?.Invoke(mobileMovement + pcMovement);
    }

    private void HandleMobileInput()
    {
        mobileMovement = new Vector3(joystickPanel.Horizontal, 0, joystickPanel.Vertical);
        
        if (jumpButton.IsPressed)
        {            
            JumpPressed?.Invoke();
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

        if (crawlButton.IsPressed)
        {
            CrawlPressed?.Invoke();
        }

        if (climbButton.IsPressed)
        {
            ClimbPressed?.Invoke();
        }
    }

    private void HandlePCInput()
    {
        pcMovement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpPressed?.Invoke();
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

        if (Input.GetKeyDown(KeyCode.C))
        {
            CrawlPressed?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ClimbPressed?.Invoke();
        }
    }
    
}