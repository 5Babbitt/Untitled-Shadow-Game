using FiveBabbittGames;
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

    [Header("Player Settings")]
    [SerializeField] private bool isShadow;
    [SerializeField] private HumanMovement human;
    [SerializeField] private ShadowMovement shadow;

    private GameEvent onSound;

    void Awake()
    {
        human = GetComponentInChildren<HumanMovement>();
        shadow = GetComponentInChildren<ShadowMovement>();
    } 
    
    void Start()
    {
        cam = Camera.main;

        Switch(!isShadow);
    }

    void Update()
    {
        CalculateMoveVector(moveInput);
    }

    void CalculateMoveVector(Vector2 move)
    {
        Vector3 verticalVector = cam.transform.forward * move.y;
        Vector3 horizontalVector = cam.transform.right * move.x;
        Vector3 moveVector = (verticalVector + horizontalVector);

        BroadcastMessage("Move", moveVector.normalized);
    }

    void Switch(bool value)
    {
        if (isShadow && !shadow.CanSwitch())
        {
            Debug.Log("Unable to switch to human here");
            return;

        }

        if (value)
            human.transform.position = shadow.transform.position;
        else
            shadow.transform.position = human.transform.position;

        isShadow = !value;

        human.gameObject.SetActive(!isShadow);
        shadow.gameObject.SetActive(isShadow);
    }

    void Interact()
    {

    }

    void Crouch()
    {
        Debug.Log("Crouch");
        BroadcastMessage("HandleCrouch");
    }

    void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
    }

    void OnCrouch(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            Crouch();
        }
    }

    void OnInteract(InputValue inputValue) 
    {
        Debug.Log("Interact");
    }

    void OnSwitch(InputValue inputValue)
    {
        Switch(isShadow);
    }
}
