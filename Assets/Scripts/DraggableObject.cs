using System;
using UnityEngine;

public class DraggableObject : ClickableObject
{
    [SerializeField]
    private LayerMask layerMask;

    private bool isDragDisabled;

    public event Action OnDragStarted;

    public bool IsDragging => isPointerDown;

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if(isDragDisabled)
            return;
        
        if(isPointerDown)
        {
            OnDragged();
        }
    }

    public override void OnPointerDown(Vector3 pos)
    {
        base.OnPointerDown(pos);

        OnDragStarted?.Invoke();

        Debug.Log(transform.name);
    }

    private void OnDragged()
    {    
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var distance = Camera.main.farClipPlane - Camera.main.nearClipPlane;

        // Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, 2f);
        Physics.Raycast(
            ray,
            out RaycastHit hit,
            Camera.main.farClipPlane - Camera.main.nearClipPlane,
            layerMask
        );

        if(hit.collider == null)
        {
            Debug.LogError("There is a missing collider in scene");
            return;
        }
        
        var newPos = 0.4f * -ray.direction + hit.point;
        newPos.y += .1f;

        transform.position = newPos;
    }

    public void EnableDrag()
    {
        isDragDisabled = false;
    }

    public void DisableDrag()
    {
        isDragDisabled = true;
    }
}
