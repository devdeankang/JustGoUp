using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 8.79f, -15.57f);
    public Vector3 rotationOffset = new Vector3(25f, 0, 0);
    public float rotationSpeed = 3.0f;
    public float followSpeed = 0.8f;
        
    Quaternion initialRotationOffset;
    Quaternion targetRotation;
    Vector3 initialOffset;
    Vector3 currentVelocity;

    void Start()
    {        
        if (player == null)
        {
            Debug.LogError("CameraController: Player transform is not assigned!");
            return;
        }
             
        initialOffset = offset;
        initialRotationOffset = Quaternion.Euler(rotationOffset);
    }

    void LateUpdate()
    {
        if (player == null)
        {
            Debug.LogError("CameraController: Player transform is not assigned!");
            return;
        }

        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, followSpeed);
        transform.position = smoothedPosition;

        Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position);
        Quaternion adjustedRotation = Quaternion.Euler(rotationOffset.x, targetRotation.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, adjustedRotation, Time.deltaTime * rotationSpeed);
    }

    public void SetIdleCameraOffset(Vector3 offset)
    {
        this.offset = offset;
    }
}
