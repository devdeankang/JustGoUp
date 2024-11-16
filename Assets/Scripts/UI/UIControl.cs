using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UIControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public event System.Action<PointerEventData> OnBeginDragEvent;
    public event System.Action<PointerEventData> OnEndDragEvent;
    public event System.Action<PointerEventData> OnPointerDownEvent;
    public event System.Action<PointerEventData> OnPointerUpEvent;
    public event System.Action<PointerEventData> OnDragEvent;

    protected CameraController cameraController;

    private void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    public virtual void OnBeginDrag(PointerEventData eventArgs)
    {
        //Debug.Log($"{gameObject.name}: OnBeginDrag");
        OnBeginDragEvent?.Invoke(eventArgs);
    }

    public virtual void OnEndDrag(PointerEventData eventArgs)
    {
        //Debug.Log($"{gameObject.name}: OnEndDrag");
        OnEndDragEvent?.Invoke(eventArgs);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log($"{gameObject.name}: OnPointerDown");
        OnPointerDownEvent?.Invoke(eventData);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log($"{gameObject.name}: OnPointerUp");
        OnPointerUpEvent?.Invoke(eventData);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        //Debug.Log($"{gameObject.name}: OnDrag");
        OnDragEvent?.Invoke(eventData);
    }
}
