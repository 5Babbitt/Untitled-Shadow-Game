using UnityEngine;

/// <summary>
/// HumanMovement
/// </summary>
public class HumanMovement : MonoBehaviour
{
    CharacterController controller;

    [Header("Move Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    private Vector3 moveVector;
    
    void Awake()
    {
        controller = GetComponent<CharacterController>();
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
        controller.Move(moveVector * Time.deltaTime);
        Rotate(moveVector.normalized);
    }

    void Move(Vector3 value)
    {
        Debug.Log($"Human Moving {value}");
        moveVector = Vector3.ProjectOnPlane(value.normalized, Vector3.up) * moveSpeed;
    }

    private void Rotate(Vector3 vector)
    {
        if (vector == Vector3.zero)
            return;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vector), Time.deltaTime * rotationSpeed);
    }
}
