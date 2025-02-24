using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 startMousePos;
    private const float R = 100.0f;  
    private const float speed = 0.062f;  
    private const float scaleSpeed = 0.01f;  

    private Vector3 scaleFactors = new Vector3(1, 1, 1);  
    private enum InteractionMode { None, TranslateXY, TranslateXZ, Rotate, Scale }
    private InteractionMode currentMode = InteractionMode.None;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void SetInteractionMode(int mode)
    {
        currentMode = (InteractionMode)mode;
    }

    private void OnMouseDown()
    {
        startMousePos = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        Vector3 currentMousePosition = Input.mousePosition;

        switch (currentMode)
        {
            case InteractionMode.TranslateXY:
                TranslateXY(currentMousePosition - startMousePos);
                break;
            case InteractionMode.TranslateXZ:
                TranslateXZ(currentMousePosition - startMousePos);
                break;
            case InteractionMode.Rotate:
                RotateObject(currentMousePosition - startMousePos);
                break;
            case InteractionMode.Scale:
                ScaleObject(currentMousePosition - startMousePos);
                break;
        }

        startMousePos = currentMousePosition;   
    }

    private void TranslateXY(Vector3 mouseDelta)
    {
        Vector3 movement = new Vector3(mouseDelta.x * speed * 0.1f, mouseDelta.y * speed * 0.1f, 0);
        transform.position += movement;
    }

    private void TranslateXZ(Vector3 mouseDelta)
    {
        Vector3 movement = new Vector3(mouseDelta.x * speed * 0.1f, 0, mouseDelta.y * speed * 0.1f);
        transform.position += movement;
    }

    private void RotateObject(Vector3 mouseDelta)
    {
        // Calculate rotation axis and angle based on mouse movement
        float dx = mouseDelta.x;
        float dy = mouseDelta.y;
        Vector3 rotationAxis = new Vector3(dy, dx, 0).normalized;
        float angle = Mathf.Asin(Mathf.Clamp(mouseDelta.magnitude / R, -1f, 1f)) * Mathf.Rad2Deg;
        angle *= speed;

        // Apply rotation directly to the transform
        transform.Rotate(rotationAxis, angle, Space.Self);
    }

    private void ScaleObject(Vector3 mouseDelta)
    {
        float scaleChangeX = mouseDelta.x * scaleSpeed;
        float scaleChangeY = mouseDelta.y * scaleSpeed;

        scaleFactors.x += scaleChangeX;
        scaleFactors.y += scaleChangeY;

        scaleFactors.x = Mathf.Max(scaleFactors.x, 0.01f);
        scaleFactors.y = Mathf.Max(scaleFactors.y, 0.01f);

        transform.localScale = new Vector3(scaleFactors.x, scaleFactors.y, scaleFactors.z);
    }

    public void ResetTransformations()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    public void ToggleObjectActive()
    {
        gameObject.SetActive(true);
    }

    public void ToggleObjectDeactive()
    {
        gameObject.SetActive(false);
    }
}
