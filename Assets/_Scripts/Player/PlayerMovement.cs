using UnityEngine;

/// <summary>
/// PlayerMovement
/// </summary>
public abstract class PlayerMovement : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float rotationSpeed;
    protected Vector3 moveVector;

    protected virtual void Update()
    {
        HandleMove();
        HandleRotate(moveVector.normalized);

        HandleGravity();
    }

    public virtual void Switch(bool isEnabled)
    {

    }

    protected virtual void Move(Vector3 value)
    {
        moveVector = value * moveSpeed;
    }

    protected abstract void HandleMove();
    protected abstract void HandleGravity();
    protected void HandleRotate(Vector3 vector)
    {
        if (vector == Vector3.zero)
            return;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(vector, transform.up)), Time.deltaTime * rotationSpeed);
    }
}
