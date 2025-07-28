using UnityEngine;
using UnityEngine.InputSystem;

public class DoorHold : MonoBehaviour, IHoldInteractable
{
    public Transform doorPivot;
    public float minAngle = 0f;
    public float maxAngle = 100f;
    public float sensitivity = 0.15f;

    private bool isGrabbed;
    private float currentAngle;
    private int invertSign = 1;

    private void Awake()
    {
        if (!doorPivot) doorPivot = transform;
        currentAngle = doorPivot.localEulerAngles.y;
    }

    public void OnHoldStart(PlayerInteractor interactor)
    {
        Vector3 toPlayer = interactor.transform.position - doorPivot.position;
        float side = Vector3.Dot(doorPivot.right, toPlayer);
        invertSign = side > 0 ? -1 : 1;
        isGrabbed = true;
    }

    public void OnHold(PlayerInteractor interactor)
    {
        if (!isGrabbed) return;

        Vector2 delta = Mouse.current.delta.ReadValue();
        float deltaYaw = delta.x * sensitivity * invertSign;

        currentAngle = Mathf.Clamp(currentAngle + deltaYaw, minAngle, maxAngle);
        Vector3 euler = doorPivot.localEulerAngles;
        euler.y = currentAngle;
        doorPivot.localEulerAngles = euler;
    }

    public void OnHoldEnd(PlayerInteractor interactor)
    {
        isGrabbed = false;
    }
}