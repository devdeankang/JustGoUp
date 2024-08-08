using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public event Action<Vector2> onJoystickInput;
    public event Action<bool> onRunButton;
    public event Action onJumpButton;
    public event Action onCrawlButton;
    public event Action onClimbButton;
    public event Action<Vector2> onTouchDrag;

    private Vector2 touchStartPos;
    private bool isDragging = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        HandleJoystickInput();
        HandleUIButtonInput();
        HandleTouchInput();
    }

    void HandleJoystickInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        if (moveX != 0 || moveY != 0)
        {
            onJoystickInput?.Invoke(new Vector2(moveX, moveY));
        }
    }

    void HandleUIButtonInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            onRunButton?.Invoke(true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            onRunButton?.Invoke(false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            onJumpButton?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            onCrawlButton?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            onClimbButton?.Invoke();
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 touchDelta = touch.position - touchStartPos;
                onTouchDrag?.Invoke(touchDelta);
                touchStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
    }
}
