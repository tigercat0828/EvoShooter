
using UnityEngine;

public class SmoothCamFollow : MonoBehaviour {
    [SerializeField] private Vector3 offset;
    [SerializeField] private float damping;

    public Transform target;
    private Vector3 velocity = Vector3.zero;
    private void FixedUpdate() {
        if (!target) return;

        Vector3 targetPosition = target.position + offset;
        targetPosition.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, damping);
        transform.position = targetPosition;
    }
}
