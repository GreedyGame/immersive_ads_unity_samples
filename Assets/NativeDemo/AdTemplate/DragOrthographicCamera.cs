using UnityEngine;
using UnityEngine.UIElements;

public class DragOrthographicCamera : MonoBehaviour
{
    Vector3 hit_position = Vector3.zero;
    Vector3 current_position = Vector3.zero;
    Vector3 camera_position = Vector3.zero;
    private Camera cam;
    private Vector3 targetPosition;
    public float minX;
    public float maxX;
    private Vector3 direction;

    private void Awake()
    {
        cam=Camera.main;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hit_position = Input.mousePosition;
            camera_position = transform.position;

        }
        if (Input.GetMouseButton(0))
        {
            current_position = Input.mousePosition;
            LeftMouseDrag();
        }
    }
    void LeftMouseDrag()
    {
        direction = GetWorldPosition(current_position) - GetWorldPosition(hit_position);
        Vector3 temPos = camera_position - direction;
        targetPosition = new Vector3(Mathf.Clamp(temPos.x, minX, maxX), transform.position.y, -10);
        transform.position = targetPosition;
    }
    public Vector3 GetWorldPosition(Vector3 pos)
    {
        return cam.ScreenToWorldPoint(pos);
    }

}
