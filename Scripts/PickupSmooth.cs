using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PickupSmooth : MonoBehaviour, IHoldInteractable
{
    [Header("Movement")]
    public float followSpeed = 12f;        // �������� ������������
    public float holdDistance = 2f;        // ��������� �� ������
    public float throwForce = 8f;          // ���� ������

    [Header("Rotation")]
    public bool alignOnPickup = true;      // ����������� ��� �������
    public float alignSpeed = 8f;          // �������� ������������

    private Rigidbody rb;
    private bool isHeld;
    private PlayerInteractor interactor;
    private Quaternion targetRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnHoldStart(PlayerInteractor interactor)
    {
        this.interactor = interactor;
        isHeld = true;
        rb.useGravity = false;
        rb.linearDamping = 10f;

        if (alignOnPickup)
            targetRotation = Quaternion.identity; // ������ "�����������"
        else
            targetRotation = rb.rotation;
    }

    public void OnHold(PlayerInteractor interactor)
    {
        if (!isHeld) return;

        Vector3 targetPos = interactor.playerCamera.transform.position +
                            interactor.playerCamera.transform.forward * holdDistance;

        // ������ ����� �������
        Vector3 newPos = Vector3.Lerp(rb.position, targetPos, Time.deltaTime * followSpeed);
        rb.MovePosition(newPos);

        // ����������� ��������
        if (alignOnPickup)
        {
            Quaternion newRot = Quaternion.Lerp(rb.rotation, targetRotation, Time.deltaTime * alignSpeed);
            rb.MoveRotation(newRot);
        }
    }

    public void OnHoldEnd(PlayerInteractor interactor)
    {
        isHeld = false;
        rb.useGravity = true;
        rb.linearDamping = 0f;

        rb.AddForce(interactor.playerCamera.transform.forward * throwForce, ForceMode.Impulse);
        this.interactor = null;
    }
}