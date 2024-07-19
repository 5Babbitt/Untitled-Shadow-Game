using FiveBabbittGames;
using UnityEngine;

/// <summary>
/// ShadowMovement
/// </summary>
public class ShadowMovement : MonoBehaviour
{
    // https://www.youtube.com/watch?v=OWvUWGHe9OY 

    [Header("Move Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    private Vector3 moveVector;

    [SerializeField] private LayerMask groundLayers;

    void Awake()
    {
        
    } 
    
    void Start()
    {
        
    }

    void Update()
    {
        HandleMove();
    }

    void HandleMove()
    {
        Rotate(moveVector.normalized);

        float arcAngle = 270;
        float arcRadius = moveSpeed * moveVector.normalized.magnitude;
        Debug.Log(moveVector.normalized.magnitude);
        int arcResolution = 8;

        if (PhysicsUtils.ArcCast(transform.position, transform.rotation, arcAngle, arcRadius * Time.deltaTime, arcResolution, groundLayers, out RaycastHit hit))
        {
            transform.position = hit.point;
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }
    }

    private void Rotate(Vector3 vector)
    {
        if (vector == Vector3.zero)
            return;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vector), Time.deltaTime * rotationSpeed);
    }

    void Move(Vector3 value)
    {
        Debug.Log($"Shadow Moving {value}");
        moveVector = new Vector3(value.x, 0, value.y) * moveSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        float arcAngle = 270;
        float arcRadius = moveSpeed;
        int arcResolution = 8;

        PhysicsUtils.ArcCast(transform.position, transform.rotation, arcAngle, arcRadius, arcResolution, groundLayers, out RaycastHit hit, drawGizmos: true);

        Gizmos.DrawSphere(hit.point, 0.1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right);
    }
}
