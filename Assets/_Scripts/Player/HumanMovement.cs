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

    [Header("Crouch Settings")]
    [SerializeField] private float crouchSpeed;

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
        Rotate(moveVector.normalized);
    }

    void HandleMove()
    {
        controller.Move(moveVector * Time.deltaTime);
    }

    void Move(Vector3 value)
    {
        moveVector = Vector3.ProjectOnPlane(value.normalized, Vector3.up) * moveSpeed;
    }

    private void Rotate(Vector3 vector)
    {
        if (vector == Vector3.zero)
            return;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vector), Time.deltaTime * rotationSpeed);
    }
}
