using UnityEngine;

public class DragOrthographicCamera : MonoBehaviour
{
    [SerializeField]
    private float dragSpeed = 2f;
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform startPoint;
    private bool dragPanMoveActive;
    private Vector2 lastMousePosition;

    private Vector3 InitPos;

    private void Awake()
    {
        InitPos = transform.position;
    }
    void LateUpdate()
    {
        HandleCameraMovementDragPan();
    }
    private void HandleCameraMovementDragPan()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        if (Input.GetMouseButtonDown(0))
        {
            dragPanMoveActive = true;
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            dragPanMoveActive = false;
        }

        if (dragPanMoveActive)
        {
            Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - lastMousePosition;

            float dragPanSpeed = 1f;
            inputDir.x = mouseMovementDelta.x * dragPanSpeed;
            inputDir.z = mouseMovementDelta.y * dragPanSpeed;

            lastMousePosition = Input.mousePosition;
        }

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        transform.position += -moveDir * dragSpeed * Time.deltaTime;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, startPoint.position.x, endPoint.position.x), InitPos.y, InitPos.z);
    }
}
