using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-50)]
public class PlayerInteractor : MonoBehaviour
{
    [Header("Refs")]
    public Camera playerCamera;
    public Transform holdPoint;

    [Header("Raycast")]
    public float interactDistance = 3f;
    public LayerMask interactLayer;

    private InputSystem_Actions input;
    private IHoldInteractable current;
    private bool holding;

    private void Awake()
    {
        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();
    }

    private void OnEnable()
    {
        if (input == null) input = new InputSystem_Actions();
        input.Player.Enable();

        input.Player.Primary.started += OnPrimaryStarted;
        input.Player.Primary.canceled += OnPrimaryCanceled;
    }

    private void OnDisable()
    {
        input.Player.Primary.started -= OnPrimaryStarted;
        input.Player.Primary.canceled -= OnPrimaryCanceled;
        input.Player.Disable();
    }

    private void Update()
    {
        if (holding && current != null)
        {
            current.OnHold(this);
        }
    }

    private void OnPrimaryStarted(InputAction.CallbackContext ctx)
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out var hit, interactDistance, interactLayer))
        {
            current = hit.collider.GetComponentInParent<IHoldInteractable>();
            if (current != null)
            {
                holding = true;
                current.OnHoldStart(this);
                return;
            }
        }

        holding = false;
        current = null;
    }

    private void OnPrimaryCanceled(InputAction.CallbackContext ctx)
    {
        if (holding && current != null)
        {
            current.OnHoldEnd(this);
        }
        holding = false;
        current = null;
    }
}
