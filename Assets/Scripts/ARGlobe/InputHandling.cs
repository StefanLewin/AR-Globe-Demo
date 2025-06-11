using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ARGlobe
{
    public class InputHandling : MonoBehaviour
    {    
        public Camera mainCamera; 
        public LayerMask interactableLayer; 
        public Plane interactionPlane = new Plane(Vector3.up, Vector3.zero);
        public event EventHandler OnInputFinished;
        public event EventHandler OnInputStarted;

        private GameObject selectedObject; 
        private bool _inputFinishedRegistered;
    
        private void Update()
        {
            Vector2 touchPosition = Vector2.zero;
            bool isTouching = false;

            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                // Real touch input
                touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                isTouching = true;
                _inputFinishedRegistered = false;
            }
            else if (Mouse.current.leftButton.isPressed)
            {
                // Simulate touch with mouse
                touchPosition = Mouse.current.position.ReadValue();
                isTouching = true;
                _inputFinishedRegistered = false;
            }
            else
            {
                // Reset selection if no touch is active
                selectedObject = null;
            }

            if (!isTouching)
            {
                if (_inputFinishedRegistered) return;
                _inputFinishedRegistered = true;
                OnInputFinished?.Invoke(this, EventArgs.Empty);
                return;
            }
        
            OnInputStarted?.Invoke(this, EventArgs.Empty);
        
            if (!selectedObject)
            {
                SelectObject(touchPosition);
            }
            else
            {
                MoveObjectToTouch(selectedObject, touchPosition);
            }
        }

        private void SelectObject(Vector2 touchPosition)
        {
            Ray ray = mainCamera.ScreenPointToRay(touchPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactableLayer))
            {
                // Set the selected object if the ray hits something on the interactable layer
                selectedObject = hit.collider.gameObject;
            }
        }

        private void MoveObjectToTouch(GameObject obj, Vector2 touchPosition)
        {
            Ray ray = mainCamera.ScreenPointToRay(touchPosition);

            if (interactionPlane.Raycast(ray, out float distance))
            {
                Vector3 worldPosition = ray.GetPoint(distance);
                obj.transform.position = worldPosition;
            }
        }
    }
}
