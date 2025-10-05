using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour {
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(5, 0, -10);

    private void FixedUpdate() {
        float facingValue = target.GetComponent<Player>().GetFacingValue();
        offset = new Vector3(5*facingValue, 0, -10);

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

    }
}