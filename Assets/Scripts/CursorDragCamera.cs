using UnityEngine;

public class CursorDragCamera : MonoBehaviour {
    public float DragSpeed = 2.0f; 
    public float ZoomSpeed = 5.0f; 
    public float MinZoom = 1.0f; 
    public float MaxZoom = 30.0f;

    private Vector3 dragOrigin; 
    private void Start() {
        dragOrigin = Vector3.zero;
        //Camera.main.orthographicSize = 30;
    }
    private void Update() {
        // Drag Camera with Mouse Drag
        if (Input.GetMouseButtonDown(0)) {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (Input.GetMouseButton(0)) {
            Vector3 difference = Input.mousePosition - dragOrigin;
            Vector3 move = new Vector3(-difference.x * DragSpeed * Time.deltaTime, -difference.y * DragSpeed * Time.deltaTime, 0);
            transform.Translate(move, Space.World);
            dragOrigin = Input.mousePosition;
        }

        // Zoom In and Out with Mouse Scroll Wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0) {
            float newSize = Mathf.Clamp(Camera.main.orthographicSize - scroll * ZoomSpeed, MinZoom, MaxZoom);
            Camera.main.orthographicSize = newSize;
        }
    }
}
