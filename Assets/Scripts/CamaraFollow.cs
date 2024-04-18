
using UnityEngine;

public class CamaraFollow : MonoBehaviour
{
    // Follow Player
    public Transform target;

    // Camera smothSpeed
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }




}
