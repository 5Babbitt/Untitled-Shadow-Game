using FiveBabbittGames;
using UnityEngine;

/// <summary>
/// ShadowMovement
/// </summary>
public class ShadowMovement : PlayerMovement
{
    private Rigidbody rb;

    [Header("Move Settings")]
    [SerializeField] private Vector3 targetPosition;
    private Quaternion targetRotation;

    [Header("Arc Cast Settings")]
    [SerializeField] float arcAngle = 270;
    private float arcRadius;
    [SerializeField] int arcResolution = 8;
    [SerializeField] private LayerMask groundLayers;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    void Start()
    {
        
    }

    protected override void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        //HandleMove();
    }

    protected override void HandleMove()
    {
        arcRadius = moveSpeed * moveVector.normalized.magnitude * Time.deltaTime;

        if (PhysicsUtils.ArcCast(transform.position, transform.rotation, arcAngle, arcRadius, arcResolution, groundLayers, out RaycastHit hit))
        {
            targetPosition = hit.point;
            targetRotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, Time.deltaTime * rotationSpeed);
        }

        //rb.MovePosition(targetPosition);
        //rb.MoveRotation(targetRotation);
        transform.position = targetPosition;
        transform.rotation = targetRotation;
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
        Gizmos.DrawSphere(transform.position + moveVector, 0.2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 3));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (transform.right * 3));

        Gizmos.color = Color.yellow;
        PhysicsUtils.ArcCast(transform.position, transform.rotation, arcAngle, arcRadius, arcResolution, groundLayers, out RaycastHit hit, drawGizmos: true);
        Gizmos.DrawSphere(targetPosition, 0.25f);
    }
}
