using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;        // ī�޶� ���� ��� (ĳ����)
    public Vector3 offset;          // ī�޶�� ĳ���� ������ �ʱ� �Ÿ�
    public float smoothSpeed = 0.125f;  // ī�޶��� �̵� �ӵ�
    public float rotationSmoothSpeed = 5f;  // ȸ�� �ӵ�
    public Vector3 rotationOffset;  // ȸ�� ������ (x, y, z)

    private Vector3 initialOffset;  // �ʱ� ������ �� ����
    private Vector3 initialRotationOffset;  // �ʱ� ������ �� ����

    private void Start()
    {
        initialOffset = offset;  // �ʱ� ������ ����
        initialRotationOffset = rotationOffset;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // ���� Ÿ���� ��ġ�� ȸ������ �̿��� ī�޶��� ��ǥ ��ġ ���
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Ÿ��(ĳ����)�� �ٶ󺸵��� ȸ�� (ȸ�� ������ �߰�)
        Vector3 direction = target.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(rotationOffset);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothSpeed * Time.deltaTime);
    }

    public void SetIdleCameraOffset(Vector3 newOffset, Vector3 newRotationOffset)
    {
        offset = newOffset;  // ���ο� ������ ����
        rotationOffset = newRotationOffset;
    }

    public void ResetCameraOffset()
    {
        offset = initialOffset;  // �ʱ� ���������� �ǵ���
        rotationOffset = initialRotationOffset;         
    }
}
