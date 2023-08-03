using System;
using UnityEngine;
using NotDecided.InputManagament;

public class ClickableObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    protected bool isPointerDown;

    public static event Action<ClickableObject> OnAnyObjectClicked;

    protected virtual void Start()
    {
        InputManager.OnAnyPointerUp += OnAnyPointerUp;
    }

    public virtual void OnPointerDown(Vector3 pos)
    {
        isPointerDown = true;
    }

    public virtual void OnPointerUp(Vector3 pos)
    {
        if(isPointerDown)
        {
            OnClicked();
        }
    }

    protected virtual void OnAnyPointerUp()
    {
        isPointerDown = false;
    }

    private void OnClicked()
    {        
        OnAnyObjectClicked?.Invoke(this);
    }
}
