using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveObject : MonoBehaviour
{    
       public Camera mainCamera; // Assign your main camera here
    public LayerMask interactableLayer; // LayerMask to specify objects that can be moved
    public Plane interactionPlane = new Plane(Vector3.up, Vector3.zero); // Plane with Y = 0

    private GameObject selectedObject; // The currently selected object

    private void Update()
    {
        Vector2 touchPosition = Vector2.zero;
        bool isTouching = false;

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            // Real touch input
            touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            isTouching = true;
        }
        else if (Mouse.current.leftButton.isPressed)
        {
            // Simulate touch with mouse
            touchPosition = Mouse.current.position.ReadValue();
            isTouching = true;
        }
        else
        {
            // Reset selection if no touch is active
            selectedObject = null;
        }

        if (isTouching)
        {
            if (!selectedObject)
            {
                SelectObject(touchPosition);
            }
            else
            {
                MoveObjectToTouch(selectedObject, touchPosition);
            }
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
