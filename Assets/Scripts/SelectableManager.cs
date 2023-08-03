using UnityEngine;
using NotDecided.InputManagament;

public class SelectableManager : MonoBehaviour
{
    [SerializeField]
    private Material originalMaterial;

    [SerializeField]
    private Material outlineMaterial;
    
    private ISelectable currSelected;


    private void Start()
    {
        InputManager.OnAnyPointerDown      += OnAnyPointerDown;
        ClickableObject.OnAnyObjectClicked += OnAnyObjectClicked;        
    }

    private void OnAnyPointerDown()
    {
        if(currSelected != null)
        {
            ChangeMaterial(currSelected, originalMaterial);
            currSelected = null;
        }
    }

    private void OnAnyObjectClicked(ClickableObject obj)
    {
        var newSelectable = obj.GetComponent<ISelectable>();
        if(newSelectable != null)
        {
            ChangeMaterial(newSelectable, outlineMaterial);
            currSelected = newSelectable;
        }
    }

    private void ChangeMaterial(ISelectable selectable, Material mat)
    {
        selectable.Renderer.material = mat;
    }
}
