using UnityEngine;

public class CameraPan : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 5f;
    public Vector3 offset;

    private void FixedUpdate()
    {
        Vector3 wantedPosition = target .position + offset;
        Vector3 smoothPositon = Vector3.Lerp(transform.position, wantedPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothPositon;
    }
}
