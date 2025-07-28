using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Transform cam;
    public float sphereRadius = 0.1f;
    public float maxDistance = 0.5f;
    public LayerMask blockMask;
    public float smooth = 15f;

    private Vector3 defaultLocalPos;

    private void Awake()
    {
        if (!cam) cam = GetComponentInChildren<Camera>().transform;
        defaultLocalPos = cam.localPosition;
    }

    private void LateUpdate()
    {
        Vector3 desiredLocal = defaultLocalPos;

        Vector3 worldFrom = transform.position;
        Vector3 worldTo = transform.TransformPoint(defaultLocalPos);
        Vector3 dir = worldTo - worldFrom;
        float dist = dir.magnitude;

        if (Physics.SphereCast(worldFrom, sphereRadius, dir.normalized, out var hit, dist, blockMask, QueryTriggerInteraction.Ignore))
        {
            float t = Mathf.Clamp(hit.distance - 0.02f, 0f, maxDistance);
            desiredLocal = transform.InverseTransformPoint(worldFrom + dir.normalized * t);
        }

        cam.localPosition = Vector3.Lerp(cam.localPosition, desiredLocal, Time.deltaTime * smooth);
    }
}
