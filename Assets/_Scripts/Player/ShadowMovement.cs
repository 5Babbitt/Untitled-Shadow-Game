using FiveBabbittGames;
using UnityEngine;

/// <summary>
/// ShadowMovement
/// </summary>
public class ShadowMovement : PlayerMovement
{
    private Rigidbody rb;
    bool isValid;

    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 targetDirection;
    [SerializeField] private Vector3 targetNormal;
    [SerializeField] private Quaternion targetRotation;
    private Quaternion lastTargetRotation;

    [Header("Arc Cast Settings")]
    [SerializeField] float arcAngle = 270;
    [SerializeField] private float arcRadius = 5;
    [SerializeField] int arcResolution = 8;
    [SerializeField] private LayerMask groundLayers;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        targetPosition = transform.position;
        targetRotation = transform.rotation;

        Debug.Log("Switched to shadow");
    }

    void Update()
    {
        HandleGravity();
    }

    private void FixedUpdate()
    {
        HandleRotate(moveVector.normalized);
        HandleMove();
    }

    protected override void Move(Vector3 value)
    {
        moveVector = Vector3.ProjectOnPlane(value.normalized, transform.up) * moveSpeed;
    }

    protected override void HandleMove()
    {
        float radius = arcRadius * moveVector.normalized.magnitude * Time.fixedDeltaTime;
        Quaternion rotation = moveVector != Vector3.zero ? Quaternion.LookRotation(moveVector.normalized, transform.up) : transform.rotation;

        if (PhysicsUtils.ArcCast(transform.position, rotation, arcAngle, radius, arcResolution, groundLayers, out RaycastHit hit))
        {
            targetPosition = hit.point;
            targetNormal = hit.normal;
            isValid = true;
        }
        else
        {
            isValid = false;
        }

        rb.MovePosition(MathUtils.LerpByDistance(transform.position, targetPosition, moveSpeed));
    }

    protected override void HandleRotate(Vector3 vector)
    {
        if (vector == Vector3.zero)
        {
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, lastTargetRotation, rotationSpeed * Time.fixedDeltaTime));
            return;
        }

        Vector3 forwardDirection = Vector3.ProjectOnPlane(moveVector.normalized, targetNormal);
        targetRotation = Quaternion.LookRotation(forwardDirection.normalized, targetNormal);
        lastTargetRotation = targetRotation;
        var currentRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        rb.MoveRotation(currentRotation);
    }

    protected override void HandleGravity()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + moveVector.normalized/2);
        Gizmos.DrawSphere(transform.position + moveVector.normalized / 2, 0.05f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 1));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (transform.right * 1));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (transform.up * 1));

        Quaternion rotation = moveVector != Vector3.zero ? Quaternion.LookRotation(moveVector.normalized, transform.up) : transform.rotation;

        Gizmos.color = isValid ? Color.yellow : Color.cyan;
        PhysicsUtils.ArcCast(transform.position, rotation, arcAngle, moveSpeed, arcResolution, groundLayers, out RaycastHit hit, drawGizmos: true);
        Gizmos.DrawSphere(hit.point, 0.05f);
        Gizmos.DrawLine(hit.point, hit.point + targetNormal.normalized);
    }
}
