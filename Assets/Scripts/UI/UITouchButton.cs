using SuperMobileController;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITouchButton : UIControl
{
    public string inputName;
    public OnResultEvent resultEvent;
    public OnResultEvent beginTouchEvent;

    public float cooldownSecond = 2f;
    public bool canCancel = true;
    public int quantityLeft = -1;
    public Sprite buttonImage;

    private GameObject[] allCancelAreas;
    private Image cooldownImage;
    private Image disableImage;
    private Text quantityText;
    private Text cooldownText;
    private bool isEnabled = true;
    private float currentCoolDown = 0;
    private Image buttonInnerImage;
    private bool isButtonHeld = false;

    public bool IsPressed => isButtonHeld;

    void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();

        cooldownImage = transform.Find("Cooldown").GetComponent<Image>();
        disableImage = transform.Find("Disable").GetComponent<Image>();
        quantityText = transform.Find("QuantityText").GetComponent<Text>();
        cooldownText = cooldownImage.transform.Find("Text").GetComponent<Text>();

        if (buttonImage != null)
        {
            buttonInnerImage = transform.Find("ButtonInner/Image").GetComponent<Image>();
            buttonInnerImage.sprite = buttonImage;
        }

        allCancelAreas = GameObject.FindObjectsOfType<CancelArea>().Select(c => c.gameObject).ToArray();
        SetCancelAreasEnabled(false);

        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
        trigger.triggers.Add(pointerDownEntry);

        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
        pointerUpEntry.eventID = EventTriggerType.PointerUp;
        pointerUpEntry.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
        trigger.triggers.Add(pointerUpEntry);

        if (quantityLeft > -1 && quantityText != null)
        {
            quantityText.text = quantityLeft.ToString();
        }
    }

    private void Update()
    {
        if (currentCoolDown > 0)
        {
            currentCoolDown -= Time.deltaTime;
            cooldownImage.fillAmount = currentCoolDown / cooldownSecond;

            if (cooldownText != null)
            {
                cooldownText.text = currentCoolDown > 0 ? currentCoolDown.ToString("0.0") : "";
            }
        }

        if (isButtonHeld && isEnabled && currentCoolDown <= 0)
        {
            resultEvent?.Invoke(inputName, new Vector2(transform.position.x, transform.position.y));
        }
    }

    public override void OnPointerDown(PointerEventData eventArgs)
    {
        //Debug.Log("UITouchButton: OnPointerDown");
        base.OnPointerDown(eventArgs);

        if (!isEnabled || currentCoolDown > 0) return;

        if (canCancel)
        {
            SetCancelAreasEnabled(true);
        }

        isButtonHeld = true;
        beginTouchEvent?.Invoke(inputName, new Vector2(transform.position.x, transform.position.y));
    }

    public override void OnPointerUp(PointerEventData eventArgs)
    {
        //Debug.Log("UITouchButton: OnPointerUp");
        base.OnPointerUp(eventArgs);

        if (!isEnabled || currentCoolDown > 0) return;

        if (canCancel)
        {
            SetCancelAreasEnabled(false);
        }

        isButtonHeld = false;

        if (eventArgs != null && eventArgs.pointerEnter != null)
        {
            CancelArea cancelButton = eventArgs.pointerEnter.GetComponent<CancelArea>();
            if (cancelButton != null) return;
        }

        if (cooldownSecond > 0 && cooldownImage != null)
        {
            currentCoolDown = cooldownSecond;
        }

        ReturnResult(eventArgs == null ? Vector2.zero : eventArgs.position);
    }

    private void SetCancelAreasEnabled(bool isEnabled)
    {
        foreach (var ca in allCancelAreas)
        {
            ca.GetComponent<Image>().enabled = isEnabled;
        }
    }

    private void ReturnResult(Vector2 relativePosition)
    {
        resultEvent?.Invoke(inputName, relativePosition);

        if (quantityLeft > 0)
        {
            quantityLeft -= 1;
            quantityText.text = quantityLeft.ToString();
            if (quantityLeft == 0)
            {
                SetEnabled(false);
            }
        }
    }

    public void SetEnabled(bool isEnabled)
    {
        this.isEnabled = isEnabled;
        if (disableImage != null)
        {
            disableImage.enabled = !isEnabled;
        }
    }

    public void SetQuantity(int quantity)
    {
        quantityLeft = quantity;
        quantityText.text = quantity.ToString();
        SetEnabled(quantity != 0);
    }

    public void SetCoolDown(float seconds)
    {
        cooldownSecond = seconds;
    }

    public void SetButtonImage(Sprite image)
    {
        buttonImage = image;
        buttonInnerImage.sprite = image;
    }

    public override void OnBeginDrag(PointerEventData eventArgs)
    {
        //Debug.Log("UITouchButton: OnBeginDrag");
        base.OnBeginDrag(eventArgs);
    }

    public override void OnEndDrag(PointerEventData eventArgs)
    {
        //Debug.Log("UITouchButton: OnEndDrag");
        base.OnEndDrag(eventArgs);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("UITouchButton: OnDrag");
        base.OnDrag(eventData);
    }
}
