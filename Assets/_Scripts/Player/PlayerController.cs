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
    [SerializeField] private GameObject human;
    [SerializeField] private GameObject shadow;

    void Awake()
    {
        cam = Camera.main;
    } 
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();

        Vector3 verticalVector = cam.transform.forward * moveInput.y;
        Vector3 horizontalVector = cam.transform.right * moveInput.x;
        Vector3 moveVector = (verticalVector + horizontalVector);

        BroadcastMessage("Move", moveVector.normalized);
    }

    void OnSwitch(InputValue inputValue)
    {
        human.SetActive(!human.activeInHierarchy);
        shadow.SetActive(!human.activeInHierarchy);
    }
}
