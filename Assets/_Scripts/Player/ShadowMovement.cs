using UnityEngine;

/// <summary>
/// ShadowMovement
/// </summary>
public class ShadowMovement : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float moveSpeed;
    private Vector3 moveVector;

    void Awake()
    {
        
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

    }

    void Move(Vector3 value)
    {
        Debug.Log($"Shadow Moving {value}");
        moveVector = value;
    }

}
