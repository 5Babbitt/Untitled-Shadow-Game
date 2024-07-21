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
    [SerializeField] private GameObject human;
    [SerializeField] private GameObject shadow;

    void Awake()
    {

    } 
    
    void Start()
    {
        cam = Camera.main;

        Switch();
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

    void Switch()
    {
        if (isShadow)
            human.transform.position = shadow.transform.position;
        else
            shadow.transform.position = human.transform.position;

        isShadow = !isShadow;

        human.SetActive(!isShadow);
        shadow.SetActive(isShadow);

        human.GetComponent<PlayerMovement>().Switch(!isShadow);
        shadow.GetComponent<PlayerMovement>().Switch(isShadow);
    }

    void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
    }

    void OnCrouch(InputValue inputValue)
    {

    }

    void OnInteract(InputValue inputValue) 
    { 
    
    }

    void OnSwitch(InputValue inputValue)
    {
        Switch();
    }
}
