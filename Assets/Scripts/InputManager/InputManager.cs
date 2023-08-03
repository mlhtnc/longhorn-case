using System;
using UnityEngine;

namespace NotDecided.InputManagament
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;

        private Camera currCamera;

        [SerializeField]
        private LayerMask layerMask;

        private bool pointerWasOnUI;

        private bool isInputDisabled;

        public static event Action OnAnyPointerDown;

        public static event Action OnAnyPointerUp;

        private void Awake()
        {
            if(Instance != null)
            {
                return;
            }

            Instance = this;
            currCamera = Camera.main;
        }

        private void Update()
        {
            if(isInputDisabled)
                return;

            var down = Input.GetMouseButtonDown(0);
            var up = Input.GetMouseButtonUp(0);

            if(down)
            {
                OnMouseDown();
            }

            if(up)
            {
                OnMouseUp();
            }
        }

        private void OnMouseDown()
        {
            Vector2 pos = Input.mousePosition;
            if(InputHelper.IsPointerOverUIObject(pos))
            {
                pointerWasOnUI = true;
                return;
            }
            
            pointerWasOnUI = false;
            RaycastHit hit;
            RaycastFromCamera(pos, out hit);

            if (hit.collider != null)
            {
                hit.transform.GetComponent<IPointerDownHandler>()?.OnPointerDown(hit.point);
            }

            OnAnyPointerDown?.Invoke();
        }
     
        private void OnMouseUp()
        {
            // If one of the pointers was on UI, do not run the rest of it
            if(pointerWasOnUI)
                return;

            Vector2 pos = Input.mousePosition;
            RaycastHit hit;

            RaycastFromCamera(pos, out hit);

            if (hit.collider != null)
            {
                hit.transform.GetComponent<IPointerUpHandler>()?.OnPointerUp(hit.point);
            }

            // NOTE: Call this before OnPointerUp call
            OnAnyPointerUp?.Invoke();
        }

        private void RaycastFromCamera(Vector3 pos,out RaycastHit hit)
        {
            Ray ray = currCamera.ScreenPointToRay(pos);
            var distance = currCamera.farClipPlane - currCamera.nearClipPlane;

            // Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, 2f);
            Physics.Raycast(
                ray,
                out hit,
                currCamera.farClipPlane - currCamera.nearClipPlane,
                layerMask
            );
        }

        public void EnableInput()
        {
            isInputDisabled = true;
        }

        public void DisableInput()
        {
            isInputDisabled = false;
        }
    }
}
