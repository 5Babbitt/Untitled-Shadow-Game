using UnityEngine;

/// <summary>
/// HumanMovement
/// </summary>
public class HumanMovement : PlayerMovement
{
    CharacterController controller;

    [Header("Crouch Settings")]
    [SerializeField] private float crouchSpeedModifier;

    [Header("Gravity Settings")]
    [SerializeField] private float gravityForce;
    
    void Awake()
    {
        controller = GetComponent<CharacterController>();
    } 
    
    void Start()
    {
        
    }

    protected override void Update()
    {
        base.Update();
    }

    void Move(Vector3 value)
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

    protected override void HandleGravity()
    {
        if (!controller.isGrounded)
        {
            controller.Move(gravityForce * Time.deltaTime * Vector3.down);
        }
    }
}
