using UnityEngine;

public class CursorDragCamera : MonoBehaviour {
    public float dragSpeed = 2.0f; // Speed at which the camera moves
    private Vector3 dragOrigin; // The starting point of the drag

    void Update() {
        // Detect mouse button press
        if (Input.GetMouseButtonDown(0)) {
            // Capture the initial position where the mouse button was pressed
            dragOrigin = Input.mousePosition;
            return;
        }

        // Detect mouse button hold and drag
        if (Input.GetMouseButton(0)) {
            // Calculate how much the mouse has moved since the initial press
            Vector3 difference = Input.mousePosition - dragOrigin;

            // Move the camera in the opposite direction of the mouse movement
            Vector3 move = new Vector3(-difference.x * dragSpeed * Time.deltaTime, -difference.y * dragSpeed * Time.deltaTime, 0);

            transform.Translate(move, Space.World);

            // Update drag origin for the next frame
            dragOrigin = Input.mousePosition;
        }
    }
}
