using FiveBabbittGames;
using UnityEngine;

/// <summary>
/// ShadowMovement
/// </summary>
public class ShadowMovement : MonoBehaviour
{

    [Header("Move Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    private Vector3 targerPosition;
    private Vector3 moveVector;

    [Header("Arc Cast Settings")]
    [SerializeField] float arcAngle = 270;
    private float arcRadius;
    [SerializeField] int arcResolution = 8;
    [SerializeField] private LayerMask groundLayers;

    // Debug Settings

    void Awake()
    {
        
    } 
    
    void Start()
    {
        
    }

    void Update()
    {
        HandleMove();
        Rotate(moveVector.normalized);
    }

    void HandleMove()
    {
        arcRadius = moveSpeed * moveVector.normalized.magnitude;

        if (PhysicsUtils.ArcCast(transform.position, transform.rotation, arcAngle, arcRadius * Time.deltaTime, arcResolution, groundLayers, out RaycastHit hit))
        {
            targerPosition = hit.point;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, Time.deltaTime * rotationSpeed);
        }

    }

    private void Rotate(Vector3 vector)
    {
        if (vector == Vector3.zero)
            return;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(vector, transform.up)), Time.deltaTime * rotationSpeed);
    }

    void Move(Vector3 value)
    {
        moveVector = value * moveSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        PhysicsUtils.ArcCast(transform.position, transform.rotation, arcAngle, arcRadius, arcResolution, groundLayers, out RaycastHit hit, drawGizmos: true);
        Gizmos.DrawSphere(hit.point, 0.1f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position + moveVector, 0.2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 3));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (transform.right * 3));
    }
}
