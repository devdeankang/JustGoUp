using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickPanel : UIControl, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string inputName;
    public float draggableRadiusModifier = 0;
    public Transform relativeTransform;

    public OnDragBeginEvent beginDragEvent;
    public OnResultEvent dragEvent;
    public Sprite analogImage;
    public Sprite analogAreaImage;

    private Image joystickImage;
    private RectTransform rectTransform;
    private Vector2 parentPosition;
    private Vector3 relativeForward;
    private float maxDisplacement;
    public Vector2 draggingTarget { get; private set; }

    public float Horizontal => draggingTarget.x;
    public float Vertical => draggingTarget.y;

    private void Start()
    {
        InitializeJoystick();
        SetupEventHandlers();
        maxDisplacement = (rectTransform.rect.width / 2) + draggableRadiusModifier;
    }

    private void InitializeJoystick()
    {
        joystickImage = transform.Find("Joystick").GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        if (analogImage != null && joystickImage != null)
        {
            joystickImage.sprite = analogImage;
        }
        if (analogAreaImage != null && GetComponent<Image>() != null)
        {
            GetComponent<Image>().sprite = analogAreaImage;
        }
    }

    private void SetupEventHandlers()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = gameObject.AddComponent<EventTrigger>();
        }

        AddEventTrigger(trigger, EventTriggerType.PointerDown, OnPointerDown);
        AddEventTrigger(trigger, EventTriggerType.PointerUp, OnPointerUp);
        AddEventTrigger(trigger, EventTriggerType.BeginDrag, OnBeginDrag);
        AddEventTrigger(trigger, EventTriggerType.Drag, OnDrag);
        AddEventTrigger(trigger, EventTriggerType.EndDrag, OnEndDrag);
    }

    private void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, UnityAction<PointerEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener((data) => action((PointerEventData)data));
        trigger.triggers.Add(entry);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("JoystickPanel: OnPointerDown");
        base.OnPointerDown(eventData);

        parentPosition = transform.position;
        beginDragEvent?.Invoke(inputName);

        if (relativeTransform != null)
        {
            relativeForward = relativeTransform.forward;
        }
        OnDrag(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("JoystickPanel: OnPointerUp");
        base.OnPointerUp(eventData);

        OnEndDrag(eventData);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("JoystickPanel: OnBeginDrag");
        base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Debug.Log("JoystickPanel: OnDrag");
        base.OnDrag(eventData);

        Vector2 relativePosition = eventData.position - parentPosition;
        if (relativePosition.magnitude > maxDisplacement)
        {
            relativePosition = relativePosition.normalized * maxDisplacement;
        }

        Vector2 targetMarkerImagePosition = relativePosition;
        relativePosition = UIHelper.GetRelativeTransformedPosition(relativePosition, relativeForward);
        draggingTarget = GetNormalizedPosition(relativePosition);

        if (joystickImage != null)
        {
            joystickImage.transform.localPosition = targetMarkerImagePosition;
        }

        ReturnResult(relativePosition);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("JoystickPanel: OnEndDrag");
        base.OnEndDrag(eventData);

        if (joystickImage != null)
        {
            joystickImage.transform.localPosition = Vector3.zero;
        }
        draggingTarget = Vector2.zero;
        ReturnResult(Vector2.zero);
    }

    private void ReturnResult(Vector2 relativePosition)
    {
        dragEvent?.Invoke(inputName, GetNormalizedPosition(relativePosition));
    }

    private Vector2 GetNormalizedPosition(Vector2 relativePosition)
    {
        return relativePosition / maxDisplacement;
    }

    private void Update()
    {
        if (joystickImage != null && joystickImage.transform.localPosition != Vector3.zero)
        {
            Vector2 relativePosition = joystickImage.transform.localPosition;
            ReturnResult(relativePosition);
        }
    }
}

// 이벤트 정의
[System.Serializable]
public class OnDragBeginEvent : UnityEvent<string> { }

[System.Serializable]
public class OnResultEvent : UnityEvent<string, Vector2> { }
