using UnityEngine;

public class LogController : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 360f)]
    [Tooltip("degrees the log rotates per second")]
    private float rotationSpeed = 10f;

    private void FixedUpdate()
    {
        float normalizedRotationSpeed = rotationSpeed * Time.fixedDeltaTime;
        transform.Rotate(Vector3.forward, normalizedRotationSpeed);
    }
}
