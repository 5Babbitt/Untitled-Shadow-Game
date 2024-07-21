using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// PlayerController
/// </summary>
public class PlayerController : MonoBehaviour
{
    Camera cam;

    [Header("Input Settings")]
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector2 lastMoveInput;

    [Header("Player Settings")]
    [SerializeField] private GameObject human;
    [SerializeField] private GameObject shadow;

    [Header("Gizmos Settings")]
    [SerializeField] private bool constrainLineToShadowUp;

    void Awake()
    {

    } 
    
    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // CalculateMoveVector(moveInput);
    }

    void CalculateMoveVector(Vector2 move, bool drawGizmos = false, bool contstrainToPlayerUp = false)
    {
        Vector3 verticalVector = Vector3.forward;
        Vector3 horizontalVector = Vector3.right;

        if (Application.isPlaying)
        {
            verticalVector = cam.transform.forward * move.y;
            horizontalVector = cam.transform.right * move.x;
        }

        Vector3 moveVector = (verticalVector + horizontalVector);

        if (drawGizmos)
        {
            Vector3 gizmosVector = (contstrainToPlayerUp) ? Vector3.ProjectOnPlane(moveVector, shadow.transform.up) : moveVector;

            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(shadow.transform.position, shadow.transform.position + (gizmosVector.normalized * 3));
        }

        if (Application.isPlaying)
            BroadcastMessage("Move", moveVector.normalized);
    }

    void OnMove(InputValue inputValue)
    {
        lastMoveInput = moveInput;
        moveInput = inputValue.Get<Vector2>();
        CalculateMoveVector(moveInput);
    }

    void OnSwitch(InputValue inputValue)
    {
        human.SetActive(!human.activeInHierarchy);
        shadow.SetActive(!human.activeInHierarchy);
    }

    private void OnDrawGizmos()
    {
        // CalculateMoveVector(moveInput, true, constrainLineToShadowUp);
    }
}
