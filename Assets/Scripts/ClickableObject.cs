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

    private void OnClicked()
    {
        Debug.Log("clicked");
        
        // NOTE: We can highlight the object here

        OnAnyObjectClicked?.Invoke(this);
    }

    private void OnAnyPointerUp()
    {
        isPointerDown = false;
    }
}
