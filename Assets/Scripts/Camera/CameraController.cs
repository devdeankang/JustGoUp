using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 8.79f, -15.57f);
    public Vector3 rotationOffset = new Vector3(25f, 0, 0);
    public float rotationSpeed = 3.0f;
    public float followSpeed = 0.8f;
    public float zoomSpeed = 5f;
    public float zoomThreshold = 0.1f;

    string[] ignoreTags = { 
        LayerTagManager.PlayerTag,
        LayerTagManager.UITag,
        LayerTagManager.NonCrawlObstacleTag        
    };

    private Quaternion initialRotationOffset;
    private Vector3 initialOffset;
    private Vector3 currentVelocity;
    private Vector3 zoomedOffset;
    private GameObject lastObstacle;
    private bool isZoomed = false;

    void Start()
    {
        initialOffset = offset;
        zoomedOffset = offset;
        initialRotationOffset = Quaternion.Euler(rotationOffset);
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag(LayerTagManager.PlayerTag).transform;
        }

        Vector3 directionToPlayer = player.position - transform.position;
        RaycastHit hit;

        if (Physics.Raycast(player.position, -directionToPlayer.normalized, out hit, directionToPlayer.magnitude))
        {
            foreach (string tag in ignoreTags)
            {
                if (hit.collider.CompareTag(tag))
                {
                    return;
                }
                
            }

            if (lastObstacle == hit.collider.gameObject && isZoomed)
            {
                return;
            }

            lastObstacle = hit.collider.gameObject;
            Vector3 hitPoint = hit.point + directionToPlayer.normalized * 0.5f;
            zoomedOffset = hitPoint;
            isZoomed = true;
        }
        else
        {
            if (isZoomed)
            {
                lastObstacle = null;
                zoomedOffset = initialOffset;
                isZoomed = false;
            }
        }
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + (isZoomed ? zoomedOffset : offset);

        if (isZoomed)
        {
            if (lastObstacle != null && Vector3.Distance(transform.position, zoomedOffset) <= zoomThreshold)
            {
                return;
            }

            if (Vector3.Distance(transform.position, zoomedOffset) > zoomThreshold)
            {
                transform.position = Vector3.Lerp(transform.position, zoomedOffset, Time.deltaTime * zoomSpeed);
            }
            else
            {
                transform.position = zoomedOffset;
            }
        }
        else
        {
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, followSpeed);
            transform.position = smoothedPosition;
        }

        Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position);
        Quaternion adjustedRotation = Quaternion.Euler(rotationOffset.x, targetRotation.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, adjustedRotation, Time.deltaTime * rotationSpeed);
    }

    public void SetIdleCameraOffset(Vector3 offset)
    {
        this.offset = offset;
    }
}
