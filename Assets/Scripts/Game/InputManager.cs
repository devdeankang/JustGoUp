using System;
using UnityEngine;

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

    public Vector3 CurrentMoveVector { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Vector3 joystickMoveVector = new Vector3(joystickPanel.Horizontal, 0, joystickPanel.Vertical);
        Vector3 keyboardMoveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        CurrentMoveVector = joystickMoveVector != Vector3.zero ? joystickMoveVector : keyboardMoveVector;

        if (CurrentMoveVector != Vector3.zero)
        {
            MoveInput?.Invoke(CurrentMoveVector);
        }

        if (Input.GetKey(KeyCode.F2))
        {
            if (joystickPanel != null) joystickPanel.gameObject.SetActive(true);
            HandleMobileInput();
        }
        else if (Input.GetKey(KeyCode.F3))
        {
            if (joystickPanel != null) joystickPanel.gameObject.SetActive(false);
            HandlePCInput();
        }
    }
        

    private void HandleMobileInput()
    {
        Vector3 moveVector = new Vector3(joystickPanel.Horizontal, 0, joystickPanel.Vertical);
        MoveInput?.Invoke(moveVector);

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
        Vector3 moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        MoveInput?.Invoke(moveVector);

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