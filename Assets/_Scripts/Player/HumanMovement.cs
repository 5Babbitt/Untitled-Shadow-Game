using UnityEngine;

/// <summary>
/// HumanMovement
/// </summary>
public class HumanMovement : PlayerMovement
{
    CharacterController controller;

    [Header("Crouch Settings")]
    [SerializeField] private bool isCrouching;
    [SerializeField] private float crouchSpeedModifier;

    [Header("Gravity Settings")]
    [SerializeField] private float gravityForce;

    protected override void Awake()
    {
        base.Awake();
        controller = GetComponent<CharacterController>();
    } 
    
    void Start()
    {
        
    }

    void Update()
    {
        HandleMove();
        HandleRotate(moveVector.normalized);

        HandleGravity();
    }

    protected override void Move(Vector3 value)
    {
        moveVector = Vector3.ProjectOnPlane(value.normalized, Vector3.up) * moveSpeed;
    }

    public override void Switch(bool isEnabled)
    {

    }

    protected override void HandleMove()
    {
        controller.Move(moveVector * Time.deltaTime);
    }

    protected override void HandleRotate(Vector3 vector)
    {
        if (vector == Vector3.zero)
            return;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(vector, transform.up)), Time.deltaTime * rotationSpeed);
    }

    protected override void HandleGravity()
    {
        if (!controller.isGrounded)
        {
            controller.Move(gravityForce * Time.deltaTime * Vector3.down);
        }
    }
}
