using UnityEngine;

public class Camera_Script : MonoBehaviour
{
    private int dragSpeedY = 100;
    private int dragSpeedX = 178;
    private float zoomSpeedFactor = 0.02f;
    private Vector3 dragOrigin;

    public float zoomSpeed = 10f;
    public float minZoom = 5f;
    public float maxZoom = 50f;
    public float moveSpeed = 0.5f;

    private new Camera camera;

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        CameraDrag();

        CameraZoom();
    }

    void CameraZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0)
        {
            Vector3 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);

            float newSize = camera.orthographicSize - scrollInput * zoomSpeed;

            newSize = Mathf.Clamp(newSize, minZoom, maxZoom);

            if (newSize != camera.orthographicSize)
            {
                camera.orthographicSize = newSize;

                Vector3 directionToMouse = mouseWorldPos - transform.position;

                transform.position += directionToMouse * moveSpeed * scrollInput;
            }
        }
    }

    private void CameraDrag()
    {
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(1)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);

        float zoomFactor = 1.0f;
        if (Camera.main.orthographic)
        {
            zoomFactor = Camera.main.orthographicSize;
        }
        else
        {
            zoomFactor = Camera.main.fieldOfView;
        }

        float zoomAdjustedSpeedX = dragSpeedX * (zoomFactor * zoomSpeedFactor);
        float zoomAdjustedSpeedY = dragSpeedY * (zoomFactor * zoomSpeedFactor);

        Vector3 move = new Vector3(-pos.x * zoomAdjustedSpeedX, -pos.y * zoomAdjustedSpeedY, 0);

        transform.Translate(move, Space.World);

        dragOrigin = Input.mousePosition;
    }
}
