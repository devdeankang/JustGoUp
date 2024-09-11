using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UITouchArea : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler
{
    public CameraController cameraController;
    private bool isPointerOverUI = false;

    void Start()
    {
        if (cameraController == null)
        {
            cameraController = Camera.main.GetComponent<CameraController>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsPointerOverUIObject(eventData))
        {
            Debug.Log("UITouchArea: OnPointerDown - Starting drag");
            cameraController.StartDragging();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsPointerOverUIObject(eventData))
        {
            Debug.Log("UITouchArea: OnBeginDrag - Dragging");
            cameraController.StartDragging();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsPointerOverUIObject(eventData))
        {
            Debug.Log("UITouchArea: OnDrag - Dragging");
            cameraController.StartDragging();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("UITouchArea: OnEndDrag - Stopping drag");
        cameraController.StopDragging();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("UITouchArea: OnPointerUp - Stopping drag");
        cameraController.StopDragging();
    }



    private bool IsPointerOverUIObject(PointerEventData eventData)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = eventData.position
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("UI"))
            {
                Debug.Log("UITouchArea: IsPointerOverUIObject - Pointer is over UI object: " + result.gameObject.name);
                return true;
            }
        }
        return false;
    }
}
