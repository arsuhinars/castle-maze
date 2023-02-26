using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SavePointerZone : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public Vector2 PointerPos { get; private set; } = Vector2.zero;
    public bool IsPointerDown { get; private set; } = false;
    public bool IsPointerInside { get; private set; } = false;

    public event Action OnPointerEnter;
    public event Action OnPointerExit;
    public event Action OnPointerMove;
    public event Action OnPointerClick;
    public event Action OnPointerDown;
    public event Action OnPointerUp;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        IsPointerInside = true;
        OnPointerEnter?.Invoke();
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        IsPointerInside = false;
        OnPointerExit?.Invoke();
    }

    void IPointerMoveHandler.OnPointerMove(PointerEventData eventData)
    {
        PointerPos = eventData.position;
        OnPointerMove?.Invoke();
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        OnPointerClick?.Invoke();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        IsPointerDown = true;
        OnPointerDown?.Invoke();
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        IsPointerDown = false;
        OnPointerUp?.Invoke();
    }
}
