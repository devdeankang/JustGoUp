using UnityEngine;

public class UIRotate : MonoBehaviour
{
    public float turnSpeed;

    void Update()
    {
        transform.Rotate(Vector3.forward, turnSpeed * Time.deltaTime);
    }
}