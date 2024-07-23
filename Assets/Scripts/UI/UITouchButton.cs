using SuperMobileController;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITouchButton : UIControl
{
    [Header("General Settings")]
    public string inputName;

    [Header("Result Callback Settings")]
    public OnResultEvent resultEvent;  //Callback for end drag / end touch
    public OnResultEvent beginTouchEvent;  //Callback on touch begin

    [Header("Skill Button Settings")]
    public float cooldownSecond = 2f; //Cooldown of the button
    public bool canCancel = true; //Is cancel available for this button
    public int quantityLeft = -1; //Quantity left for this button to be used
    public Sprite buttonImage;

    //private variables
    private GameObject[] allCancelAreas;
    private Image cooldownImage;
    private Image disableImage;
    private Text quantityText;
    private Text cooldownText;
    private bool isEnabled = true;
    private float currentCoolDown = 0;
    private Image buttonInnerImage;
    private bool isButtonHeld = false;

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
                if (currentCoolDown > 0)
                {
                    cooldownText.text = currentCoolDown.ToString("0.0");
                }
                else
                {
                    cooldownText.text = "";
                }
            }
        }

        if (isButtonHeld && isEnabled && currentCoolDown <= 0)
        {
            if (resultEvent != null)
            {
                resultEvent.Invoke(inputName, new Vector2(transform.position.x, transform.position.y));
            }
        }
    }

    public override void OnPointerDown(PointerEventData eventArgs)
    {
        if (!isEnabled || currentCoolDown > 0) return;

        if (canCancel)
        {
            SetCancelAreasEnabled(true);
        }

        isButtonHeld = true;

        if (beginTouchEvent != null)
        {
            beginTouchEvent.Invoke(inputName, new Vector2(transform.position.x, transform.position.y));
        }
    }

    public override void OnPointerUp(PointerEventData eventArgs)
    {
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
        if (resultEvent != null)
        {
            resultEvent.Invoke(inputName, relativePosition);
        }

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
            if (isEnabled)
            {
                disableImage.enabled = false;
            }
            else
            {
                disableImage.enabled = true;
            }
        }
    }

    public void SetQuantity(int quantity)
    {
        quantityLeft = quantity;
        quantityText.text = quantity.ToString();
        if (quantity != 0)
        {
            SetEnabled(true);
        }
        else
        {
            SetEnabled(false);
        }
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
}