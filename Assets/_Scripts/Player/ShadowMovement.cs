using FiveBabbittGames;
using UnityEngine;

/// <summary>
/// ShadowMovement
/// </summary>
public class ShadowMovement : PlayerMovement
{
    private Rigidbody rb;

    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 targetNormal;
    [SerializeField] private Quaternion targetRotation;

    [Header("Arc Cast Settings")]
    [SerializeField] float arcAngle = 270;
    private float arcRadius;
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
        Debug.Log("Update on shadow");

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
        arcRadius = moveSpeed * moveVector.normalized.magnitude * Time.fixedDeltaTime;

        if (PhysicsUtils.ArcCast(transform.position, transform.rotation, arcAngle, arcRadius, arcResolution, groundLayers, out RaycastHit hit))
        {
            targetPosition = hit.point;
            targetNormal = hit.normal;
        }

        rb.MovePosition(targetPosition);
    }

    protected override void HandleRotate(Vector3 vector)
    {
        if (vector == Vector3.zero)
            return;

        Vector3 forwardDirection = Vector3.ProjectOnPlane(moveVector.normalized, targetNormal);
        Vector3 upDirection = targetNormal;
        Quaternion targetQuaternion = Quaternion.LookRotation(forwardDirection, upDirection);
        targetRotation = Quaternion.Slerp(transform.rotation, targetQuaternion, rotationSpeed * Time.fixedDeltaTime);

        rb.MoveRotation(targetRotation);
    }

    protected override void HandleGravity()
    {

    }

    public override void Switch(bool isEnabled)
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere((transform.position + moveVector) / 2, 0.2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 3));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (transform.right * 3));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (transform.up * 3));

        Gizmos.color = Color.yellow;
        PhysicsUtils.ArcCast(transform.position, transform.rotation, arcAngle, moveSpeed / 4, arcResolution, groundLayers, out RaycastHit hit, drawGizmos: true);
        Gizmos.DrawSphere(hit.point, 0.25f);
    }
}
