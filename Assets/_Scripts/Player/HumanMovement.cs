using UnityEngine;

/// <summary>
/// HumanMovement
/// </summary>
public class HumanMovement : PlayerMovement
{
    CharacterController controller;
    Animator animator;
    [SerializeField] private float speedMultiplier = 1;

    [Header("Crouch Settings")]
    [SerializeField] private bool isCrouching;
    [SerializeField] private float walkHeight;
    [SerializeField] private float crouchHeight;
    [SerializeField] private float crouchSpeedModifier;

    [Header("Gravity Settings")]
    [SerializeField] private float gravityForce;

    public bool IsCrouching => isCrouching;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
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
        moveVector = Vector3.ProjectOnPlane(value.normalized, Vector3.up) * moveSpeed * speedMultiplier;
    }

    protected override void HandleMove()
    {
        controller.Move(moveVector * Time.deltaTime);
        animator.SetFloat("currentSpeed", controller.velocity.magnitude);
    }

    protected override void HandleRotate(Vector3 vector)
    {
        if (vector == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(vector, transform.up));

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    protected override void HandleGravity()
    {
        if (!controller.isGrounded)
        {
            controller.Move(gravityForce * Time.deltaTime * Vector3.down);
        }
    }

    void HandleCrouch()
    {
        if (controller.height == walkHeight)
            isCrouching = true;
        else
            isCrouching = false;

        Crouch(isCrouching);
    }

    void Crouch(bool value)
    {
        controller.height = isCrouching ? crouchHeight : walkHeight;
        controller.center = new Vector3(0, controller.height / 2, 0);

        speedMultiplier = isCrouching ? crouchSpeedModifier : 1f;

        animator.SetBool("isCrouching", isCrouching);
    }
}
