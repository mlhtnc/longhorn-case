using UnityEngine;

public class DraggableObject : ClickableObject
{
    [SerializeField]
    private LayerMask layerMask;

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if(isPointerDown)
        {
            OnDragged();
        }
    }

    private void OnDragged()
    {
        Debug.Log("Dragged");
    
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
        
        Debug.Log(hit.collider.name);

        var newPos = 0.4f * -ray.direction + hit.point;
        newPos.y += .1f;

        transform.position = newPos;
    }
}
