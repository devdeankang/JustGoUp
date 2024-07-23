using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;        // 카메라가 따라갈 대상 (캐릭터)
    public Vector3 offset;          // 카메라와 캐릭터 사이의 초기 거리
    public float smoothSpeed = 0.125f;  // 카메라의 이동 속도
    public float rotationSmoothSpeed = 5f;  // 회전 속도
    public Vector3 rotationOffset;  // 회전 오프셋 (x, y, z)

    private Vector3 initialOffset;  // 초기 오프셋 값 저장
    private Vector3 initialRotationOffset;  // 초기 오프셋 값 저장

    private void Start()
    {
        initialOffset = offset;  // 초기 오프셋 저장
        initialRotationOffset = rotationOffset;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // 현재 타겟의 위치와 회전값을 이용해 카메라의 목표 위치 계산
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // 타겟(캐릭터)을 바라보도록 회전 (회전 오프셋 추가)
        Vector3 direction = target.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(rotationOffset);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothSpeed * Time.deltaTime);
    }

    public void SetIdleCameraOffset(Vector3 newOffset, Vector3 newRotationOffset)
    {
        offset = newOffset;  // 새로운 오프셋 설정
        rotationOffset = newRotationOffset;
    }

    public void ResetCameraOffset()
    {
        offset = initialOffset;  // 초기 오프셋으로 되돌림
        rotationOffset = initialRotationOffset;         
    }
}
