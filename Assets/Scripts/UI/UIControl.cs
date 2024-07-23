using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UIControl : MonoBehaviour
{
    public virtual void OnPointerDown(PointerEventData eventArgs) { }
    public virtual void OnPointerUp(PointerEventData eventArgs) { }
    public virtual void OnBeginDrag(PointerEventData eventArgs) { }
    public virtual void OnDrag(PointerEventData eventArgs) { }
    public virtual void OnEndDrag(PointerEventData eventArgs) { }
}