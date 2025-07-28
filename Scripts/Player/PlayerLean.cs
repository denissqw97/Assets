using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLean : MonoBehaviour
{
    public Transform cameraRoot;
    public float leanAmount = 0.3f;
    public float leanAngle = 10f;
    public float leanSpeed = 10f;

    private InputSystem_Actions input;
    private float target;
    private bool leftHeld;
    private bool rightHeld;

    private Vector3 defaultLocalPos;
    private Quaternion defaultLocalRot;

    private void Awake()
    {
        if (cameraRoot == null)
            cameraRoot = GetComponentInChildren<Camera>().transform;

        defaultLocalPos = cameraRoot.localPosition;
        defaultLocalRot = cameraRoot.localRotation;

        input = new InputSystem_Actions();
        input.Player.LeanLeft.started += _ => { leftHeld = true; UpdateTarget(); };
        input.Player.LeanLeft.canceled += _ => { leftHeld = false; UpdateTarget(); };
        input.Player.LeanRight.started += _ => { rightHeld = true; UpdateTarget(); };
        input.Player.LeanRight.canceled += _ => { rightHeld = false; UpdateTarget(); };
    }

    private void OnEnable() => input.Player.Enable();
    private void OnDisable() => input.Player.Disable();

    private void UpdateTarget()
    {
        if (leftHeld && !rightHeld) target = -1f;
        else if (!leftHeld && rightHeld) target = 1f;
        else target = 0f;
    }

    private void Update()
    {
        Vector3 desiredPos = defaultLocalPos + Vector3.right * (target * leanAmount);
        cameraRoot.localPosition = Vector3.Lerp(cameraRoot.localPosition, desiredPos, Time.deltaTime * leanSpeed);

        Quaternion desiredRot = defaultLocalRot * Quaternion.Euler(0f, 0f, -target * leanAngle);
        cameraRoot.localRotation = Quaternion.Slerp(cameraRoot.localRotation, desiredRot, Time.deltaTime * leanSpeed);
    }
}
